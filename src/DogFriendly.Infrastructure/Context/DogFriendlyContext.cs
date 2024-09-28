using CaseExtensions;
using DogFriendly.Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Infrastructure.Context
{
    /// <summary>
    /// Database context for DogFriendly application.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DogFriendlyContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DogFriendlyContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DogFriendlyContext(DbContextOptions<DogFriendlyContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Amenity entities.
        /// </summary>
        public DbSet<AmenityEntity> Amenities { get; set; }

        /// <summary>
        /// Favorite list entities.
        /// </summary>
        public DbSet<FavoriteListEntity> Favorites { get; set; }

        /// <summary>
        /// News entities.
        /// </summary>
        public DbSet<NewsEntity> News { get; set; }

        /// <summary>
        /// Place amenity entities.
        /// </summary>
        public DbSet<PlaceAmenityEntity> PlaceAmenities { get; set; }

        /// <summary>
        /// Place entities.
        /// </summary>
        public DbSet<PlaceEntity> Places { get; set; }

        /// <summary>
        /// Place favorite entities.
        /// </summary>
        public DbSet<PlaceFavoriteEntity> PlaceFavorites { get; set; }

        /// <summary>
        /// Place news entities.
        /// </summary>
        public DbSet<PlaceNewsEntity> PlaceNews { get; set; }

        /// <summary>
        /// Place types.
        /// </summary>
        public DbSet<PlaceTypeEntity> PlaceTypes { get; set; }

        /// <summary>
        /// Review entities.
        /// </summary>
        public DbSet<ReviewEntity> Reviews { get; set; }

        /// <summary>
        /// User entities.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <ineheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Compose the primary keys for many-to-many relationships.
            modelBuilder.Entity<PlaceAmenityEntity>()
                .HasKey(pa => new { pa.PlaceId, pa.AmenityId });
            modelBuilder.Entity<PlaceFavoriteEntity>()
                .HasKey(pa => new { pa.PlaceId, pa.FavoriteListId });
            modelBuilder.Entity<PlaceNewsEntity>()
                .HasKey(pa => new { pa.PlaceId, pa.NewsId });

            // Use snake case for table names.
            modelBuilder.Entity<AmenityEntity>().ToTable("amenities");
            modelBuilder.Entity<FavoriteListEntity>().ToTable("favorites");
            modelBuilder.Entity<NewsEntity>().ToTable("news");
            modelBuilder.Entity<PlaceAmenityEntity>().ToTable("place_amenities");
            modelBuilder.Entity<PlaceEntity>().ToTable("places");
            modelBuilder.Entity<PlaceFavoriteEntity>().ToTable("place_favorites");
            modelBuilder.Entity<PlaceNewsEntity>().ToTable("place_news");
            modelBuilder.Entity<PlaceTypeEntity>().ToTable("place_types");
            modelBuilder.Entity<ReviewEntity>().ToTable("reviews");
            modelBuilder.Entity<UserEntity>().ToTable("users");

            // Define unique indexes for UserEntity
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Use snake case for column names.
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToSnakeCase());
                }
            }
        }
    }
}
