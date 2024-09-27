using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

namespace DogFriendly.Web.Client.Layout
{
    /// <summary>
    /// Navigation menu component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class NavMenu : ComponentBase, IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the js runtime.
        /// </summary>
        /// <value>
        /// The js runtime.
        /// </value>
        [Inject]
        public required IJSRuntime JsRuntime { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            AuthenticationService.OnUserChanged -= SetIsAuthenticated;
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            AuthenticationService.OnUserChanged += SetIsAuthenticated;
            base.OnInitialized();
        }

        /// <summary>
        /// Sets the is authenticated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private void SetIsAuthenticated(object sender, JwtSecurityToken token)
        {
            IsAuthenticated = token != null;
            StateHasChanged();
        }
    }
}
