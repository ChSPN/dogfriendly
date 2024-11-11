using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.Queries.Reviews;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;
using MediatR;
using Npoi.Mapper;
using System.Collections.Immutable;

namespace DogFriendly.Domain.Models
{
    /// <summary>
    /// Model for place.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Models.ModelBase" />
    public class PlaceModel : ModelBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceModel"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="placeId">The place identifier.</param>
        public PlaceModel(IMediator mediator, int placeId = 0)
        {
            _mediator = mediator;
            Id = placeId;
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the kilometers.
        /// </summary>
        /// <value>
        /// The kilometers.
        /// </value>
        public double Kilometers { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the opening hours.
        /// </summary>
        /// <value>
        /// The opening hours.
        /// </value>
        public string? OpeningHours { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the photos.
        /// </summary>
        /// <value>
        /// The photos.
        /// </value>
        public List<string>? Photos { get; set; }

        /// <summary>
        /// Gets or sets the place type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
        public int PlaceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the reviews.
        /// </summary>
        /// <value>
        /// The reviews.
        /// </value>
        public ImmutableList<ReviewModel> Reviews { get; private set; } = [];

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string? Website { get; set; }

        /// <summary>
        /// Imports the specified Excel.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="file">The file.</param>
        /// <param name="userEmail">The user email.</param>
        public static async Task ImportExcel(IMediator mediator, Stream file, string? userEmail = null)
        {
            var types = await mediator.Send(new PlaceTypeViewAllQuery());
            var placeTypes = types.ToDictionary(t => t.Name, t => t.Id);
            var mapper = new Mapper(file);
            mapper.HasHeader = true;
            mapper
                .Map<PlaceModel>(0, o => o.Id)
                .Map<PlaceModel>(1, o => o.Name)
                .Map<PlaceModel>(2, o => o.Address)
                .Map<PlaceModel>(3, o => o.PostalCode)
                .Map<PlaceModel>(4, o => o.City)
                .Map<PlaceModel>(5, o => o.Country)
                .Map<PlaceModel>(6, o => o.Phone)
                .Map<PlaceModel>(7, o => o.Website)
                .Map<PlaceModel>(8, o => o.Description);
            var exceptions = new List<Exception>();
            for (int i = 0; i < mapper.Workbook.NumberOfSheets; i++)
            {
                string sheetName = mapper.Workbook.GetSheetName(i);
                var placeTypeId = placeTypes[sheetName];
                var places = mapper.Take<PlaceModel>(sheetName, objectInitializer: () => new PlaceModel(mediator));
                foreach (var place in places)
                {
                    if (place.ErrorMessage != null)
                    {
                        var exception = new InvalidOperationException(place.ErrorMessage);
                        exceptions.Add(exception);
                    }
                    else
                    {
                        try
                        {
                            place.Value.PlaceTypeId = placeTypeId;
                            await place.Value.Save(userEmail);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(ex);
                        }
                        finally
                        {
                            await Task.Delay(1000);
                        }
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Imports the photos.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="photos">The photos.</param>
        /// <param name="userEmail">The user email.</param>
        public static async Task ImportPhotos(IMediator mediator, Dictionary<string, Stream> photos, string? userEmail = null)
        {
            var dicos = photos
                .Select(p =>
                {
                    if (!p.Key.Contains('_'))
                        return null;
                    var file = p.Key.Split('_');
                    if (int.TryParse(file.First(), out int placeId))
                        return new
                        {
                            FileName = p.Key,
                            FileStream = p.Value,
                            PlaceId = placeId,
                        };
                    else
                        return null;
                })
                .Where(p => p != null)
                .GroupBy(p => p.PlaceId)
                .ToDictionary(
                    p => p.Key,
                    p => p.ToList());
            var exceptions = new List<Exception>();
            foreach (var dic in dicos)
            {
                try
                {
                    var place = new PlaceModel(mediator, dic.Key);
                    await place.AddPhotos(
                        dic.Value.ToDictionary(p => p.FileName, p => p.FileStream), 
                        userEmail);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Loads the view model.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="userEmail">The user email.</param>
        /// <returns></returns>
        public static async Task<PlaceViewModel> LoadViewModel(IMediator mediator,
            int placeId, string? userEmail = null)
            => await mediator.Send(new PlaceViewQuery
            {
                PlaceId = placeId,
                UserEmail = userEmail
            });

        /// <summary>
        /// Searches the specified places.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<List<PlaceListViewModel>> Search(IMediator mediator,
        PlaceListViewQuery request)
        => await mediator.Send(request);

        /// <summary>
        /// Adds the favorite.
        /// </summary>
        /// <param name="favoriteId">The favorite identifier.</param>
        /// <param name="userEmail">The user email.</param>
        public async Task<int?> AddFavorite(int favoriteId, string userEmail)
            => await _mediator.Send(new AddPlaceFavoriteCommand
            {
                FavoriteId = favoriteId,
                PlaceId = Id,
                UserEmail = userEmail
            });

        /// <summary>
        /// Adds the favorite.
        /// </summary>
        /// <param name="favoriteName">Name of the favorite.</param>
        /// <param name="userEmail">The user email.</param>
        public async Task<int?> AddFavorite(string favoriteName, string userEmail)
            => await _mediator.Send(new AddPlaceFavoriteCommand
            {
                FavoriteName = favoriteName,
                PlaceId = Id,
                UserEmail = userEmail
            });

        /// <summary>
        /// Adds the photos.
        /// </summary>
        /// <param name="photos">The photos.</param>
        /// <param name="userEmail">The user email.</param>
        public async Task AddPhotos(Dictionary<string, Stream> photos,
            string? userEmail = null)
        {
            var result = await _mediator.Send(new PlacePhotosCommand
            {
                PlaceId = Id,
                Photos = photos,
                UserEmail = userEmail ?? nameof(AddPhotos)
            });
            if (result != null)
            {
                if (Photos == null)
                    Photos = [];
                Photos.AddRange(photos.Keys);
            }
        }

        /// <summary>
        /// Add the review.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task AddReview(AddPlaceReviewCommand request)
        {
            if (Reviews?.Any(r => r.User?.Email == request.UserEmail) == true)
                throw new InvalidOperationException("User already reviewed this place.");

            request.PlaceId = Id;
            var review = await _mediator.Send(request);

            if (Reviews == null)
                Reviews = new List<ReviewModel>().ToImmutableList();

            Reviews = Reviews.Insert(0, new ReviewModel(_mediator, review.Id, review.CreatedAt)
            {
                Comment = review.Comment,
                PlaceId = review.PlaceId,
                Rating = review.Rating,
                UserId = review.UserId,
                User = new UserModel(_mediator,
                    review.User?.Email,
                    review.User?.Name,
                    review.UserId)
                {
                    PictureUri = review.User?.PhotoUri
                }
            });
        }

        /// <summary>
        /// Gets the place reviews.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlaceReviewViewModel>> GetPlaceReviews()
        {
            if (!Reviews.Any())
            {
                await LoadReviews();
            }

            return Reviews
                .Select(r => new PlaceReviewViewModel
                {
                    Id = r.Id,
                    PlaceId = r.PlaceId,
                    CreatedAt = r.CreatedAt,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    UserPictureUri = r.User?.PictureUri,
                    UserName = r.User?.Name
                })
                .ToList();
        }

        /// <summary>
        /// Loads the reviews.
        /// </summary>
        public async Task LoadReviews()
        {
            var reviews = await _mediator.Send(new GetPlaceReviewQuery { PlaceId = Id });
            Reviews = reviews
                .Select(r => new ReviewModel(_mediator, r.Id, r.CreatedAt)
                {
                    Comment = r.Comment,
                    PlaceId = r.PlaceId,
                    Rating = r.Rating,
                    UserId = r.UserId,
                    User = new UserModel(_mediator,
                        r.User?.Email,
                        r.User?.Name,
                        r.UserId)
                    {
                        PictureUri = r.User?.PhotoUri
                    }
                })
                .ToImmutableList();
        }

        /// <summary>
        /// Removes the favorite.
        /// </summary>
        /// <param name="favoriteId">The favorite identifier.</param>
        /// <param name="userEmail">The user email.</param>
        /// <returns></returns>
        public async Task<bool> RemoveFavorite(int favoriteId, string userEmail)
            => await _mediator.Send(new RemovePlaceFavoriteCommand
            {
                FavoriteId = favoriteId,
                PlaceId = Id,
                UserEmail = userEmail
            });

        /// <summary>
        /// Removes the review.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <param name="userEmail">The user email.</param>
        /// <returns></returns>
        public async Task<bool> RemoveReview(int reviewId, string userEmail)
        {
            var result = await _mediator.Send(new RemovePlaceReviewCommand
            {
                PlaceId = Id,
                ReviewId = reviewId,
                UserEmail = userEmail
            });
            if (Reviews != null && result)
                Reviews.RemoveAll(r => r.Id == reviewId);
            return result;
        }

        /// <summary>
        /// Saves this place.
        /// </summary>
        /// <param name="userEmail">User email.</param>
        public async Task Save(string? userEmail = null)
        {
            if (Id == 0)
            {
                CreatedAt = DateTimeOffset.UtcNow;
                CreatedBy = userEmail ?? nameof(Save);
            }
            else
            {
                UpdatedAt = DateTimeOffset.UtcNow;
                UpdatedBy = userEmail ?? nameof(Save);
            }

            var place = await _mediator.Send(new PlaceSaveCommand
            {
                Place = this
            });

            Id = place ?? Id;
        }
    }
}
