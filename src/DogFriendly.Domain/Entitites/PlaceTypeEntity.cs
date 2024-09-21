namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Place type entity.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Entitites.EntityBase" />
    public class PlaceTypeEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string? Color { get; set; }

        /// <summary>
        /// Gets or sets the icon URI.
        /// </summary>
        /// <value>
        /// The icon URI.
        /// </value>
        public required string IconUri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the places.
        /// </summary>
        /// <value>
        /// The places.
        /// </value>
        public ICollection<PlaceEntity>? Places { get; set; }
    }
}
