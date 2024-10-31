using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DogFriendly.Admin.Services
{
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        private IServiceProvider _serviceProvider;

        public AuthenticationProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await GetCurrentUser();
            return new AuthenticationState(user);
        }

        private async Task<ClaimsPrincipal> GetCurrentUser()
        {
            var authenticationService = _serviceProvider.GetRequiredService<AuthenticationService>();
            await authenticationService.LoadAsync();
            return authenticationService.Identity ?? new ClaimsPrincipal(new ClaimsIdentity());
        }

        public void NotifyUserAuthentication(ClaimsPrincipal user)
        {
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyUserAuthentication(anonymousUser);
        }

        public void LoggedIn(ClaimsPrincipal user)
        {
            NotifyUserAuthentication(user);
        }
    }
}
