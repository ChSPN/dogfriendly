using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Resource for place.
    /// </summary>
    public interface IPlaceResource
    {
        /// <summary>
        /// Searches the specified places.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Post("/api/place/search")]
        Task<List<PlaceListViewModel>> Search([Body] PlaceListViewQuery request);
    }
}
