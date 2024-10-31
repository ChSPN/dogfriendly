using Microsoft.AspNetCore.Mvc;

namespace DogFriendly.API.Controllers
{
    /// <summary>
    /// Firebase Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FirebaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetConfig()
        {
            return Ok(new
            {
                apiKey = _configuration["Firebase:ApiKey"],
                authDomain = _configuration["Firebase:AuthDomain"],
                projectId = _configuration["Firebase:ProjectId"],
                appId = _configuration["Firebase:AppId"]
            });
        }
    }
}
