using DogFriendly.Domain.ViewModels.Favorites;

namespace DogFriendly.Domain.ViewModels.Places
{
    /// <summary>
    /// View model for place list.
    /// </summary>
    public class PlaceListViewModel
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the favorite.
        /// </summary>
        /// <value>
        /// The favorite.
        /// </value>
        public PlaceFavoriteViewModel? Favorite { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the kilometers.
        /// </summary>
        /// <value>
        /// The kilometers.
        /// </value>
        public double Kilometers { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public required string Name { get; set; }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <value>
        /// The photo.
        /// </value>
        public string? Photo { get => Photos?.FirstOrDefault(); }

        /// <summary>
        /// Gets or sets the photos.
        /// </summary>
        /// <value>
        /// The photos.
        /// </value>
        public List<string>? Photos { get; set; }

        /// <summary>
        /// Gets or sets the place type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public int PlaceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public double Rating { get; set; }
    }
}
