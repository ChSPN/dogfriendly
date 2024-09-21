namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Favorite list entity.
    /// </summary>
    public class FavoriteListEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the place favorites.
        /// </summary>
        /// <value>
        /// The place favorites.
        /// </value>
        public ICollection<PlaceFavoriteEntity>? PlaceFavorites { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public UserEntity? User { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }
    }
}
