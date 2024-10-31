using DogFriendly.Domain.ViewModels.Places;
using MediatR;

namespace DogFriendly.Domain.Queries.Places
{
    /// <summary>
    /// Query for getting list of places.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Places.PlaceListViewModel&gt;&gt;" />
    public class PlaceListViewQuery : IRequest<List<PlaceListViewModel>>
    {
        /// <summary>
        /// Gets or sets the amenities.
        /// </summary>
        /// <value>
        /// The amenities.
        /// </value>
        public List<int>? Amenities { get; set; }

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
        /// Gets or sets the place type identifier.
        /// </summary>
        /// <value>
        /// The place type identifier.
        /// </value>
        public int PlaceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public int? Rating { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        public string? Search { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string? UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the zoom level.
        /// </summary>
        /// <value>
        /// The zoom level.
        /// </value>
        public int ZoomLevel { get; set; }
    }
}
