using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Favorites;
using DogFriendly.Domain.ViewModels.Places;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DogFriendly.Application.Queries.Places
{
    /// <summary>
    /// Handler for <see cref="PlaceListViewQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Places.PlaceListViewQuery, System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Places.PlaceListViewModel&gt;&gt;" />
    public class PlaceListViewQueryHandler : IRequestHandler<PlaceListViewQuery, List<PlaceListViewModel>>
    {
        private const string GetKilometersPlaces = "SELECT * FROM GetKilometersPlaces(@Kilometers, @Latitude, @Longitude)";

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceListViewQueryHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PlaceListViewQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<List<PlaceListViewModel>> Handle(PlaceListViewQuery request, CancellationToken cancellationToken)
        {
            bool isGlobal;
            IQueryable<PlaceEntity> query;
            if (request.Latitude == 0 
                && request.Longitude == 0 
                && request.ZoomLevel == 0)
            {
                isGlobal = true;
                query = _unitOfWork
                    .DbContext
                    .Set<PlaceEntity>();
            }
            else
            {
                isGlobal = false;
                var km = ZoomToKm(request.ZoomLevel);
                query = _unitOfWork
                    .DbContext
                    .Set<PlaceEntity>()
                    .FromSqlRaw(GetKilometersPlaces,
                        new NpgsqlParameter("@Kilometers", km),
                        new NpgsqlParameter("@Latitude", request.Latitude),
                        new NpgsqlParameter("@Longitude", request.Longitude));
            }

            query = query.Where(p => p.PlaceTypeId == request.PlaceTypeId);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query
                    .Where(p => p.Name.ToLower().Contains(request.Search.ToLower()));
            }

            if (request.Rating.HasValue)
            {
                query = query
                    .Where(p =>
                        (p.Reviews.Sum(r => r.Rating) / p.Reviews.Count()) >= request.Rating.Value);
            }

            if (request.Amenities?.Any() == true)
            {
                query = query
                    .Where(p => p.PlaceAmenities
                        .Any(pa => request.Amenities.Contains(pa.AmenityId)));
            }

            var result = await query
                .Select(p => new PlaceListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Substring(0, p.Description.LastIndexOf(' ', 100)) + " ...",
                    Kilometers = p.Kilometers,
                    PlaceTypeId = p.PlaceTypeId,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Photos = p.Photos,
                    Rating = p.Reviews.Any()
                        ? p.Reviews.Sum(r => r.Rating) / p.Reviews.Count()
                        : 0,
                    Favorite = string.IsNullOrEmpty(request.UserEmail)
                        ? null
                        : p.PlaceFavorites
                            .Where(f => f.FavoriteList.User.Email == request.UserEmail)
                            .Select(f => new PlaceFavoriteViewModel
                            {
                                Id = f.FavoriteList.Id,
                                Name = f.FavoriteList.Name
                            })
                            .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);
            return isGlobal
                ? result.OrderByDescending(p => p.Id).ToList()
                : result.OrderBy(p => p.Kilometers).ToList();
        }

        /// <summary>
        /// Zoom to km.
        /// </summary>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns>Boundingbox in kilometers.</returns>
        private double ZoomToKm(int zoomLevel)
        {
            switch (zoomLevel)
            {
                case 19: return 1;
                case 18: return 2;
                case 17: return 4;
                case 16: return 8;
                case 15: return 16;
                case 14: return 32;
                case 13: return 64;
                case 12: return 128;
                case 11: return 256;
                case 10: return 512;
                default: return 512;
            }
        }
    }
}