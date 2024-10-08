using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Users;
using DogFriendly.Domain.ViewModels.Users;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Users
{
    /// <summary>
    /// Handler for <see cref="GetUserFavoritesQuery"/>.
    /// </summary>
    /// <seealso cref="IRequestHandler&lt;FavoriteListViewQuery, List&lt;UserFavoriteViewModel&gt;&gt;" />
    public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, List<UserFavoriteViewModel>>
    {
        private readonly IRepository<FavoriteListEntity> _favorites;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserFavoritesQueryHandler"/> class.
        /// </summary>
        /// <param name="favorites">The favorites.</param>
        public GetUserFavoritesQueryHandler(IRepository<FavoriteListEntity> favorites)
        {
            _favorites = favorites;
        }

        /// <inheritdoc />
        public async Task<List<UserFavoriteViewModel>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            var query = _favorites
                .SingleResultQuery()
                .AndFilter(f => f.User.Email == request.Email)
                .OrderBy(f => f.Name);
            return await _favorites
                .ToQueryable(query)
                .Select(f => new UserFavoriteViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    PhotoUri = f.PlaceFavorites
                        .OrderBy(p => p.Place.Name)
                        .Select(p => p.Place.Photos.FirstOrDefault())
                        .FirstOrDefault(),
                    PlaceCount = f.PlaceFavorites.Count()
                })
                .ToListAsync();
        }
    }
}
