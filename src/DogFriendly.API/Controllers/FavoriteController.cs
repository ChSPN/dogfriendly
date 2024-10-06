using DogFriendly.Domain.Models;
using DogFriendly.Domain.ViewModels.Favorites;
using DogFriendly.Domain.ViewModels.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace DogFriendly.API.Controllers
{
    /// <summary>
    /// Favorite Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteController"/> class.
        /// </summary>
        /// <param name="mediator">The configuration.</param>
        public FavoriteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the favorites for view.
        /// </summary>
        /// <returns></returns>
        [HttpGet("view")]
        public async Task<ActionResult<FavoriteListViewModel>> GetViewAll()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            return Ok(await FavoriteModel.LoadViewModel(_mediator, emailClaim?.Value));
        }
    }
}
