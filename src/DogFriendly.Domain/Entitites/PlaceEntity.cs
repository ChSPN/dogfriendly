using System.ComponentModel.DataAnnotations.Schema;

namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Place entity.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Entitites.EntityBase" />
    public class PlaceEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public required string Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public required string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public required string Country { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

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
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the photos.
        /// </summary>
        /// <value>
        /// The photos.
        /// </value>
        public List<string> Photos { get; set; } = [];

        /// <summary>
        /// Gets or sets the place amenities.
        /// </summary>
        /// <value>
        /// The place amenities.
        /// </value>
        public ICollection<PlaceAmenityEntity>? PlaceAmenities { get; set; }

        /// <summary>
        /// Gets or sets the place favorites.
        /// </summary>
        /// <value>
        /// The place favorites.
        /// </value>
        public ICollection<PlaceFavoriteEntity>? PlaceFavorites { get; set; }

        /// <summary>
        /// Gets or sets the place news.
        /// </summary>
        /// <value>
        /// The place news.
        /// </value>
        public ICollection<PlaceNewsEntity>? PlaceNews { get; set; }

        /// <summary>
        /// Gets or sets the type of the place.
        /// </summary>
        /// <value>
        /// The type of the place.
        /// </value>
        public PlaceTypeEntity? PlaceType { get; set; }

        /// <summary>
        /// Gets or sets the place type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public int PlaceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public required string PostalCode { get; set; }
    }
}
