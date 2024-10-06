using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;

namespace DogFriendly.Application.Command.Favorites
{
    /// <summary>
    /// Command handler for removing a place to favorites.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Favorites.RemovePlaceFavoriteCommand, System.Boolean&gt;" />
    public class RemovePlaceFavoriteCommandHandler : IRequestHandler<RemovePlaceFavoriteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PlaceFavoriteEntity> _placeFavorites;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovePlaceFavoriteCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="placeFavorites">The place favorites.</param>
        public RemovePlaceFavoriteCommandHandler(IUnitOfWork unitOfWork,
            IRepository<PlaceFavoriteEntity> placeFavorites)
        {
            _unitOfWork = unitOfWork;
            _placeFavorites = placeFavorites;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(RemovePlaceFavoriteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isOwner = await _placeFavorites
                    .AnyAsync(f => f.FavoriteListId == request.FavoriteId
                        && f.FavoriteList.User.Email == request.UserEmail, cancellationToken);
                if (!isOwner)
                {
                    return false;
                }

                var result = await _placeFavorites
                    .RemoveAsync(f => f.FavoriteListId == request.FavoriteId
                        && f.PlaceId == request.PlaceId, cancellationToken);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return result > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
    }
}
