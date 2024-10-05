using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Reviews;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Reviews
{
    /// <summary>
    /// Handler for <see cref="GetPlaceReviewQuery"/> query.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Reviews.GetPlaceReviewQuery, System.Collections.Generic.List&lt;DogFriendly.Domain.Entitites.ReviewEntity&gt;&gt;" />
    public class GetPlaceReviewQueryHandler : IRequestHandler<GetPlaceReviewQuery, List<ReviewEntity>>
    {
        private readonly IRepository<ReviewEntity> _reviewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlaceReviewQueryHandler"/> class.
        /// </summary>
        /// <param name="reviewRepository">The review repository.</param>
        public GetPlaceReviewQueryHandler(IRepository<ReviewEntity> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        /// <inheritdoc />
        public async Task<List<ReviewEntity>> Handle(GetPlaceReviewQuery request, CancellationToken cancellationToken)
        {
            var query = _reviewRepository
                .SingleResultQuery()
                .AndFilter(x => x.PlaceId == request.PlaceId);
            return await _reviewRepository
                .ToQueryable(query)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
