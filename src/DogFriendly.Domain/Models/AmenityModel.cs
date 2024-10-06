using DogFriendly.Domain.Queries.Amenities;
using DogFriendly.Domain.ViewModels.Amenities;
using MediatR;

namespace DogFriendly.Domain.Models
{
    /// <summary>
    /// Review model.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Models.ModelBase" />
    public class AmenityModel : ModelBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewModel"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="placeId">The place identifier.</param>
        public AmenityModel(IMediator mediator, 
            int placeId = 0)
        {
            _mediator = mediator;
            Id = placeId;
        }

        /// <summary>
        /// Gets or sets the icon URI.
        /// </summary>
        /// <value>
        /// The icon URI.
        /// </value>
        public string? IconUri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public required string Name { get; set; }

        /// <summary>
        /// Loads the view model.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <returns></returns>
        public static async Task<List<AmenityListViewModel>> LoadViewModel(IMediator mediator)
            => await mediator.Send(new AmenityViewAllQuery());
    }
}
