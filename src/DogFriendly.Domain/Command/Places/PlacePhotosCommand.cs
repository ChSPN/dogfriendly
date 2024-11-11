using MediatR;

namespace DogFriendly.Domain.Command.Favorites
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;System.String&gt;&gt;" />
    public class PlacePhotosCommand : IRequest<List<string>?>
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
        public required string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the photos.
        /// </summary>
        /// <value>
        /// The photos.
        /// </value>
        public required Dictionary<string, Stream> Photos { get; set; }
    }
}
