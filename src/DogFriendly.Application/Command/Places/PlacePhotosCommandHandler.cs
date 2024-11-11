using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DogFriendly.Application.Command.Favorites
{
    /// <summary>
    /// Handler for the <see cref="PlacePhotosCommand"/> command.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Favorites.PlacePhotosCommand, System.Collections.Generic.List&lt;System.String&gt;&gt;" />
    public class PlacePhotosCommandHandler : IRequestHandler<PlacePhotosCommand, List<string>?>
    {
        private readonly IRepository<PlaceEntity> _places;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly FileStorageOption _fileStorageOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlacePhotosCommandHandler"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="places">The places.</param>
        /// <param name="options">The options.</param>
        /// <param name="fileStorageRepository">The file storage repository.</param>
        public PlacePhotosCommandHandler(IUnitOfWork unitOfWork,
            IRepository<PlaceEntity> places,
            IOptions<FileStorageOption> options,
            IFileStorageRepository fileStorageRepository)
        {
            _unitOfWork = unitOfWork;
            _places = places;
            _fileStorageRepository = fileStorageRepository;
            _fileStorageOptions = options.Value;
        }

        /// <inheritdoc />
        public async Task<List<string>?> Handle(PlacePhotosCommand request, CancellationToken cancellationToken)
        {
            PlaceEntity? place = null;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                place = await _places
                    .ToQueryable(_places
                        .SingleResultQuery()
                        .AndFilter(p => p.Id == request.PlaceId))
                    .FirstOrDefaultAsync();
                if (place == null)
                {
                    return null;
                }
                else
                {
                    if (place.Photos == null)
                        place.Photos = [];

                    foreach (var file in request.Photos)
                    {
                        if (place.Photos.Any(p => p.Contains(file.Key)))
                            continue;

                        using (var reader = new MemoryStream())
                        {
                            file.Value.CopyTo(reader);
                            var result = await _fileStorageRepository.UploadFileAsync($"{_fileStorageOptions.PlacesUri}/{file.Key}", reader.ToArray());
                            if (result)
                                place.Photos.Add($"{_fileStorageOptions.DomainUri}/{_fileStorageOptions.PlacesUri}/{file.Key}");
                        }
                    }

                    place.UpdatedAt = DateTimeOffset.UtcNow;
                    place.UpdatedBy = request.UserEmail;
                    _places.Update(place);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
            }

            return place?.Photos;
        }
    }
}
