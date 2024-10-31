using MediatR;

namespace DogFriendly.Domain.Models
{
    /// <summary>
    /// Review model.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Models.ModelBase" />
    public class ReviewModel : ModelBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewModel"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="createdAt">The created at.</param>
        public ReviewModel(IMediator mediator, 
            int placeId = 0, 
            DateTimeOffset? createdAt = null)
        {
            _mediator = mediator;
            Id = placeId;
            if (createdAt.HasValue)
            {
                CreatedAt = createdAt.Value;
            }
        }

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
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public UserModel? User { get; set; }
    }
}
