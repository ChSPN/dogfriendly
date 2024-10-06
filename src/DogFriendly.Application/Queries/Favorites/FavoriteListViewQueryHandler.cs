using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Favorites;
using DogFriendly.Domain.ViewModels.Favorites;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Favorites
{
    /// <summary>
    /// Handler for <see cref="FavoriteListViewQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Favorites.FavoriteListViewQuery, System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Favorites.FavoriteListViewModel&gt;&gt;" />
    public class FavoriteListViewQueryHandler : IRequestHandler<FavoriteListViewQuery, List<FavoriteListViewModel>>
    {
        private readonly IRepository<FavoriteListEntity> _favorites;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteListViewQueryHandler"/> class.
        /// </summary>
        /// <param name="favorites">The favorites.</param>
        public FavoriteListViewQueryHandler(IRepository<FavoriteListEntity> favorites)
        {
            _favorites = favorites;
        }

        /// <inheritdoc />
        public async Task<List<FavoriteListViewModel>> Handle(FavoriteListViewQuery request, CancellationToken cancellationToken)
        {
            var query = _favorites
                .SingleResultQuery()
                .AndFilter(f => f.User.Email == request.UserEmail)
                .OrderBy(f => f.Name);
            return await _favorites
                .ToQueryable(query)
                .Select(f => new FavoriteListViewModel
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
