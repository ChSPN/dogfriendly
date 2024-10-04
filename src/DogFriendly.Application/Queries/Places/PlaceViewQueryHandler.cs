using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Places
{
    /// <summary>
    /// Handler for <see cref="PlaceViewQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Places.PlaceViewQuery, DogFriendly.Domain.ViewModels.Places.PlaceViewModel&gt;" />
    public class PlaceViewQueryHandler : IRequestHandler<PlaceViewQuery, PlaceViewModel>
    {
        private readonly IRepository<PlaceEntity> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceViewQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaceViewQueryHandler(IRepository<PlaceEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<PlaceViewModel> Handle(PlaceViewQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                .SingleResultQuery()
                .AndFilter(p => p.Id == request.PlaceId);
            return await _repository
                .ToQueryable(query)
                .Select(p => new PlaceViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Address = p.Address,
                    City = p.City,
                    Country = p.Country,
                    Email = p.Email,
                    Phone = p.Phone,
                    PostalCode = p.PostalCode,
                    Photos = p.Photos,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    PlaceTypeId = p.PlaceTypeId,
                    OpeningHours = p.OpeningHours,
                    Website = p.Website,
                    Amenities = p.PlaceAmenities.Any()
                        ? p.PlaceAmenities
                            .Select(pa => new KeyValuePair<string, string?>
                            (
                                pa.Amenity.Name,
                                pa.Amenity.IconUri
                            ))
                            .ToList()
                        : null,
                    ReviewsCount = p.Reviews.Any()
                        ? p.Reviews.Count()
                        : 0,
                    Rating = p.Reviews.Any()
                        ? p.Reviews.Sum(r => r.Rating) / p.Reviews.Count()
                        : 0
                })
                .FirstAsync();
        }
    }
}
