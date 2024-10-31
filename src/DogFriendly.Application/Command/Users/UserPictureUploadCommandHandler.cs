using DogFriendly.Domain.Command.Users;
using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace DogFriendly.Application.Command.Users
{
    /// <summary>
    /// Handler for user picture upload command.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Users.UserPictureUploadCommand, System.Boolean&gt;" />
    public class UserPictureUploadCommandHandler : IRequestHandler<UserPictureUploadCommand, string?>
    {
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly FileStorageOption _fileStorageOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPictureUploadCommandHandler"/> class.
        /// </summary>
        /// <param name="fileStorageRepository">The file storage repository.</param>
        /// <param name="fileStorageOptions">The file storage options.</param>
        public UserPictureUploadCommandHandler(IFileStorageRepository fileStorageRepository, 
            IOptions<FileStorageOption> fileStorageOptions)
        {
            _fileStorageRepository = fileStorageRepository;
            _fileStorageOptions = fileStorageOptions.Value;
        }

        /// <inheritdoc />
        public async Task<string?> Handle(UserPictureUploadCommand request, CancellationToken cancellationToken)
        {
            if (request.User.PictureContent == null 
                || request.User.PictureContent.Length == 0
                || request.User.PictureName == null)
            {
                return request.User.PictureUri;
            }

            var fileName = $"{_fileStorageOptions.UsersUri}/{request.User.Name.ToLower()}/{request.User.PictureName}";
            var isSuccess = await _fileStorageRepository.UploadFileAsync(fileName, request.User.PictureContent);
            return isSuccess ? $"{_fileStorageOptions.DomainUri}/{fileName}" : null;
        }
    }
}
