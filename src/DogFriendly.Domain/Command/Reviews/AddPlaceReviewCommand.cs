using DogFriendly.Domain.Entitites;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DogFriendly.Domain.Command.Reviews
{
    /// <summary>
    /// Add place review command.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;DogFriendly.Domain.Entitites.ReviewEntity&gt;" />
    public class AddPlaceReviewCommand : IRequest<ReviewEntity>
    {
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string? UserEmail { get; set; }
    }
}
