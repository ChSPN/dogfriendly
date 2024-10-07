using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;

namespace DogFriendly.Application.Command.Reviews
{
    /// <summary>
    /// Handler for <see cref="RemovePlaceReviewCommand"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Reviews.RemovePlaceReviewCommand, bool&gt;" />
    public class RemovePlaceReviewCommandHandler : IRequestHandler<RemovePlaceReviewCommand, bool>
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
        public RemovePlaceReviewCommandHandler(IUnitOfWork unitOfWork,
            IRepository<ReviewEntity> reviewRepository,
            IRepository<UserEntity> userRepository)
        {
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(RemovePlaceReviewCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var review = await _reviewRepository
                    .RemoveAsync(r => r.User.Email == request.UserEmail 
                        && r.Id == request.ReviewId
                        && r.PlaceId == request.PlaceId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return review > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
