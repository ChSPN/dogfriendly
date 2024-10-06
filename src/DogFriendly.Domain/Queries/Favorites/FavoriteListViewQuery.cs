using DogFriendly.Domain.ViewModels.Favorites;
using MediatR;

namespace DogFriendly.Domain.Queries.Favorites
{
    /// <summary>
    /// Query for favorite list.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Favorites.FavoriteListViewModel&gt;&gt;" />
    public class FavoriteListViewQuery : IRequest<List<FavoriteListViewModel>>
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public required string UserEmail { get; set; }
    }
}
