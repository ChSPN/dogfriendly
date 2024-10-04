﻿using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Search result component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class SearchResult : ComponentBase, IHandleEvent, IDisposable
    {
        private Timer? _searchTimer;
        private EventHandler OnSearchChanged;

        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>
        /// The place identifier.
        /// </value>
        [Parameter]
        public int? PlaceId { get; set; }

        /// <summary>
        /// Gets or sets the place type identifier.
        /// </summary>
        /// <value>
        /// The place type identifier.
        /// </value>
        [Parameter]
        public int? PlaceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        [Inject]
        protected IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the js runtime.
        /// </summary>
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        protected PlaceViewModel Place { get; set; }

        /// <summary>
        /// Gets or sets the place resource.
        /// </summary>
        /// <value>
        /// The place resource.
        /// </value>
        [Inject]
        protected IPlaceResource PlaceResource { get; set; }

        /// <summary>
        /// Gets or sets the places.
        /// </summary>
        /// <value>
        /// The places.
        /// </value>
        protected List<PlaceListViewModel> Places { get; set; } = new List<PlaceListViewModel>();

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
        /// <inheritdoc />
        public void Dispose()
        {
            SearchService.OnSearchChanged -= OnSearchChanged;
        }

        /// <summary>
        /// Close place.
        /// </summary>
        protected void ClosePlace()
        {
            PlaceId = null;
            NavigationManager.NavigateTo($"/search/{Place.PlaceTypeId}");
        }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            OnSearchChanged = async (sender, e) => await SearchChanged();
            SearchService.OnSearchChanged += OnSearchChanged;
            await LoadDatas();
        }
        /// <inheritdoc />
        protected override async Task OnParametersSetAsync()
        {
            await LoadDatas();
            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// Raises the <see cref="E:KeyPress" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        protected async Task OnSearchKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" || e.Key == "Escape")
            {
                await SearchChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:InputChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangeEventArgs"/> instance containing the event data.</param>
        protected void OnSearchText(ChangeEventArgs e)
        {
            SearchQuery = e?.Value?.ToString();
            if (_searchTimer != null)
            {
                _searchTimer.Dispose();
            }

            _searchTimer = new Timer(async _ => await SearchChanged(), null, 500, Timeout.Infinite);
        }

        /// <summary>
        /// Searches the changed.
        /// </summary>
        protected async Task SearchChanged()
        {
            if (PlaceTypeId.HasValue)
            {
                Places = await PlaceResource.Search(new PlaceListViewQuery
                {
                    Latitude = SearchService.Latitude,
                    Longitude = SearchService.Longitude,
                    ZoomLevel = SearchService.ZoomLevel,
                    PlaceTypeId = PlaceTypeId.Value,
                    Search = SearchQuery
                });
                SearchService.SetPlaces(Places);
                StateHasChanged();
            }
        }

        /// <summary>
        /// Views the result.
        /// </summary>
        /// <param name="place">The place.</param>
        protected void ViewResult(PlaceListViewModel place)
        {
            // todo: fix this position for the map center.
            var latitude = place.Latitude/* - 0.01*/;
            var longitude = place.Longitude/* + 0.04*/;
            SearchService.SetView(latitude, longitude, 15);
        }

        /// <summary>
        /// Loads the datas.
        /// </summary>
        private async Task LoadDatas()
        {
            if (PlaceId.HasValue)
            {
                Place = await PlaceResource.GetViewModel(PlaceId.Value);
                PlaceTypeId = Place.PlaceTypeId;
            }

            if (!Places.Any() && PlaceTypeId.HasValue)
            {
                await SearchChanged();
            }
        }

        /// <summary>
        /// On place clicked.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private async Task OnPlaceClicked(PlaceListViewModel place)
        {
            PlaceId = place.Id;
            NavigationManager.NavigateTo($"/place/{PlaceId}");
        }
    }
}
