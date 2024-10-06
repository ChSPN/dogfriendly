using MediatR;

namespace DogFriendly.Domain.Command.Favorites
{
    /// <summary>
    /// Command for removing a place to favorites.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Boolean&gt;" />
    public class RemovePlaceFavoriteCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the favorite identifier.
        /// </summary>
        /// <value>
        /// The favorite identifier.
        /// </value>
        public int FavoriteId { get; set; }

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
