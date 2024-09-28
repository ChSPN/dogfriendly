using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Users;
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

        private readonly IServiceProvider _provider;

        /// <summary>
        /// Gets the user profil.
        /// </summary>
        /// <value>
        /// The user profil.
        /// </value>
        private UserProfilViewModel? _userProfil;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="provider">The service provider.</param>
        public AuthenticationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// The token JWT.
        /// </summary>
        public static JwtSecurityToken? JwtToken { get; private set; }
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
        /// Gets the user profil.
        /// </summary>
        /// <returns></returns>
        public async Task<UserProfilViewModel?> GetUserProfil()
        {
            if (_userProfil == null && JwtToken != null)
            {
                _userProfil = await _provider
                    .GetRequiredService<IUserResource>()
                    .GetProfil();
            }

            return _userProfil;
        }

        /// <summary>
        /// Determines whether is user authenticated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is user authenticated; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsUserAuthenticated()
        {
            return await _provider
                .GetRequiredService<IJSRuntime>()
                .InvokeAsync<bool>("isFirebaseUserAuth");
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        public async Task Logout()
        {
            _userProfil = null;
            JwtToken = null;
            await _provider
                .GetRequiredService<IJSRuntime>()
                .InvokeVoidAsync("logoutFirebaseAuth");
            OnUserChanged?.Invoke(null, null);
        }

        /// <summary>
        /// Sets the user profil.
        /// </summary>
        /// <param name="userProfil">The user profil.</param>
        public void SetUserProfil(UserProfilViewModel userProfil)
        {
            _userProfil = userProfil;
        }
    }
}
