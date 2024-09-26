using DogFriendly.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogFriendly.API.Controllers
{
    /// <summary>
    /// Firebase Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;


        /// <summary>
        /// Initializes a new instance of the <see cref="FirebaseController"/> class.
        /// </summary>
        /// <param name="mediator">The configuration.</param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Determines whether this user is exist.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("exist")]
        public async Task<ActionResult<bool>> IsExist()
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst("email");
            if (emailClaim == null)
            {
                return BadRequest("Email claim not found.");
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Send the UserExistQuery with the user's email.
            var isExist = await _mediator.Send(new UserExistQuery
            {
                Email = email
            });

            return Ok(isExist);
        }
    }
}
