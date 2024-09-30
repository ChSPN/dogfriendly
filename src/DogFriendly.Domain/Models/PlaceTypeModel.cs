using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using MediatR;

namespace DogFriendly.Domain.Models
{
    /// <summary>
    /// Model for place type.
    /// </summary>
    public class PlaceTypeModel : ModelBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceTypeModel"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public PlaceTypeModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string? Color { get; set; }

        /// <summary>
        /// Gets or sets the icon URI.
        /// </summary>
        /// <value>
        /// The icon URI.
        /// </value>
        public string IconUri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets all place type for view.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <returns></returns>
        public static async Task<List<PlaceTypeViewModel>> LoadViewModel(IMediator mediator)
            => await mediator.Send(new PlaceTypeViewAllQuery());
    }
}
