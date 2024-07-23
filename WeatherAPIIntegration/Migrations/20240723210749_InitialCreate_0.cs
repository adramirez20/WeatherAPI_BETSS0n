using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherAPIIntegration.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Users");
        }
    }
}
