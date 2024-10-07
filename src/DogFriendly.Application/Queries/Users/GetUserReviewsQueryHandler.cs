using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Users;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;
using DogFriendly.Domain.ViewModels.Users;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Users
{
    /// <summary>
    /// Handler for <see cref="GetUserReviewsQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Users.GetUserReviewsQuery, System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Users.UserReviewViewModel&gt;&gt;" />
    public class GetUserReviewsQueryHandler : IRequestHandler<GetUserReviewsQuery, List<UserReviewViewModel>>
    {
        private readonly IRepository<UserEntity> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserReviewsQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public GetUserReviewsQueryHandler(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc>
        public async Task<List<UserReviewViewModel>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
        {
            var query = _userRepository
                .SingleResultQuery()
                .AndFilter(x => x.Email == request.Email);
            return await _userRepository
                .ToQueryable(query)
                .SelectMany(x => x.Reviews)
                .Select(r => new UserReviewViewModel
                {
                    Review = new PlaceReviewViewModel
                    {
                        Id = r.Id,
                        PlaceId = r.PlaceId,
                        Rating = r.Rating,
                        CreatedAt = r.CreatedAt,
                        Comment = r.Comment,
                    },
                    Place = new PlaceListViewModel 
                    {
                        Id = r.Place.Id,
                        Name = r.Place.Name,
                        Latitude = r.Place.Latitude,
                        Longitude = r.Place.Longitude,
                        Description = r.Place.Description.Substring(0, r.Place.Description.LastIndexOf(' ', 100)) + " ...",
                        PlaceTypeId = r.Place.PlaceTypeId,
                        Photos = r.Place.Photos,
                        Rating = r.Place.Reviews.Any()
                            ? r.Place.Reviews.Sum(r => r.Rating) / r.Place.Reviews.Count()
                            : 0,
                    }
                })
                .OrderByDescending(x => x.Review.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
