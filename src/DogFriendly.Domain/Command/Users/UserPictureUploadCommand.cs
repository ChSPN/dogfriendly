using DogFriendly.Domain.Models;
using MediatR;

namespace DogFriendly.Domain.Command.Users
{
    /// <summary>
    /// Command for user picture upload.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;DogFriendly.Domain.ViewModels.ResponseViewModel&gt;" />
    public class UserPictureUploadCommand : IRequest<string?>
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public required UserModel User { get; set; }
    }
}
