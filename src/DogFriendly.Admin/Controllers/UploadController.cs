using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        /// <summary>
        /// Constructor of the controller.
        /// </summary>
        /// <param name="fileStorageRepository">Repository of file.</param>
        /// <param name="options">Options for storage.</param>
        public UploadController(IFileStorageRepository fileStorageRepository, IOptions<FileStorageOption> options)
        {
            _fileStorageRepository = fileStorageRepository;
            _fileStorageOptions = options.Value;
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
