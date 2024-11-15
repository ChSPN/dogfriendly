using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DogFriendly.Admin.Controllers
{
    /// <summary>
    /// Controller for handling authentication.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly Services.AuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public AuthenticationController(Services.AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Logins the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string token)
        {
            await _authenticationService.LogIn(token);
            return base.SignIn(
                    _authenticationService.Identity,
                    new AuthenticationProperties
                    {
                        AllowRefresh = false,
                        IsPersistent = true
                    },
                    CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public SignOutResult Logout()
        {
            _authenticationService.LogOut();
            return base.SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
