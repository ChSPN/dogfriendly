using DogFriendly.Domain.ViewModels;
using DogFriendly.Domain.ViewModels.Users;
using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Interface for user resource.
    /// </summary>
    public interface IUserResource
    {
        /// <summary>
        /// Gets the place reviews.
        /// </summary>
        /// <returns></returns>
        [Get("/api/user/reviews")]
        Task<List<UserReviewViewModel>> GetPlaceReviews();

        /// <summary>
        /// Gets the profil.
        /// </summary>
        /// <returns></returns>
        [Get("/api/user/profil")]
        Task<UserProfilViewModel> GetProfil();

        /// <summary>
        /// Determines whether this user is exist.
        /// </summary>
        /// <returns>The user exist.</returns>
        [Get("/api/user/exist")]
        Task<bool> IsExist();

        /// <summary>
        /// Registers the specified register user.
        /// </summary>
        /// <param name="registerUserModel">The register user.</param>
        /// <returns>Response of register user.</returns>
        [Post("/api/user")]
        Task<ResponseViewModel> Register([Body] UserProfilViewModel registerUserModel);

        /// <summary>
        /// Updates the specified register user model.
        /// </summary>
        /// <param name="registerUserModel">The register user model.</param>
        /// <returns></returns>
        [Put("/api/user")]
        Task<ResponseViewModel> Update([Body] UserProfilViewModel registerUserModel);
    }
}
