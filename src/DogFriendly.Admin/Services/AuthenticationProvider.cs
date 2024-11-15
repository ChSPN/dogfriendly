using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DogFriendly.Admin.Services
{
    /// <summary>
    /// Provides the authentication state for the application.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider" />
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public AuthenticationProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Asynchronously gets an <see cref="T:Microsoft.AspNetCore.Components.Authorization.AuthenticationState" /> that describes the current user.
        /// </summary>
        /// <returns>
        /// A task that, when resolved, gives an <see cref="T:Microsoft.AspNetCore.Components.Authorization.AuthenticationState" /> instance that describes the current user.
        /// </returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await GetCurrentUser();
            return new AuthenticationState(user);
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns></returns>
        private async Task<ClaimsPrincipal> GetCurrentUser()
        {
            var authenticationService = _serviceProvider.GetRequiredService<AuthenticationService>();
            await authenticationService.LoadAsync();
            return authenticationService.Identity ?? new ClaimsPrincipal(new ClaimsIdentity());
        }

        /// <summary>
        /// Notifies the user authentication.
        /// </summary>
        /// <param name="user">The user.</param>
        public void NotifyUserAuthentication(ClaimsPrincipal user)
        {
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// Logged out user.
        /// </summary>
        public void LoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyUserAuthentication(anonymousUser);
        }

        /// <summary>
        /// Logged in user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void LoggedIn(ClaimsPrincipal user)
        {
            NotifyUserAuthentication(user);
        }
    }
}
