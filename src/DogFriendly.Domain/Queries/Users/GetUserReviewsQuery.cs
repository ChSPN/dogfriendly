using DogFriendly.Domain.ViewModels.Users;
using MediatR;

namespace DogFriendly.Domain.Queries.Users
{
    /// <summary>
    /// Query for user reviews.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Users.UserReviewViewModel&gt;&gt;" />
    public class GetUserReviewsQuery : IRequest<List<UserReviewViewModel>>
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public required string Email { get; set; }
    }
}
