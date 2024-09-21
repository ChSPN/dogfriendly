namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Place news entity.
    /// </summary>
    public class PlaceNewsEntity
    {
        /// <summary>
        /// Gets or sets the news.
        /// </summary>
        /// <value>
        /// The news.
        /// </value>
        public NewsEntity? News { get; set; }

        /// <summary>
        /// Gets or sets the news identifier.
        /// </summary>
        /// <value>
        /// The news identifier.
        /// </value>
        public int NewsId { get; set; }

        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        /// <value>
        /// The place.
        /// </value>
        public PlaceEntity? Place { get; set; }

        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        public int PlaceId { get; set; }
    }
}
