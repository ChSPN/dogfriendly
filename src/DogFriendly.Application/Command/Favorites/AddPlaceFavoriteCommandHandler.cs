using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Command.Favorites
{
    /// <summary>
    /// Command handler for adding a place to favorites.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Favorites.AddPlaceFavoriteCommand, System.Boolean&gt;" />
    public class AddPlaceFavoriteCommandHandler : IRequestHandler<AddPlaceFavoriteCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PlaceFavoriteEntity> _placeFavorites;
        private readonly IRepository<FavoriteListEntity> _favorites;
        private readonly IRepository<UserEntity> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPlaceFavoriteCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="placeFavorites">The place favorites.</param>
        /// <param name="favorites">The favorites.</param>
        /// <param name="users">The users.</param>
        public AddPlaceFavoriteCommandHandler(IUnitOfWork unitOfWork, 
            IRepository<PlaceFavoriteEntity> placeFavorites,
            IRepository<FavoriteListEntity> favorites,
            IRepository<UserEntity> users)
        {
            _unitOfWork = unitOfWork;
            _placeFavorites = placeFavorites;
            _favorites = favorites;
            _users = users;
        }

        /// <inheritdoc />
        public async Task<int?> Handle(AddPlaceFavoriteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                PlaceFavoriteEntity placeFavorite = null;
                if (request.FavoriteId.HasValue)
                {
                    var isOwner = await _favorites
                        .AnyAsync(f => f.Id == request.FavoriteId.Value
                            && f.User.Email == request.UserEmail, cancellationToken);
                    if (!isOwner)
                    {
                        return null;
                    }

                    placeFavorite = await _placeFavorites.AddAsync(new PlaceFavoriteEntity
                    {
                        FavoriteListId = request.FavoriteId.Value,
                        PlaceId = request.PlaceId
                    }, cancellationToken);
                }
                else if (!string.IsNullOrWhiteSpace(request.FavoriteName))
                {
                    var userQuery = _users
                        .SingleResultQuery()
                        .AndFilter(u => u.Email == request.UserEmail);
                    var userId = await _users
                        .ToQueryable(userQuery)
                        .Select(u => u.Id)
                        .FirstAsync(cancellationToken);
                    var favorite = new FavoriteListEntity
                    {
                        CreatedBy = request.UserEmail,
                        CreatedAt = DateTimeOffset.UtcNow,
                        Name = request.FavoriteName,
                        UserId = userId
                    };
                    favorite = await _favorites.AddAsync(favorite, cancellationToken);
                    placeFavorite = await _placeFavorites.AddAsync(new PlaceFavoriteEntity
                    {
                        FavoriteList = favorite,
                        PlaceId = request.PlaceId
                    }, cancellationToken);
                }
                else
                {
                    return null;
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return placeFavorite.FavoriteList?.Id ?? placeFavorite.FavoriteListId;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return null;
            }
        }
    }
}
