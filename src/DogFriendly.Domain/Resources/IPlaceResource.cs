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
        /// Gets the view model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Get("/api/place/view/{id}")]
        Task<PlaceViewModel> GetViewModel(int id);

        /// <summary>
        /// Searches the specified places.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Post("/api/place/search")]
        Task<List<PlaceListViewModel>> Search([Body] PlaceListViewQuery request);
    }
}
