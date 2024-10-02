using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Web.Client.Services;
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
        private EventHandler<List<PlaceListViewModel>> OnPlacesChanged;
        private EventHandler OnViewChanged;

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        [Inject]
        protected IConfiguration Configuration { get; set; }

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
        protected string? SearchQuery { get; set; }

        /// <summary>
        /// Gets or sets the search service.
        /// </summary>
        [Inject]
        protected SearchService SearchService { get; set; }

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
            SearchService.OnPlacesChanged -= OnPlacesChanged;
            SearchService.OnViewChanged -= OnViewChanged;
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
            SearchService.SetSearch(adjusted.Latitude, adjusted.Longitude, 13);
            this.Map.Geometric.Points.AppearanceOnType((item) => item.type == "place").pattern = new PointIcon
            {
                iconUrl = "/img/point.png",
                iconSize = new int[] { 30, 50 },
            };
            this.Map.Geometric.Points.Appearance(item => item.type == "place").pattern = new PointTooltip
            {
                permanent = false,
                content = @"<div class='result-list'>
                    <div class='media mb-3'>
                        <div class='result-image bg-secondary mr-3' style='width: 60px; height: 60px;'>
                            <img href='" + Configuration["PhotoUrl"] + @"width=60,height=60.,quality=75,fit=cover${value.photo}' class='img-fluid' alt='${value.name}' />
                        </div>
                        <div class='media-body'>
                            <h5 class='mt-0'>${value.name}</h5>
                            <p class='text-muted'>${value.description}</p>
                        </div>
                    </div>
                </div>"
            };
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
            OnPlacesChanged = async (sender, e) => await PlacesChanged(e);
            SearchService.OnPlacesChanged += OnPlacesChanged;
            OnViewChanged = (sender, e) => ViewChanged();
            SearchService.OnViewChanged += OnViewChanged;
            PlaceTypes = await PlaceTypeResource.GetViewAll();
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Called when mouse up map.
        /// </summary>
        /// <param name="args">The <see cref="ClicksMapArgs"/> instance containing the event data.</param>
        protected void OnMouseUpMap(ClicksMapArgs args)
        {
            SearchService.Latitude = args.location.latitude;
            SearchService.Longitude = args.location.longitude;
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
        protected void OnZoomLevelEndChange(MapZoomEventArgs args)
        {
            SearchService.ZoomLevel = args.zoomLevel;
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
            SearchService.SetSearch(adjusted.Latitude, adjusted.Longitude, 13);
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
        /// Places the changed.
        /// </summary>
        /// <param name="places">The places.</param>
        private async Task PlacesChanged(List<PlaceListViewModel> places)
        {
            await this.Map.Geometric.Points.delete();
            await this.Map.Geometric.Points.upload(places
                .Select(p => new StreamPoint
                {
                    guid = Guid.NewGuid(),
                    latitude = p.Latitude,
                    longitude = p.Longitude,
                    type = "place",
                    value = p
                })
                .ToList());
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
                    SearchService.SetSearch(adjusted.Latitude, adjusted.Longitude, 13);
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
        /// Views the changed.
        /// </summary>
        private async void ViewChanged()
        {
            this.Map.View.setZoomLevel = SearchService.ZoomLevel;
            await Task.Delay(200);
            this.Map.View.setCenter = new Location
            {
                latitude = SearchService.Latitude,
                longitude = SearchService.Longitude
            };
        }
    }
}
