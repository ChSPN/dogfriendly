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
        private readonly IRepository<FavoriteListEntity> _favorites;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovePlaceFavoriteCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="placeFavorites">The place favorites.</param>
        /// <param name="favorites">The favorites.</param>
        public RemovePlaceFavoriteCommandHandler(IUnitOfWork unitOfWork,
            IRepository<PlaceFavoriteEntity> placeFavorites,
            IRepository<FavoriteListEntity> favorites)
        {
            _unitOfWork = unitOfWork;
            _placeFavorites = placeFavorites;
            _favorites = favorites;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(RemovePlaceFavoriteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isOwner = await _favorites
                    .AnyAsync(f => f.Id == request.FavoriteId
                        && f.User.Email == request.UserEmail, cancellationToken);
                if (!isOwner)
                {
                    return false;
                }

                int result;
                if (request.PlaceId == 0)
                {
                    await _placeFavorites
                        .RemoveAsync(f => f.FavoriteListId == request.FavoriteId, cancellationToken);
                    result = await _favorites
                        .RemoveAsync(f => f.Id == request.FavoriteId, cancellationToken);
                }
                else 
                {
                    result = await _placeFavorites
                        .RemoveAsync(f => f.FavoriteListId == request.FavoriteId
                            && f.PlaceId == request.PlaceId, cancellationToken);
                }

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
