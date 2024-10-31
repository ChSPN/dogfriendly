using DogFriendly.Domain.Models;
using DogFriendly.Domain.ViewModels.Amenities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DogFriendly.API.Controllers
{
    /// <summary>
    /// Amenity Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityController"/> class.
        /// </summary>
        /// <param name="mediator">The configuration.</param>
        public AmenityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the amenities for view.
        /// </summary>
        /// <returns></returns>
        [HttpGet("view")]
        public async Task<ActionResult<List<AmenityListViewModel>>> GetViewAll()
            => Ok(await AmenityModel.LoadViewModel(_mediator));
    }
}
