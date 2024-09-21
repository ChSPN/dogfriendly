namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Place amenity entity.
    /// </summary>
    public class PlaceAmenityEntity
    {
        /// <summary>
        /// Gets or sets the amenity identifier.
        /// </summary>
        /// <value>
        /// The amenity identifier.
        /// </value>
        public int AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the amenity.
        /// </summary>
        /// <value>
        /// The amenity.
        /// </value>
        public AmenityEntity? Amenity { get; set; }

        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }

        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        /// <value>
        /// The place.
        /// </value>
        public PlaceEntity? Place { get; set; }
    }
}
