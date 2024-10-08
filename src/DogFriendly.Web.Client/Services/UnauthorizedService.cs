using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;

namespace DogFriendly.Web.Client.Services
{
    public class UnauthorizedService : DelegatingHandler
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _js;

        public UnauthorizedService(NavigationManager navigationManager, IJSRuntime js)
        {
            _navigationManager = navigationManager;
            _js = js;
        }

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
