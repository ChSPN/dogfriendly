using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using MediatR;

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
        public PlaceModel(IMediator mediator)
        {
            _mediator = mediator;
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
        /// <returns></returns>
        public static async Task<PlaceViewModel> LoadViewModel(IMediator mediator,
            int placeId)
            => await mediator.Send(new PlaceViewQuery { PlaceId = placeId });

        /// <summary>
        /// Searches the specified places.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static async Task<List<PlaceListViewModel>> Search(IMediator mediator,
            PlaceListViewQuery request)
            => await mediator.Send(request);
    }
}
