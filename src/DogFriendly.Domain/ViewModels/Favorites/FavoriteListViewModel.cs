namespace DogFriendly.Domain.ViewModels.Favorites
{
    /// <summary>
    /// Place favorite list view model.
    /// </summary>
    public class FavoriteListViewModel : PlaceFavoriteViewModel
    {
        /// <summary>
        /// Gets or sets the photo URI.
        /// </summary>
        /// <value>
        /// The photo URI.
        /// </value>
        public string? PhotoUri { get; set; }

        /// <summary>
        /// Gets or sets the place count.
        /// </summary>
        /// <value>
        /// The place count.
        /// </value>
        public int PlaceCount { get; set; }
    }
}
