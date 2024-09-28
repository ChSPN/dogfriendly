using DogFriendly.Domain.Resources;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;

namespace DogFriendly.Web.Client.Layout
{
    /// <summary>
    /// Navigation menu component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class NavMenu : ComponentBase, IDisposable
    {
        private EventHandler<JwtSecurityToken> OnUserChanged;

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        [Inject]
        protected IServiceProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            AuthenticationService.OnUserChanged -= OnUserChanged;
        }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            OnUserChanged = async (sender, e) => await SetIsAuthenticated(e);
            await SetIsAuthenticated(AuthenticationService.JwtToken);
            AuthenticationService.OnUserChanged += OnUserChanged;
            base.OnInitialized();
        }

        /// <summary>
        /// Sets the is authenticated.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private async Task SetIsAuthenticated(JwtSecurityToken? token)
        {
            if (token != null)
            {
                try
                {
                    var service = Provider.GetRequiredService<IUserResource>();
                    var profil = await service.GetProfil();
                    IsAuthenticated = !string.IsNullOrEmpty(profil?.UserName);
                }
                catch
                {
                    IsAuthenticated = false;
                }
            }
            else
            {
                IsAuthenticated = false;
            }

            StateHasChanged();
        }
    }
}
