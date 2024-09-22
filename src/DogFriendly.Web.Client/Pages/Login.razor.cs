using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Login component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Login : ComponentBase
    {
        /// <summary>
        /// Gets or sets the js runtime.
        /// </summary>
        /// <value>
        /// The js runtime.
        /// </value>
        [Inject]
        public required IJSRuntime JsRuntime { get; set; }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("initFirebaseUi");
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
