using DogFriendly.Domain.Models;
using DogFriendly.Domain.ViewModels.Amenities;
using MediatR;

namespace DogFriendly.Domain.Command.Favorites
{
    /// <summary>
    /// Command for saving place.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Int32?&gt;" />
    public class PlaceSaveCommand : IRequest<int?>
    {
        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        /// <value>
        /// The place.
        /// </value>
        public required PlaceModel Place { get; set; }
    }
}
