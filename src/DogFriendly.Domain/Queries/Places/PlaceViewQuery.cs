using DogFriendly.Domain.ViewModels.Places;
using MediatR;

namespace DogFriendly.Domain.Queries.Places
{
    /// <summary>
    /// Query for getting the place.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Places.PlaceListViewModel&gt;&gt;" />
    public class PlaceViewQuery : IRequest<PlaceViewModel>
    {
        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string? UserEmail { get; set; }
    }
}
