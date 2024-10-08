using DogFriendly.Domain.ViewModels.Places;
using MediatR;

namespace DogFriendly.Domain.Queries.Users
{
    /// <summary>
    /// Query for favorite places.
    /// </summary>
    /// <seealso cref="IRequest&lt;List&lt;PlaceListViewModel&gt;&gt;" />
    public class GetUserPlacesQuery : IRequest<List<PlaceListViewModel>>
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the favorite identifier.
        /// </summary>
        /// <value>
        /// The favorite identifier.
        /// </value>
        public required int FavoriteId { get; set; }
    }
}
