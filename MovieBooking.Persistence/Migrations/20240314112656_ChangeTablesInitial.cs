using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTablesInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "theaterMains",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "theaterMains");
        }
    }
}
