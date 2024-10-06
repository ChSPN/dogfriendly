using DogFriendly.Domain.ViewModels.Places;

namespace DogFriendly.Web.Client.Services
{
    /// <summary>
    /// Service for search.
    /// </summary>
    public class SearchService
    {
        /// <summary>
        /// The on places changed
        /// </summary>
        public EventHandler<List<PlaceListViewModel>> OnPlacesChanged;

        /// <summary>
        /// The on search changed.
        /// </summary>
        public EventHandler OnSearchChanged;

        /// <summary>
        /// The on view changed.
        /// </summary>
        public EventHandler OnViewChanged;

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the zoom level.
        /// </summary>
        /// <value>
        /// The zoom level.
        /// </value>
        public int ZoomLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the places.
        /// </summary>
        /// <param name="places">The places.</param>
        public void SetPlaces(List<PlaceListViewModel> places)
        {
            OnPlacesChanged?.Invoke(this, places);
        }

        /// <summary>
        /// Sets the search.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        public void SetSearch(double latitude, double longitude, int? zoomLevel = null)
        {
            Latitude = latitude;
            Longitude = longitude;

            if (zoomLevel.HasValue)
            {
                ZoomLevel = zoomLevel.Value;
            }

            OnSearchChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the view.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        public void SetView(double latitude, double longitude, int? zoomLevel = null)
        {
            Latitude = latitude;
            Longitude = longitude;

            if (zoomLevel.HasValue)
            {
                ZoomLevel = zoomLevel.Value;
            }

            OnViewChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
