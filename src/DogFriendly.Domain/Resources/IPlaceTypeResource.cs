using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Users;
using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Resource for place type.
    /// </summary>
    public interface IPlaceTypeResource
    {
        /// <summary>
        /// Gets all places types for view.
        /// </summary>
        /// <returns></returns>
        [Get("/api/placetype/view")]
        Task<List<PlaceTypeViewModel>> GetViewAll();
    }
}
