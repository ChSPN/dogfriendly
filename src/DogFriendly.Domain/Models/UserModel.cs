using DogFriendly.Domain.Command.Users;
using DogFriendly.Domain.Queries.Users;
using DogFriendly.Domain.ViewModels;
using MediatR;

namespace DogFriendly.Domain.Models
{
    /// <summary>
    /// Model for user.
    /// </summary>
    public class UserModel
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserModel"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="userEmail">The user email.</param>
        /// <param name="userName">Name of the user.</param>
        public UserModel(IMediator mediator, string userEmail, string? userName = null)
        {
            _mediator = mediator;
            Email = userEmail;
            Name = userName ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string Email { get; private set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the content of the picture.
        /// </summary>
        /// <value>
        /// The content of the picture.
        /// </value>
        public byte[]? PictureContent { get; set; }

        /// <summary>
        /// Gets or sets the name of the picture.
        /// </summary>
        /// <value>
        /// The name of the picture.
        /// </value>
        public string? PictureName { get; set; }

        /// <summary>
        /// Gets or sets the picture URI.
        /// </summary>
        /// <value>
        /// The picture URI.
        /// </value>
        public string? PictureUri { get; set; }

        /// <summary>
        /// Determines whether is exist.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is exist; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsExist()
            => await _mediator.Send(new UserEmailExistQuery
            {
                Email = Email
            });

        /// <summary>
        /// Loads this user.
        /// </summary>
        public async Task Load()
        {
            var user = await _mediator.Send(new UserLoadQuery
            {
                Email = Email
            });

            if (user != null)
            {
                PictureUri = user.PhotoUri;
                Name = user.Name;
            }
        }

        /// <summary>
        /// Registers this user.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseViewModel> Register()
        {
            // Check if user name is exist.
            var isExist = await _mediator.Send(new UserNameExistQuery
            {
                Name = Name
            });
            if (isExist)
            {
                return new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Ce pseudo est déjà utilisé."
                };
            }

            // Upload user picture.
            PictureUri = await _mediator.Send(new UserPictureUploadCommand
            {
                User = this
            });

            // Register user.
            var isSuccess = await _mediator.Send(new UserRegisterCommand
            {
                User = this
            });

            // Return response.
            return new ResponseViewModel
            {
                IsSuccess = isSuccess,
                Message = isSuccess 
                ? string.Empty 
                : "Une erreur est survenue lors de la création de compte."
            };
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update()
        {
            // Load user.
            var user = await _mediator.Send(new UserLoadQuery
            {
                Email = Email
            });
            if (user.Name != Name)
            {
                // Check if user name is exist.
                var isExist = await _mediator.Send(new UserNameExistQuery
                {
                    Name = Name
                });
                if (isExist)
                {
                    return new ResponseViewModel
                    {
                        IsSuccess = false,
                        Message = "Ce pseudo est déjà utilisé."
                    };
                }
            }

            // Upload user picture.
            PictureUri = await _mediator.Send(new UserPictureUploadCommand
            {
                User = this
            });

            // Update user.
            var isSuccess = await _mediator.Send(new UserUpdateCommand
            {
                User = this
            });

            // Return response.
            return new ResponseViewModel
            {
                IsSuccess = isSuccess,
                Message = isSuccess
                    ? PictureUri
                    : "Une erreur est survenue lors de la modification de compte."
            };
        }
    }
}
