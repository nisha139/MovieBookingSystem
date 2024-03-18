using Serilog;
using MovieBooking.Api;
using MovieBooking.Identity.Database;
using MovieBooking.Persistence.Database;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    try
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(new CompactJsonFormatter(), GetLogFilePath(), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .WriteTo.MSSqlServer(context.Configuration.GetConnectionString("DefaultConnection"), "Logs", autoCreateSqlTable: true); // Adjust connectionString and tableName according to your database setup
    }
    catch (System.Data.SqlClient.SqlException ex)
    {
        // Handle the SQL exception appropriately
        // Log the exception or take necessary actions
        // For example:
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.Console()
            .CreateLogger();

        Log.Error(ex, "Error occurred while configuring MSSqlServer sink: {Message}", ex.Message);

        // If you want to terminate the application, you can throw the exception again
        throw;
    }
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:5174", "http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// Rest of your code...

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();
//app.UseAuthentication();
//app.MapControllers();
//seed data
await app.Services.InitialiseAppDatabaseAsync();
await app.Services.InitialiseDatabaseAsync();
app.UseSerilogRequestLogging();
app.UseCors(); // This should be placed here

app.Run();

static string GetLogFilePath()
{
    // Get the calling method name
    var callingMethod = GetCallingMethodName();
    // Construct the log file path based on the method name
    return $"log/{callingMethod}-log-.txt";
}

static string GetCallingMethodName()
{
    // Get the calling method name by inspecting the call stack
    var stackTrace = new System.Diagnostics.StackTrace();
    var callingMethod = stackTrace.GetFrame(2)?.GetMethod();
    return callingMethod != null ? $"{callingMethod.DeclaringType?.Name}-{callingMethod.Name}" : "Unknown";
}
