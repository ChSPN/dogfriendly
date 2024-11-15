using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;

namespace DogFriendly.Web.Client.Services
{
    /// <summary>
    /// Unauthorized manager service.
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class UnauthorizedService : DelegatingHandler
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _js;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedService"/> class.
        /// </summary>
        /// <param name="navigationManager">The navigation manager.</param>
        /// <param name="js">The js.</param>
        public UnauthorizedService(NavigationManager navigationManager, IJSRuntime js)
        {
            _navigationManager = navigationManager;
            _js = js;
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _js.InvokeVoidAsync("showSpinner");
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _navigationManager.Refresh(true);
                }

                return response;
            }
            finally
            {
                await _js.InvokeVoidAsync("hideSpinner");
            }
        }
    }
}
