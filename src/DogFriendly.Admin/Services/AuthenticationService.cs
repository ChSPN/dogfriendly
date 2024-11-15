using DogFriendly.Domain.Entitites;
using DogFriendly.Infrastructure.Context;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DogFriendly.Admin.Services
{
    /// <summary>
    /// Service for managing user authentication.
    /// </summary>
    public class AuthenticationService
    {
        private readonly FirebaseAuth _firebaseAuth;
        private readonly AuthenticationProvider _provider;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="firebaseAuth">The firebase authentication.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public AuthenticationService(FirebaseAuth firebaseAuth, 
            AuthenticationProvider provider,
            IServiceProvider serviceProvider)
        {
            _firebaseAuth = firebaseAuth;
            _provider = provider;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <value>
        /// The identity.
        /// </value>
        public ClaimsPrincipal? Identity { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdmin => IsAuthenticated
            && Identity?.HasClaim(ClaimTypes.Role, "Admin") == true;

        /// <summary>
        /// Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated => Identity != null;

        /// <summary>
        /// Loads the asynchronous.
        /// </summary>
        public async Task LoadAsync()
        {
            if (Identity == null)
            {
                var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
                if (httpContextAccessor.HttpContext != null)
                {
                    var context = httpContextAccessor.HttpContext;
                    var result = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    if (result.Succeeded)
                    {
                        Identity = result.Principal;
                        _provider.LoggedIn(Identity);
                    }
                }
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="token">The token.</param>
        public async Task LogIn(string token)
        {
            var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(token);
            string email = decodedToken?.Claims["email"]?.ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, decodedToken?.Uid),
                new Claim(ClaimTypes.Name, decodedToken?.Claims["name"]?.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            if ((await _serviceProvider.GetRequiredService<DogFriendlyContext>().Users.Where(u => u.Email == email).FirstOrDefaultAsync()) is UserEntity user
                && user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            Identity = new ClaimsPrincipal(claimsIdentity);
            _provider.LoggedIn(Identity);
        }

        /// <summary>
        /// Logout the user.
        /// </summary>
        public void LogOut()
        {
            Identity = null;
            _provider.LoggedOut();
        }
    }
}
