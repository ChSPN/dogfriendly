using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Places;
using LeafletForBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Persilsoft.Nominatim.Geolocation.Blazor;
using static LeafletForBlazor.RealTimeMap;

namespace DogFriendly.Web.Client.Components
{
    /// <summary>
    /// Search place component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    /// <seealso cref="System.IDisposable" />
    public partial class SearchPlace : ComponentBase, IDisposable
    {
        /// <summary>
        /// The parameters.
        /// </summary>
        protected LoadParameters Parameters = new LoadParameters
        {
            location = new Location
            {
                latitude = 48.8575,
                longitude = 2.3514,
            },
            zoomLevel = 13
        };

        private Timer? _searchTimer;

        /// <summary>
        /// Gets or sets the geolocation.
        /// </summary>
        /// <value>
        /// The geolocation.
        /// </value>
        [Inject]
        protected GeolocationService Geolocation { get; set; }

        /// <summary>
        /// Gets or sets the js.
        /// </summary>
        /// <value>
        /// The js.
        /// </value>
        [Inject]
        protected IJSRuntime JS { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        protected RealTimeMap Map { get; set; }

        /// <summary>
        /// Gets or sets the nominatim.
        /// </summary>
        /// <value>
        /// The nominatim.
        /// </value>
        [Inject]
        protected INominatimResource Nominatim { get; set; }

        /// <summary>
        /// Gets or sets the place type resource.
        /// </summary>
        /// <value>
        /// The place type resource.
        /// </value>
        [Inject]
        protected IPlaceTypeResource PlaceTypeResource { get; set; }

        /// <summary>
        /// Gets or sets the place types.
        /// </summary>
        /// <value>
        /// The place types.
        /// </value>
        protected List<PlaceTypeViewModel> PlaceTypes { get; set; } = new List<PlaceTypeViewModel>();

        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        protected string SearchQuery { get; set; }

        /// <summary>
        /// Gets or sets the suggestions.
        /// </summary>
        /// <value>
        /// The suggestions.
        /// </value>
        protected List<NominatimResponse> Suggestions { get; set; } = new List<NominatimResponse>();
        
        /// <inheritdoc />
        public void Dispose()
        {
            JS.InvokeVoidAsync("removeMapEventListener");
        }

        /// <summary>
        /// Hides the suggestions.
        /// </summary>
        [JSInvokable]
        public void HideSuggestions()
        {
            Suggestions?.Clear();
            StateHasChanged();
        }

        protected async Task OnAfterMapLoaded(MapEventArgs args)
        {
            var position = await Geolocation.GetPosition();
            var adjusted = AdjustCenter(position.Latitude, position.Longitude);
            this.Map.View.setCenter = new Location
            {
                latitude = adjusted.Latitude,
                longitude = adjusted.Longitude
            };
            this.Map.View.setZoomLevel = 13;
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                DotNetObjectReference<SearchPlace> objRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("addMapEventListener", objRef);
                await Geolocation.GetPosition();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            PlaceTypes = await PlaceTypeResource.GetViewAll();
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Called when mouse up map.
        /// </summary>
        /// <param name="args">The <see cref="ClicksMapArgs"/> instance containing the event data.</param>
        protected async Task OnMouseUpMap(ClicksMapArgs args)
        {
        }

        /// <summary>
        /// Raises the <see cref="E:InputChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangeEventArgs"/> instance containing the event data.</param>
        protected void OnSearchChanged(ChangeEventArgs e)
        {
            SearchQuery = e?.Value?.ToString();
            if (_searchTimer != null)
            {
                _searchTimer.Dispose();
            }

            _searchTimer = new Timer(async _ => await SearchSuggestions(), null, 500, Timeout.Infinite);
        }

        /// <summary>
        /// Raises the <see cref="E:KeyPress" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected async Task OnSearchKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" || e.Key == "Escape")
            {
                Suggestions?.Clear();
                await SearchAddress();
            }
        }

        /// <summary>
        /// Called when zoom level end change.
        /// </summary>
        /// <param name="args">The <see cref="MapZoomEventArgs"/> instance containing the event data.</param>
        protected async Task OnZoomLevelEndChange(MapZoomEventArgs args)
        {
            // 
        }

        /// <summary>
        /// Selects the suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion.</param>
        protected void SelectSuggestion(NominatimResponse suggestion)
        {
            SearchQuery = suggestion.DisplayName;
            Suggestions.Clear();
            var lat = double.Parse(suggestion.Latitude, System.Globalization.CultureInfo.InvariantCulture);
            var lon = double.Parse(suggestion.Longitude, System.Globalization.CultureInfo.InvariantCulture);
            var adjusted = AdjustCenter(lat, lon);
            this.Map.View.setCenter = new Location
            {
                latitude = adjusted.Latitude,
                longitude = adjusted.Longitude
            };
            this.Map.View.setZoomLevel = 13;
        }

        /// <summary>
        /// Adjusts the center to top 5km.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        private (double Latitude, double Longitude) AdjustCenter(double latitude, double longitude)
        {
            double newLatitude = latitude - 0.08;
            return (newLatitude, longitude);
        }

        /// <summary>
        /// Searches the address.
        /// </summary>
        private async Task SearchAddress()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var response = await Nominatim.Search(SearchQuery, 1, countries: "fr");
                if (response != null && response.Count > 0)
                {
                    var location = response.First();
                    var lat = double.Parse(location.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                    var lon = double.Parse(location.Longitude, System.Globalization.CultureInfo.InvariantCulture);
                    var adjusted = AdjustCenter(lat, lon); 
                    this.Map.View.setCenter = new Location
                    {
                        latitude = adjusted.Latitude,
                        longitude = adjusted.Longitude
                    };
                    this.Map.View.setZoomLevel = 13;
                }
            }
            else
            {
                Suggestions.Clear();
            }
        }

        /// <summary>
        /// Searches the suggestions.
        /// </summary>
        private async Task SearchSuggestions()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var response = await Nominatim.Search(SearchQuery, countries: "fr");
                if (response != null && response.Any())
                {
                    Suggestions = response;
                }
            }
            else
            {
                Suggestions.Clear();
            }

            StateHasChanged();
        }

        /// <summary>
        /// Zoom to km.
        /// </summary>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns>Boundingbox in kilometers.</returns>
        private double ZoomToKm(int zoomLevel)
        {
            switch (zoomLevel)
            {
                case 19: return 0.10;
                case 18: return 0.20;
                case 17: return 0.40;
                case 16: return 0.80;
                case 15: return 1.6;
                case 14: return 3.2;
                case 13: return 6.4;
                case 12: return 12.8;
                case 11: return 25.6;
                case 10: return 51.2;
                case 9: return 102.4;
                case 8: return 204.8;
                case 7: return 409.6;
                default: return 409.6;
            }
        }
    }
}
