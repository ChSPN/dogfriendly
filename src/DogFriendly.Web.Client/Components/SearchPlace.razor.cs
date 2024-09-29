using DogFriendly.Domain.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Persilsoft.Leaflet.Blazor;
using Persilsoft.Nominatim.Geolocation.Blazor;

namespace DogFriendly.Web.Client.Components
{
    /// <summary>
    /// Search place component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    /// <seealso cref="System.IDisposable" />
    public partial class SearchPlace : ComponentBase, IDisposable
    {
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
        protected Map Map { get; set; }

        /// <summary>
        /// Gets or sets the nominatim.
        /// </summary>
        /// <value>
        /// The nominatim.
        /// </value>
        [Inject]
        protected INominatimResource Nominatim { get; set; }

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

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                DotNetObjectReference<SearchPlace> objRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("addMapEventListener", objRef);
                var position = await Geolocation.GetPosition();
                await Map.SetView(new LeafletLatLong(position.Latitude, position.Longitude), 15);
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
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
        /// Selects the suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion.</param>
        protected async Task SelectSuggestion(NominatimResponse suggestion)
        {
            SearchQuery = suggestion.DisplayName;
            Suggestions.Clear();
            var lat = double.Parse(suggestion.Latitude, System.Globalization.CultureInfo.InvariantCulture);
            var lon = double.Parse(suggestion.Longitude, System.Globalization.CultureInfo.InvariantCulture);
            await Map.SetView(new LeafletLatLong(lat, lon), 15);
            StateHasChanged();
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
                    await Map.SetView(new LeafletLatLong(lat, lon), 15);
                    StateHasChanged();
                }
            }
            else
            {
                Suggestions.Clear();
                StateHasChanged();
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
    }
}
