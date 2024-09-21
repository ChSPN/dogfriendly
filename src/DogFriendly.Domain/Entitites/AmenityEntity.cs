namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Amenity entity.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Entitites.EntityBase" />
    public class AmenityEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the icon URI.
        /// </summary>
        /// <value>
        /// The icon URI.
        /// </value>
        public string? IconUri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the place amenities.
        /// </summary>
        /// <value>
        /// The place amenities.
        /// </value>
        public ICollection<PlaceAmenityEntity>? PlaceAmenities { get; set; }
    }
}
