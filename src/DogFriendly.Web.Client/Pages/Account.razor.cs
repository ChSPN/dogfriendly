using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Account page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Account : ComponentBase
    {
        /// <summary>
        /// Gets or sets the authentication service.
        /// </summary>
        /// <value>
        /// The authentication service.
        /// </value>
        [Inject]
        public required AuthenticationService Authentication { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        protected string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the user resource.
        /// </summary>
        /// <value>
        /// The user resource.
        /// </value>
        [Inject]
        protected IUserResource UserResource { get; set; }

        /// <summary>
        /// Gets or sets the register user model.
        /// </summary>
        /// <value>
        /// The register user model.
        /// </value>
        protected UserProfilViewModel ViewModel { get; set; }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var isAuthenticated = await Authentication.IsUserAuthenticated();
                if (!isAuthenticated)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            ViewModel = new UserProfilViewModel();
            if (Authentication.GetJwtToken() != null)
            {
                var profil = await Authentication.GetUserProfil();
                ViewModel.UserName = profil?.UserName;
                ViewModel.UserPicture = profil?.UserPicture;
            }
            else
            {
                NavigationManager.NavigateTo("/login");
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected async Task UpdateUser(UserProfilViewModel? model)
        {
            if (model.PictureContent != null)
            {
                model.UserPicture = null;
            }

            var response = await UserResource.Update(model);
            if (!response.IsSuccess)
            {
                ErrorMessage = response.Message;
            }
            else
            {
                model.PictureContent = null;
                model.PictureName = null;
                model.UserPicture = response.Message;
                Authentication.SetUserProfil(model);
            }

            StateHasChanged();
        }
    }
}
