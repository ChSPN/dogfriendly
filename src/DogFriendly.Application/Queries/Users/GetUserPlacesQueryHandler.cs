using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Users;
using DogFriendly.Domain.ViewModels.Places;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Users
{
    /// <summary>
    /// Handler for <see cref="GetUserPlacesQuery"/>.
    /// </summary>
    /// <seealso cref="IRequestHandler&lt;FavoriteListViewQuery, List&lt;PlaceListViewModel&gt;&gt;" />
    public class GetUserPlacesQueryHandler : IRequestHandler<GetUserPlacesQuery, List<PlaceListViewModel>>
    {
        private readonly IRepository<FavoriteListEntity> _favorites;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserPlacesQueryHandler"/> class.
        /// </summary>
        /// <param name="favorites">The favorites.</param>
        public GetUserPlacesQueryHandler(IRepository<FavoriteListEntity> favorites)
        {
            _favorites = favorites;
        }

        /// <inheritdoc />
        public async Task<List<PlaceListViewModel>> Handle(GetUserPlacesQuery request, CancellationToken cancellationToken)
        {
            var query = _favorites
                .SingleResultQuery()
                .AndFilter(f => f.User.Email == request.Email);
            return await _favorites
                .ToQueryable(query)
                .SelectMany(f => f.PlaceFavorites.Where(p => p.FavoriteListId == request.FavoriteId))
                .Select(p => new PlaceListViewModel
                {
                    Id = p.Place.Id,
                    Name = p.Place.Name,
                    Description = p.Place.Description.Substring(0, p.Place.Description.LastIndexOf(' ', 100)) + " ...",
                    Photos = p.Place.Photos,
                    Latitude = p.Place.Latitude,
                    Longitude = p.Place.Longitude,
                    PlaceTypeId = p.Place.PlaceTypeId,
                    Rating = p.Place.Reviews.Any()
                        ? p.Place.Reviews.Sum(r => r.Rating) / p.Place.Reviews.Count()
                        : 0
                })
                .ToListAsync();
        }
    }
}
