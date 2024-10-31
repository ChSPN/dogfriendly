using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Register page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Register : ComponentBase
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        protected string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (AuthenticationService.JwtToken == null
                    || await UserResource.IsExist())
                {
                    NavigationManager.NavigateTo("/login");
                    return;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            ViewModel = new UserProfilViewModel();
            if (AuthenticationService.JwtToken != null)
            {
                ViewModel.UserName = AuthenticationService.JwtToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?
                    .Value;
                ViewModel.UserPicture = AuthenticationService.JwtToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Picture)?
                    .Value;
            }
            else
            {
                NavigationManager.NavigateTo("/login");
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        protected async Task RegisterUser(UserProfilViewModel? model)
        {
            if (model.PictureContent != null)
            {
                model.UserPicture = null;
            }

            var response = await UserResource.Register(model);
            if (response.IsSuccess)
            {
                NavigationManager.NavigateTo("/login");
                return;
            }
            else
            {
                ErrorMessage = response.Message;
                StateHasChanged();
            }
        }
    }
}
