﻿using DogFriendly.Domain.Models;
using DogFriendly.Domain.ViewModels;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
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
        /// Create the specified user.
        /// </summary>
        /// <param name="userRegister">The register user model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseViewModel>> Create([FromBody] UserProfilViewModel userRegister)
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Adresse e-mail introuvable."
                });
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email, userRegister.UserName)
            {
                PictureContent = userRegister.PictureContent,
                PictureName = userRegister.PictureName,
                PictureUri = userRegister.UserPicture
            };

            // Register the user.
            var response = await userModel.Create();
            return this.Ok(response);
        }

        /// <summary>
        /// Gets the favorites.
        /// </summary>
        /// <returns></returns>
        [HttpGet("favorites")]
        public async Task<ActionResult<UserFavoriteViewModel>> GetPlaceFavorites()
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Adresse e-mail introuvable."
                });
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email);

            return Ok(await userModel.GetPlaceFavorites());
        }

        /// <summary>
        /// Gets the place reviews.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("reviews")]
        public async Task<ActionResult<List<UserReviewViewModel>>> GetPlaceReviews()
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest("Email claim not found.");
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email);

            // Get the user reviews.
            return Ok(await userModel.GetPlaceReviews());
        }

        /// <summary>
        /// Gets the places.
        /// </summary>
        /// <param name="favoriteId">The favorite identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("/api/user/favorite/{favoriteId:int}")]
        public async Task<ActionResult<List<PlaceListViewModel>>> GetPlaces(int favoriteId)
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Adresse e-mail introuvable."
                });
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email);

            return Ok(await userModel.GetPlaces(favoriteId));
        }

        /// <summary>
        /// Gets the profil.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("profil")]
        public async Task<ActionResult<UserProfilViewModel>> GetProfil()
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest("Email claim not found.");
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email);

            // Load the user.
            await userModel.Load();

            return Ok(new UserProfilViewModel
            {
                UserEmail = userModel.Email,
                UserName = userModel.Name,
                UserPicture = userModel.PictureUri
            });
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
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest("Email claim not found.");
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Retrieve the user's name from the claims.
            var nameClaim = User.FindFirst(JwtRegisteredClaimNames.Name);
            if (nameClaim == null)
            {
                return BadRequest("Name claim not found.");
            }

            // Get the email from the claim.
            var name = nameClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email, name);

            // Check if the user exists.
            var isExist = await userModel.IsExist();

            return Ok(isExist);
        }

        /// <summary>
        /// Updates the specified user register.
        /// </summary>
        /// <param name="userRegister">The user register.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ResponseViewModel>> Update([FromBody] UserProfilViewModel userRegister)
        {
            // Retrieve the user's email from the claims.
            var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
            if (emailClaim == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Adresse e-mail introuvable."
                });
            }

            // Get the email from the claim.
            var email = emailClaim.Value;

            // Create a new user model.
            var userModel = new UserModel(_mediator, email, userRegister.UserName)
            {
                PictureContent = userRegister.PictureContent,
                PictureName = userRegister.PictureName,
                PictureUri = userRegister.UserPicture
            };

            // Register the user.
            var response = await userModel.Update();
            return this.Ok(response);
        }
    }
}
