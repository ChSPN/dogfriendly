using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;

namespace DogFriendly.Domain.ViewModels.Users
{
    /// <summary>
    /// View model for user review.
    /// </summary>
    public class UserReviewViewModel
    {
        /// <summary>
        /// Gets or sets the review.
        /// </summary>
        /// <value>
        /// The review.
        /// </value>
        public PlaceReviewViewModel Review { get; set; }

        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        /// <value>
        /// The place.
        /// </value>
        public PlaceListViewModel Place { get; set; }
    }
}
