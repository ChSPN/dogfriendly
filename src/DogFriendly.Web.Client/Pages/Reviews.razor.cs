using Blazorise;
using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Reviews page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Reviews : ComponentBase
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
        /// Gets or sets the user reviews.
        /// </summary>
        /// <value>
        /// The user reviews.
        /// </value>
        protected List<UserReviewViewModel>? UserReviews { get; set; }

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
                    UserReviews = await UserResource.GetPlaceReviews();
                    StateHasChanged();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Called when review delete.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        protected async Task OnReviewDelete(UserReviewViewModel review)
        {
            var result = await PlaceResource.RemoveReview(review.Place.Id, review.Review.Id);
            if (result)
            {
                UserReviews?.Remove(review);
                await ToastService.Success($"Succès de la suppression de l'avis sur '{review.Place.Name}'.", "Suppression de l'avis");
                StateHasChanged();
            }
            else
            {
                await ToastService.Error($"Echec de la suppression de l'avis sur '{review.Place.Name}'.", "Suppression de l'avis");
            }
        }
    }
}
