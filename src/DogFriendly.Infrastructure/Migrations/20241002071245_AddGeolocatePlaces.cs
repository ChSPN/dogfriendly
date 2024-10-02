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

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION GetKilometersPlaces(lat double precision, long double precision)
                RETURNS TABLE (
                    id integer,
                    address text,
                    city text,
                    country text,
                    description text,
                    email text,
                    latitude double precision,
                    longitude double precision,
                    kilometers double precision,
                    name text,
                    phone text,
                    photos text[],
                    place_type_id integer,
                    postal_code text,
                    type_id integer,
                    created_at timestamp with time zone,
                    created_by text,
                    updated_at timestamp with time zone,
                    updated_by text
                ) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT p.id, p.address, p.city, p.country, p.description, p.email, p.latitude, p.longitude,
                           (6371.0 * 2 * ATAN2(
                               SQRT(
                                   POWER(SIN(RADIANS(p.latitude - lat) / 2), 2) + 
                                   COS(RADIANS(lat)) * COS(RADIANS(p.latitude)) * 
                                   POWER(SIN(RADIANS(p.longitude - long) / 2), 2)
                               ), 
                               SQRT(1 - (
                                   POWER(SIN(RADIANS(p.latitude - lat) / 2), 2) + 
                                   COS(RADIANS(lat)) * COS(RADIANS(p.latitude)) * 
                                   POWER(SIN(RADIANS(p.longitude - long) / 2), 2)
                               ))
                           )) as kilometers,
                           p.name, p.phone, p.photos, p.place_type_id, p.postal_code, p.type_id, p.created_at, p.created_by, p.updated_at, p.updated_by
                    FROM public.places p;
                END;
                $$ LANGUAGE plpgsql;
            ");
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
            
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS GetKilometersPlaces(double precision, double precision);");
        }
    }
}
