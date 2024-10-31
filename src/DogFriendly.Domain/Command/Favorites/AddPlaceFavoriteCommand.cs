using MediatR;

namespace DogFriendly.Domain.Command.Favorites
{
    /// <summary>
    /// Command for adding a place to favorites.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Boolean&gt;" />
    public class AddPlaceFavoriteCommand : IRequest<int?>
    {
        /// <summary>
        /// Gets or sets the favorite identifier.
        /// </summary>
        /// <value>
        /// The favorite identifier.
        /// </value>
        public int? FavoriteId { get; set; }

        /// <summary>
        /// Gets or sets the name of the favorite.
        /// </summary>
        /// <value>
        /// The name of the favorite.
        /// </value>
        public string? FavoriteName { get; set; }

        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public required string UserEmail { get; set; }
    }
}
