using MovieBooking.Api;
using MovieBooking.Identity.Database;
using MovieBooking.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder
       .ConfigureServices()
       .ConfigurePipeline();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//seed data
await app.Services.InitialiseAppDatabaseAsync();
await app.Services.InitialiseDatabaseAsync();


app.Run();
