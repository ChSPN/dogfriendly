using DogFriendly.Domain.ViewModels.Amenities;
using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Resource for place type.
    /// </summary>
    public interface IAmenityResource
    {
        /// <summary>
        /// Gets all places types for view.
        /// </summary>
        /// <returns></returns>
        [Get("/api/amenity/view")]
        Task<List<AmenityListViewModel>> GetViewAll();
    }
}
