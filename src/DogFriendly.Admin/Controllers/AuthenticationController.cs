using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DogFriendly.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly Services.AuthenticationService _authenticationService;

        public AuthenticationController(Services.AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

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

        [HttpPost("logout")]
        public SignOutResult Logout()
        {
            _authenticationService.LogOut();
            return base.SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
