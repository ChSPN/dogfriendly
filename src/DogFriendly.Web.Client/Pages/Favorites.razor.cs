using Blazorise;
using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Favorites page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Favorites : ComponentBase
    {
        /// <summary>
        /// Gets or sets the authentication service.
        /// </summary>
        /// <value>
        /// The authentication service.
        /// </value>
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        [Inject]
        public required IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the place resource.
        /// </summary>
        /// <value>
        /// The place resource.
        /// </value>
        [Inject]
        public required IPlaceResource PlaceResource { get; set; }

        /// <summary>
        /// Gets or sets the toast service.
        /// </summary>
        /// <value>
        /// The toast service.
        /// </value>
        [Inject]
        public required IToastService ToastService { get; set; }

        /// <summary>
        /// Gets or sets the user resource.
        /// </summary>
        /// <value>
        /// The user resource.
        /// </value>
        [Inject]
        public required IUserResource UserResource { get; set; }

        /// <summary>
        /// Gets or sets the selected favorite.
        /// </summary>
        /// <value>
        /// The selected favorite.
        /// </value>
        protected UserFavoriteViewModel? Favorite { get; set; }

        /// <summary>
        /// Gets or sets the name of the favorite.
        /// </summary>
        /// <value>
        /// The name of the favorite.
        /// </value>
        protected string? FavoriteName { get; set; }

        /// <summary>
        /// Gets or sets the places.
        /// </summary>
        /// <value>
        /// The places.
        /// </value>
        protected List<PlaceListViewModel> Places { get; set; } = new List<PlaceListViewModel>();

        /// <summary>
        /// Gets or sets the user favorites.
        /// </summary>
        /// <value>
        /// The user favorites.
        /// </value>
        protected List<UserFavoriteViewModel> UserFavorites { get;set; } = new List<UserFavoriteViewModel>();
        
        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(200);
                var isAuthenticated = await AuthenticationService.IsUserAuthenticated();
                if (!isAuthenticated)
                {
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    UserFavorites = await UserResource.GetPlaceFavorites();
                    Favorite = UserFavorites.FirstOrDefault();
                    if (Favorite != null)
                    {
                        await OnSelectedFavorite(Favorite);
                    }
                    else
                    {
                        StateHasChanged();
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Called when favorite clicked.
        /// </summary>
        protected async Task OnFavoriteClicked()
        {
            var result = await PlaceResource.PostFavorite(0, FavoriteName);

            if (result != null)
            {
                UserFavorites.Insert(0, new UserFavoriteViewModel
                {
                    Id = result.Value,
                    Name = FavoriteName
                });
                await ToastService.Success($"Succès de l'ajout de la liste '{FavoriteName}'.", "Créée une liste");
            }
            else
            {
                await ToastService.Error($"Echec de l'ajout de la liste '{FavoriteName}'.", "Créée une liste");
            }

            FavoriteName = null;
            StateHasChanged();
        }

        /// <summary>
        /// Called when favorite deleted.
        /// </summary>
        /// <returns></returns>
        protected async Task OnFavoriteDeleted()
        {
            if (Favorite != null)
            {
                var result = await PlaceResource.RemoveFavorite(0, Favorite.Id);
                if (result)
                {
                    await ToastService.Success($"Succès de la suppression de la liste '{Favorite.Name}'.", "Suppression de la liste");
                    UserFavorites.Remove(Favorite);
                    Favorite = UserFavorites.FirstOrDefault();
                    if (Favorite != null)
                    {
                        await OnSelectedFavorite(Favorite);
                    }
                }
                else
                {
                    await ToastService.Error($"Echec de la suppression de la liste '{Favorite.Name}'.", "Suppression de liste");
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Called when selected favorite changed.
        /// </summary>
        /// <param name="favorite">The favorite.</param>
        /// <returns></returns>
        protected async Task OnSelectedFavorite(UserFavoriteViewModel favorite)
        {
            Favorite = favorite;
            Places = await UserResource.GetPlaces(favorite.Id);
            StateHasChanged();
        }
    }
}
