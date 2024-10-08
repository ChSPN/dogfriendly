using Blazorise;
using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Amenities;
using DogFriendly.Domain.ViewModels.Favorites;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;
using DogFriendly.Domain.ViewModels.Users;
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
        private Modal _modal;
        private EventHandler _onSearchChanged;
        private Timer? _searchTimer;

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
        /// Gets or sets the add review.
        /// </summary>
        /// <value>
        /// The add review.
        /// </value>
        protected AddPlaceReviewCommand AddReview { get; set; } = new AddPlaceReviewCommand();

        /// <summary>
        /// Gets or sets the amenities.
        /// </summary>
        /// <value>
        /// The amenities.
        /// </value>
        protected List<AmenityListViewModel> Amenities { get; set; } = new List<AmenityListViewModel>();

        /// <summary>
        /// Gets or sets the amenity resource.
        /// </summary>
        /// <value>
        /// The amenity resource.
        /// </value>
        [Inject]
        protected IAmenityResource AmenityResource { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        [Inject]
        protected IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the name of the favorite.
        /// </summary>
        /// <value>
        /// The name of the favorite.
        /// </value>
        protected string? FavoriteName { get; set; }

        /// <summary>
        /// Gets or sets the favorites.
        /// </summary>
        /// <value>
        /// The favorites.
        /// </value>
        protected List<UserFavoriteViewModel> Favorites { get; set; } = new List<UserFavoriteViewModel>();

        /// <summary>
        /// Gets or sets the js runtime.
        /// </summary>
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets the minimum amenities.
        /// </summary>
        /// <value>
        /// The minimum amenities.
        /// </value>
        protected List<int> MinAmenities { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the minimum rating.
        /// </summary>
        /// <value>
        /// The minimum rating.
        /// </value>
        protected int? MinRating { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the place.
        /// </summary>
        protected PlaceViewModel? Place { get; set; }

        /// <summary>
        /// Gets or sets the place favorite.
        /// </summary>
        /// <value>
        /// The place favorite.
        /// </value>
        protected PlaceListViewModel? PlaceFavorite { get; set; }

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
        /// Gets or sets the reviews.
        /// </summary>
        /// <value>
        /// The reviews.
        /// </value>
        protected List<PlaceReviewViewModel> Reviews { get; set; } = new List<PlaceReviewViewModel>();

        /// <summary>
        /// Gets or sets a value indicating whether review send in progress.
        /// </summary>
        /// <value>
        ///   <c>true</c> if review send in progress; otherwise, <c>false</c>.
        /// </value>
        protected bool ReviewSendInProgress { get; set; }

        /// <summary>
        /// Gets or sets the state of the review send.
        /// </summary>
        /// <value>
        /// The state of the review send.
        /// </value>
        protected bool? ReviewSendState { get; set; }

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
        /// Gets or sets the toast service.
        /// </summary>
        /// <value>
        /// The toast service.
        /// </value>
        [Inject]
        protected IToastService ToastService { get; set; }

        /// <summary>
        /// Gets or sets the user resource.
        /// </summary>
        /// <value>
        /// The user resource.
        /// </value>
        [Inject]
        protected IUserResource UserResource { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            SearchService.OnSearchChanged -= _onSearchChanged;
        }

        /// <summary>
        /// Called when change amenities.
        /// </summary>
        /// <param name="amenities">The amenities.</param>
        [JSInvokable]
        public async Task OnAmenitiesChange(List<int> amenities)
        {
            MinAmenities = amenities;
            await SearchChanged();
            StateHasChanged();
        }

        /// <summary>
        /// Called when change rating.
        /// </summary>
        /// <param name="rating">The rating.</param>
        [JSInvokable]
        public async Task OnRatingChange(int? rating)
        {
            MinRating = rating;
            await SearchChanged();
            StateHasChanged();
        }

        /// <summary>
        /// Close place.
        /// </summary>
        protected void ClosePlace()
        {
            PlaceId = null;

            if (Place != null)
            {
                NavigationManager.NavigateTo($"/search/{Place.PlaceTypeId}");
            }

            Place = null;
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Amenities = await AmenityResource.GetViewAll();
                DotNetObjectReference<SearchResult> objRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("searchResultInit", objRef);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Raises the <see cref="ChangeEventArgs" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ChangeEventArgs"/> instance containing the event data.</param>
        protected void OnAmenitiesChanged(ChangeEventArgs e)
        {
            var selectedOptions = (e.Value as IEnumerable<string>)?.ToList();
            if (selectedOptions != null)
            {
                MinAmenities = selectedOptions
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(int.Parse)
                    .ToList();
            }
        }

        /// <summary>
        /// Called when favorite changed.
        /// </summary>
        /// <param name="place">The place.</param>
        protected async Task OnFavoriteChanged(PlaceListViewModel place)
        {
            PlaceFavorite = place;
            if (place.Favorite == null)
            {
                await _modal.Show();
                Favorites = await UserResource.GetPlaceFavorites();
            }
            else
            {
                var result = await PlaceResource.RemoveFavorite(PlaceFavorite.Id, PlaceFavorite.Favorite.Id);
                if (result)
                {
                    await ToastService.Success($"Succès de la suppression du lieu '{PlaceFavorite.Name}' du favori '{PlaceFavorite.Favorite.Name}'.", "Suppression du favori");
                    place.Favorite = null;
                }
                else
                {
                    await ToastService.Error($"Echec de la suppression du lieu '{PlaceFavorite.Name}' du favori '{PlaceFavorite.Favorite.Name}'.", "Suppression du favori");
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Called when favorite clicked.
        /// </summary>
        /// <param name="favorite">The favorite.</param>
        protected async Task OnFavoriteClicked(UserFavoriteViewModel favorite)
        {
            var result = await PlaceResource.PutFavorite(PlaceFavorite.Id, favorite.Id);

            if (result != null)
            {
                PlaceFavorite.Favorite = favorite;
                await ToastService.Success($"Succès de l'ajout du lieu '{PlaceFavorite.Name}' en favori '{favorite.Name}'.", "Ajout en favori");
            }
            else
            {
                await ToastService.Error($"Echec de l'ajout du lieu '{PlaceFavorite.Name}' en favori '{favorite.Name}'.", "Ajout en favori");
            }

            await _modal.Hide();
            PlaceFavorite = null;
            StateHasChanged();
        }

        /// <summary>
        /// Called when favorite clicked.
        /// </summary>
        protected async Task OnFavoriteClicked()
        {
            var result = await PlaceResource.PostFavorite(PlaceFavorite.Id, FavoriteName);

            if (result != null)
            {
                PlaceFavorite.Favorite = new PlaceFavoriteViewModel
                {
                    Id = result.Value,
                    Name = FavoriteName
                };
                await ToastService.Success($"Succès de l'ajout du lieu '{PlaceFavorite.Name}' en favori '{FavoriteName}'.", "Créé un favori");
            }
            else
            {
                await ToastService.Error($"Echec de l'ajout du lieu '{PlaceFavorite.Name}' en favori '{FavoriteName}'.", "Créé un favori");
            }

            PlaceFavorite = null;
            FavoriteName = null;
            await _modal.Hide();
            StateHasChanged();
        }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            _onSearchChanged = async (sender, e) => await SearchChanged();
            SearchService.OnSearchChanged += _onSearchChanged;
            await LoadDatas();
        }

        /// <inheritdoc />
        protected override async Task OnParametersSetAsync()
        {
            await LoadDatas();
            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// Called when review submit.
        /// </summary>
        protected async Task OnReviewSubmit()
        {
            ReviewSendInProgress = true;
            ReviewSendState = null;

            try
            {
                AddReview.PlaceId = PlaceId.Value;
                Reviews = await PlaceResource.AddReview(PlaceId.Value, AddReview);
                ReviewSendState = true;
                Place.HasUserReviewed = true;
            }
            catch
            {
                ReviewSendState = false;
            }
            finally
            {
                ReviewSendInProgress = false;
            }

            AddReview = new AddPlaceReviewCommand();
            StateHasChanged();
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
        /// Places the favorite closed.
        /// </summary>
        protected async Task PlaceFavoriteClosed()
        {
            PlaceFavorite = null;
            FavoriteName = null;
            await _modal.Hide();
            StateHasChanged();
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
                    Search = SearchQuery,
                    Rating = MinRating,
                    Amenities = MinAmenities
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

            if (PlaceTypeId.HasValue)
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
            Reviews = await PlaceResource.GetPlaceReviews(PlaceId.Value);
        }
    }
}
