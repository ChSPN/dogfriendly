using DogFriendly.Domain.Entitites;
using MediatR;

namespace DogFriendly.Domain.Queries.Reviews
{
    /// <summary>
    /// Get place review query.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;DogFriendly.Domain.Entitites.ReviewEntity&gt;&gt;" />
    public class GetPlaceReviewQuery : IRequest<List<ReviewEntity>>
    {
        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }
    }
}
