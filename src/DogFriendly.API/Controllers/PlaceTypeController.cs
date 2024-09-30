using DogFriendly.Domain.Models;
using DogFriendly.Domain.ViewModels.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DogFriendly.API.Controllers
{
    /// <summary>
    /// Place Type Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceTypeController"/> class.
        /// </summary>
        /// <param name="mediator">The configuration.</param>
        public PlaceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the place types for view.
        /// </summary>
        /// <returns></returns>
        [HttpGet("view")]
        public async Task<ActionResult<UserProfilViewModel>> GetViewAll()
            => Ok(await PlaceTypeModel.LoadViewModel(_mediator));
    }
}
