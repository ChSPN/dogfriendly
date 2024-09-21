using DogFriendly.Domain.Enums;

namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// News entity.
    /// </summary>
    /// <seealso cref="DogFriendly.Domain.Entitites.EntityBase" />
    public class NewsEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public required string Content { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the event begin date.
        /// </summary>
        /// <value>
        /// The event begin date.
        /// </value>
        public DateTimeOffset? EventBeginDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the news.
        /// </summary>
        /// <value>
        /// The type of the news.
        /// </value>
        public required NewsType NewsType { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets the photo URI.
        /// </summary>
        /// <value>
        /// The photo URI.
        /// </value>
        public required string PhotoUri { get; set; }

        /// <summary>
        /// Gets or sets the place news.
        /// </summary>
        /// <value>
        /// The place news.
        /// </value>
        public ICollection<PlaceNewsEntity>? PlaceNews { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public required string Title { get; set; }
    }
}
