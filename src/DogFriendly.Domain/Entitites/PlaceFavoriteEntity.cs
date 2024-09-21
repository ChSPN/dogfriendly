namespace DogFriendly.Domain.Entitites
{
    /// <summary>
    /// Place favorite entity.
    /// </summary>
    public class PlaceFavoriteEntity
    {
        /// <summary>
        /// Gets or sets the favorite list.
        /// </summary>
        /// <value>
        /// The favorite list.
        /// </value>
        public FavoriteListEntity? FavoriteList { get; set; }

        /// <summary>
        /// Gets or sets the favorite list identifier.
        /// </summary>
        /// <value>
        /// The favorite list identifier.
        /// </value>
        public int FavoriteListId { get; set; }

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
