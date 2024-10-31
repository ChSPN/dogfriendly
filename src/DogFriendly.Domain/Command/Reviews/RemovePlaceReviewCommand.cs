using MediatR;

namespace DogFriendly.Domain.Command.Reviews
{
    /// <summary>
    /// Remove place review command.
    /// </summary>
    public class RemovePlaceReviewCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }

        /// <summary>
        /// Gets or sets the review identifier.
        /// </summary>
        /// <value>
        /// The review identifier.
        /// </value>
        public int ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string? UserEmail { get; set; }
    }
}
