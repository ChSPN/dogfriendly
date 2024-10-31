using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogFriendly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaceDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "opening_hours",
                table: "places",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "places",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "opening_hours",
                table: "places");

            migrationBuilder.DropColumn(
                name: "website",
                table: "places");
        }
    }
}
