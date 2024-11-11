using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Resources;
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
    public class PlaceSaveCommandHandler : IRequestHandler<PlaceSaveCommand, int?>
    {
        private readonly IRepository<PlaceEntity> _places;
        private readonly IRepository<AmenityEntity> _amenities;
        private readonly IRepository<PlaceAmenityEntity> _placeAmenities;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INominatimResource _nominatimResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceSaveCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="places">The places.</param>
        /// <param name="nominatimResource">The nominatim resource.</param>
        /// <param name="amenities">The amenities.</param>
        /// <param name="placeAmenities">The place amenities.</param>
        public PlaceSaveCommandHandler(IUnitOfWork unitOfWork,
            IRepository<PlaceEntity> places,
            INominatimResource nominatimResource,
            IRepository<AmenityEntity> amenities,
            IRepository<PlaceAmenityEntity> placeAmenities)
        {
            _unitOfWork = unitOfWork;
            _places = places;
            _nominatimResource = nominatimResource;
            _amenities = amenities;
            _placeAmenities = placeAmenities;
        }

        /// <inheritdoc />
        public async Task<int?> Handle(PlaceSaveCommand request, CancellationToken cancellationToken)
        {
            PlaceEntity? place = null;
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                place = await _places
                    .ToQueryable(_places
                        .SingleResultQuery()
                        .AndFilter(p => p.Id == request.Place.Id))
                    .Include(p => p.PlaceAmenities)
                    .FirstOrDefaultAsync();
                bool isReNew = false;
                if (place == null)
                {
                    place = new PlaceEntity
                    {
                        Id = request.Place.Id,
                        Name = request.Place.Name,
                        Description = request.Place.Description,
                        Address = request.Place.Address ?? string.Empty,
                        City = request.Place.City ?? string.Empty,
                        PostalCode = request.Place.PostalCode ?? string.Empty,
                        Country = request.Place.Country ?? string.Empty,
                        Phone = request.Place.Phone,
                        Website = request.Place.Website,
                        Photos = request.Place.Photos ?? [],
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = request.Place.CreatedBy
                    };
                    await _places.AddAsync(place);
                    isReNew = true;
                }
                else
                {
                    var newAddress = $"{request.Place.Address}, {request.Place.PostalCode} {request.Place.City}, {request.Place.Country}";
                    var oldAddress = $"{place.Address}, {place.PostalCode} {place.City}, {place.Country}";
                    isReNew = oldAddress != newAddress;
                    place.Name = request.Place.Name;
                    place.Description = request.Place.Description;
                    place.Address = request.Place.Address ?? string.Empty;
                    place.City = request.Place.City ?? string.Empty;
                    place.PostalCode = request.Place.PostalCode ?? string.Empty;
                    place.Country = request.Place.Country ?? string.Empty;
                    place.Phone = request.Place.Phone;
                    place.Website = request.Place.Website;
                    place.UpdatedAt = DateTime.UtcNow;
                    place.UpdatedBy = request.Place.CreatedBy;
                    _places.Update(place);
                }

                if (isReNew)
                {
                    var address = $"{place.Address}, {place.PostalCode} {place.City}, {place.Country}";
                    var locations = await _nominatimResource.Search(address, 1, place.Country.Substring(0, 2).ToLower());
                    var location = locations.FirstOrDefault();
                    if (location != null)
                    {
                        place.Latitude = double.Parse(location.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                        place.Longitude = double.Parse(location.Longitude, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                place.PlaceAmenities ??= [];
                place.PlaceTypeId = request.Place.PlaceTypeId;
                if (request.Place.Photos != null)
                {
                    place.Photos = request.Place.Photos;
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
            }

            if (place == null)
            {
                return null;
            }
            else if (place.PlaceAmenities.Count >= 5)
            {
                return place.Id;
            }

            var amenityMin = await _amenities.MinAsync(a => a.Id);
            var amenityMax = await _amenities.MaxAsync(a => a.Id);
            var random = new Random();
            int amenityId = 0;
            do
            {
                amenityId = random.Next(amenityMin, amenityMax);
                if (!place.PlaceAmenities.Any(a => a.AmenityId == amenityId))
                {
                    var amenity = new PlaceAmenityEntity
                    {
                        AmenityId = amenityId,
                        PlaceId = request.Place.Id,
                    };
                    place.PlaceAmenities.Add(amenity);
                    await _placeAmenities.AddAsync(amenity);
                }
            }
            while (place.PlaceAmenities.Count < 5);

            await _unitOfWork.SaveChangesAsync();
            return place.Id;
        }
    }
}
