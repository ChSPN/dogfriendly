using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogFriendly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeolocatePlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_places_place_types_place_type_id",
                table: "places");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "places");

            migrationBuilder.AlterColumn<int>(
                name: "place_type_id",
                table: "places",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_places_latitude",
                table: "places",
                column: "latitude");

            migrationBuilder.CreateIndex(
                name: "IX_places_longitude",
                table: "places",
                column: "longitude");

            migrationBuilder.AddForeignKey(
                name: "FK_places_place_types_place_type_id",
                table: "places",
                column: "place_type_id",
                principalTable: "place_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_places_place_types_place_type_id",
                table: "places");

            migrationBuilder.DropIndex(
                name: "IX_places_latitude",
                table: "places");

            migrationBuilder.DropIndex(
                name: "IX_places_longitude",
                table: "places");

            migrationBuilder.AlterColumn<int>(
                name: "place_type_id",
                table: "places",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "type_id",
                table: "places",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_places_place_types_place_type_id",
                table: "places",
                column: "place_type_id",
                principalTable: "place_types",
                principalColumn: "id");
        }
    }
}
