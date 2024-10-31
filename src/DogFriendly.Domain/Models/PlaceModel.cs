using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.Queries.Reviews;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;
using MediatR;
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
        public List<string> Photos { get; set; } = [];

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
    }
}
