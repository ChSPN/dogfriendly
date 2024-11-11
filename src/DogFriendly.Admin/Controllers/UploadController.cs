using DogFriendly.Domain.Models;
using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace DogFriendly.Admin.Controllers
{
    /// <summary>
    /// Controller for uploading files to the server.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : Controller
    {
        private readonly FileStorageOption _fileStorageOptions;
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor of the controller.
        /// </summary>
        /// <param name="fileStorageRepository">Repository of file.</param>
        /// <param name="options">Options for storage.</param>
        /// <param name="mediator">Mediator for sending commands.</param>
        public UploadController(IFileStorageRepository fileStorageRepository, 
            IOptions<FileStorageOption> options,
            IMediator mediator)
        {
            _fileStorageRepository = fileStorageRepository;
            _fileStorageOptions = options.Value;
            _mediator = mediator;
        }

        /// <summary>
        /// Imports the places.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        [HttpPost("places/import")]
        public async Task<ActionResult> ImportPlaces(IFormFile file)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
                await PlaceModel.ImportExcel(_mediator, file.OpenReadStream(), emailClaim?.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Uploads the image of the place.
        /// </summary>
        /// <param name="files">Files.</param>
        /// <returns></returns>
        [HttpPost("places/photos")]
        public async Task<ActionResult> ImportPlacesPhotos(ICollection<IFormFile> files)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email) ?? User.FindFirst(JwtRegisteredClaimNames.Email);
                await PlaceModel.ImportPhotos(_mediator, 
                    files.ToDictionary(f => f.FileName, f => f.OpenReadStream()), 
                    emailClaim?.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Uploads the image of the place.
        /// </summary>
        /// <param name="files">Files.</param>
        /// <returns></returns>
        [HttpPost("place")]
        public async Task<ActionResult> UploadPlace(ICollection<IFormFile> files)
        {
            try
            {
                var list = await Upload(files, _fileStorageOptions.PlacesUri);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<List<string>> Upload(ICollection<IFormFile> files, string container)
        {
            var list = new List<string>();

            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                using (var reader = new MemoryStream())
                {
                    stream.CopyTo(reader);
                    var result = await _fileStorageRepository.UploadFileAsync($"{container}/{file.FileName}", reader.ToArray());
                    if (result)
                        list.Add($"{_fileStorageOptions.DomainUri}/{container}/{file.FileName}");
                }
            }

            return list;
        }
    }
}
