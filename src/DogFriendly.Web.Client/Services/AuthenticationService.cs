using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

namespace DogFriendly.Web.Client.Services
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthenticationService
    {
        /// <summary>
        /// The user changed.
        /// </summary>
        public static EventHandler<JwtSecurityToken> OnUserChanged;

        /// <summary>
        /// The token JWT.
        /// </summary>
        private static JwtSecurityToken? JwtToken;

        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        public AuthenticationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Sets the token JWT.
        /// </summary>
        /// <param name="jwt">The JWT.</param>
        /// <returns></returns>
        [JSInvokable]
        public static Task SetJwtToken(string jwt)
        {
            if (jwt != null)
            {
                var handler = new JwtSecurityTokenHandler();
                JwtToken = handler.ReadToken(jwt) as JwtSecurityToken;
            }
            else
            {
                JwtToken = null;
            }

            OnUserChanged?.Invoke(null, JwtToken);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the token JWT.
        /// </summary>
        /// <returns></returns>
        public JwtSecurityToken? GetJwtToken() => JwtToken;

        /// <summary>
        /// Determines whether is user authenticated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is user authenticated; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsUserAuthenticated()
        {
            return await _jsRuntime.InvokeAsync<bool>("isFirebaseUserAuth");
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        public async Task SignOut()
        {
            await _jsRuntime.InvokeVoidAsync("logoutFirebaseAuth");
        }
    }
}
