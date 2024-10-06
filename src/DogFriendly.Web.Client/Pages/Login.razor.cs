using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Login component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Login : ComponentBase, IDisposable
    {
        private EventHandler<JwtSecurityToken> OnUserChanged;

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        [Inject]
        public required IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Gets the name of the user profil.
        /// </summary>
        /// <value>
        /// The name of the user profil.
        /// </value>
        public UserProfilViewModel? UserProfil { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            AuthenticationService.OnUserChanged -= OnUserChanged;
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        protected async Task Logout()
        {
            await ServiceProvider
                .GetRequiredService<AuthenticationService>()
                .Logout();
            await UserChanged(null);
            StateHasChanged();
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await ServiceProvider
                .GetRequiredService<IJSRuntime>()
                .InvokeVoidAsync("initFirebaseUi");
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <inheritdoc />
        protected async override Task OnInitializedAsync()
        {
            OnUserChanged = async (sender, e) => await UserChanged(e);
            AuthenticationService.OnUserChanged += OnUserChanged;
            if (AuthenticationService.JwtToken is JwtSecurityToken token)
            {
                await UserChanged(token);
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Users the changed.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private async Task UserChanged(JwtSecurityToken? token)
        {
            if (token != null)
            {
                try
                {
                    var userExist = await ServiceProvider
                        .GetRequiredService<IUserResource>()
                        .IsExist();
                    if (!userExist)
                    {
                        NavigationManager.NavigateTo("/register");
                        return;
                    }
                }
                catch 
                {
                    return;
                }
            }

            UserProfil = await ServiceProvider
                .GetRequiredService<AuthenticationService>()
                .GetUserProfil();
            StateHasChanged();
        }
    }
}
