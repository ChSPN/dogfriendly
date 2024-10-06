using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Command.Reviews
{
    /// <summary>
    /// Handler for <see cref="AddPlaceReviewCommand"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Reviews.AddPlaceReviewCommand, DogFriendly.Domain.Entitites.ReviewEntity&gt;" />
    public class AddPlaceReviewCommandHandler : IRequestHandler<AddPlaceReviewCommand, ReviewEntity>
    {
        private readonly IRepository<ReviewEntity> _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPlaceReviewCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="reviewRepository">The review repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public AddPlaceReviewCommandHandler(IUnitOfWork unitOfWork,
            IRepository<ReviewEntity> reviewRepository,
            IRepository<UserEntity> userRepository)
        {
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<ReviewEntity> Handle(AddPlaceReviewCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var queryUser = _userRepository
                    .SingleResultQuery()
                    .AndFilter(u => u.Email == request.UserEmail);
                var user = await _userRepository
                    .ToQueryable(queryUser)
                    .FirstAsync();
                var review = new ReviewEntity
                {
                    CreatedBy = request.UserEmail,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Comment = request.Comment,
                    PlaceId = request.PlaceId,
                    Rating = request.Rating,
                    UserId = user.Id
                };
                review = await _reviewRepository.AddAsync(review, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                review.User = user;
                return review;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
