using DogFriendly.Domain.Models;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DogFriendly.API.Controllers
{
    /// <summary>
    /// Place Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceController"/> class.
        /// </summary>
        /// <param name="mediator">The configuration.</param>
        public PlaceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Searches the specified place.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<PlaceListViewModel>>> Search(PlaceListViewQuery request)
            => Ok(await PlaceModel.Search(_mediator, request));
    }
}
