using DogFriendly.Domain.Command.Reviews;
using DogFriendly.Domain.Models;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Reviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        /// Adds the favorite.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="favoriteId">The favorite identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{placeId:int}/favorite")]
        public async Task<ActionResult<int?>> AddFavorite([FromRoute] int placeId,
            [FromForm] int favoriteId)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            var place = new PlaceModel(_mediator, placeId);
            return Ok(await place.AddFavorite(favoriteId, emailClaim?.Value));
        }

        /// <summary>
        /// Adds the favorite.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="favoriteName">Name of the favorite.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{placeId:int}/favorite")]
        public async Task<ActionResult<int?>> AddFavorite([FromRoute] int placeId,
            [FromForm] string favoriteName)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            var place = new PlaceModel(_mediator, placeId);
            return Ok(await place.AddFavorite(favoriteName, emailClaim?.Value));
        }

        /// <summary>
        /// Adds the review.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("review")]
        public async Task<ActionResult<List<PlaceReviewViewModel>>> AddReview(
            [FromBody] AddPlaceReviewCommand request)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            request.UserEmail = emailClaim?.Value;
            var place = new PlaceModel(_mediator, request.PlaceId);
            await place.AddReview(request);
            return Ok(await place.GetPlaceReviews());
        }

        /// <summary>
        /// Gets the reviews.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("reviews/{id:int}")]
        public async Task<ActionResult<List<PlaceReviewViewModel>>> GetReviews(int id)
        {
            var place = new PlaceModel(_mediator, id);
            return Ok(await place.GetPlaceReviews());
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("view/{id:int}")]
        public async Task<ActionResult<PlaceViewModel>> GetViewModel(int id)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            return Ok(await PlaceModel.LoadViewModel(_mediator, id, emailClaim?.Value));
        }

        /// <summary>
        /// Removes the favorite.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <param name="favoriteId">The favorite identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{placeId:int}/favorite/{favoriteId:int}")]
        public async Task<ActionResult<bool>> RemoveFavorite([FromRoute] int placeId,
            [FromRoute] int favoriteId)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            var place = new PlaceModel(_mediator, placeId);
            return Ok(await place.RemoveFavorite(favoriteId, emailClaim?.Value));
        }

        /// <summary>
        /// Searches the specified place.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<PlaceListViewModel>>> Search(
            PlaceListViewQuery request)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            request.UserEmail = emailClaim?.Value;
            return Ok(await PlaceModel.Search(_mediator, request));
        }
    }
}
