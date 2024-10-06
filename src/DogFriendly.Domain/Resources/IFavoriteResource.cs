using DogFriendly.Domain.ViewModels.Favorites;
using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Resource for favorite.
    /// </summary>
    public interface IFavoriteResource
    {
        /// <summary>
        /// Gets all favorites for view.
        /// </summary>
        /// <returns></returns>
        [Get("/api/favorite/view")]
        Task<List<FavoriteListViewModel>> GetViewAll();
    }
}
