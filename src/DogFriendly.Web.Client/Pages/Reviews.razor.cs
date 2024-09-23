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
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var isAuthenticated = await AuthenticationService.IsUserAuthenticated();
                if (!isAuthenticated)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
