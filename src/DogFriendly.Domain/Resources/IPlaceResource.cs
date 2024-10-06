using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;
using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Resource for place.
    /// </summary>
    public interface IPlaceResource
    {
        /// <summary>
        /// Adds the review.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Post("/api/place/review")]
        Task<List<PlaceReviewViewModel>> AddReview([Body] AddPlaceReviewCommand request);

        /// <summary>
        /// Gets the place reviews.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Get("/api/place/reviews/{id}")]
        Task<List<PlaceReviewViewModel>> GetPlaceReviews(int id);

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Get("/api/place/view/{id}")]
        Task<PlaceViewModel> GetViewModel(int id);

        /// <summary>
        /// Adds the favorite.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="favoriteName">Name of the favorite.</param>
        /// <returns></returns>
        [Multipart]
        [Post("/api/place/{placeId}/favorite")]
        Task<int?> PostFavorite(int placeId, [AliasAs("favoriteName")] string favoriteName);

        /// <summary>
        /// Adds the favorite.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="favoriteId">The favorite identifier.</param>
        /// <returns></returns>
        [Multipart]
        [Put("/api/place/{placeId}/favorite")]
        Task<int?> PutFavorite(int placeId, [AliasAs("favoriteId")] int favoriteId);

        /// <summary>
        /// Removes the favorite.
        /// </summary>
        /// <param name="placeId">Place identifier.</param>
        /// <param name="favoriteId">Favorite identifier.</param>
        /// <returns></returns>
        [Delete("/api/place/{placeId}/favorite/{favoriteId}")]
        Task<bool> RemoveFavorite(int placeId, int favoriteId);

        /// <summary>
        /// Searches the specified places.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Post("/api/place/search")]
        Task<List<PlaceListViewModel>> Search([Body] PlaceListViewQuery request);
    }
}
