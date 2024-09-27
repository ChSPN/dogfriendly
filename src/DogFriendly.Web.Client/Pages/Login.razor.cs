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
        /// <summary>
        /// Gets or sets the js runtime.
        /// </summary>
        /// <value>
        /// The js runtime.
        /// </value>
        [Inject]
        public required IJSRuntime JsRuntime { get; set; }


        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? UserName { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            AuthenticationService.OnUserChanged -= SetUserName;
        }

        /// <inheritdoc />
        protected override Task OnInitializedAsync()
        {
            if (AuthenticationService.JwtToken is JwtSecurityToken token)
            {
                SetUserName(this, token);
            }

            return base.OnInitializedAsync();
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("initFirebaseUi");
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            AuthenticationService.OnUserChanged += SetUserName;
            base.OnInitialized();
        }

        /// <summary>
        /// Sets the name of the user.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private void SetUserName(object sender, JwtSecurityToken token)
        {
            UserName = token?.Claims?.FirstOrDefault(c => c.Type == "name")?.Value;
            StateHasChanged();
        }
    }
}
