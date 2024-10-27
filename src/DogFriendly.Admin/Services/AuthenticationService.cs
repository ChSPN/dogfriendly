using DogFriendly.Domain.Entitites;
using DogFriendly.Infrastructure.Context;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DogFriendly.Admin.Services
{
    public class AuthenticationService
    {
        private readonly FirebaseAuth _firebaseAuth;
        private readonly AuthenticationProvider _provider;
        private readonly IServiceProvider _serviceProvider;

        public AuthenticationService(FirebaseAuth firebaseAuth, 
            AuthenticationProvider provider,
            IServiceProvider serviceProvider)
        {
            _firebaseAuth = firebaseAuth;
            _provider = provider;
            _serviceProvider = serviceProvider;
        }

        public ClaimsPrincipal? Identity { get; private set; }

        public bool IsAdmin => IsAuthenticated
            && Identity?.HasClaim(ClaimTypes.Role, "Admin") == true;

        public bool IsAuthenticated => Identity != null;

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

        public void LogOut()
        {
            Identity = null;
            _provider.LoggedOut();
        }
    }
}
