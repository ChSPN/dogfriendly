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

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION GetKilometersPlaces(km double precision, lat double precision, long double precision)
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
					created_at timestamp with time zone,
					created_by text,
					updated_at timestamp with time zone,
					updated_by text,
                    opening_hours text,
                    website text
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
						   p.name, p.phone, p.photos, p.place_type_id, p.postal_code, p.created_at, p.created_by, p.updated_at, p.updated_by, p.opening_hours, p.website
					FROM public.places p
					WHERE (6371.0 * 2 * ATAN2(
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
						   )) <= km
					ORDER BY (6371.0 * 2 * ATAN2(
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
						   ));
				END;
				$$ LANGUAGE plpgsql;
			");
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

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION GetKilometersPlaces(km double precision, lat double precision, long double precision)
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
						   p.name, p.phone, p.photos, p.place_type_id, p.postal_code, p.created_at, p.created_by, p.updated_at, p.updated_by
					FROM public.places p
					WHERE (6371.0 * 2 * ATAN2(
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
						   )) <= km
					ORDER BY (6371.0 * 2 * ATAN2(
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
						   ));
				END;
				$$ LANGUAGE plpgsql;
			");
        }
    }
}
