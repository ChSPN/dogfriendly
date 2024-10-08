using DogFriendly.Domain.ViewModels.Users;
using MediatR;

namespace DogFriendly.Domain.Queries.Users
{
    /// <summary>
    /// Query for favorite list.
    /// </summary>
    /// <seealso cref="IRequest&lt;List&lt;UserFavoriteViewModel&gt;&gt;" />
    public class GetUserFavoritesQuery : IRequest<List<UserFavoriteViewModel>>
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public required string Email { get; set; }
    }
}
