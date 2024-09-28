using DogFriendly.Domain.Models;
using MediatR;

namespace DogFriendly.Domain.Command.Users
{
    /// <summary>
    /// Command to update a user.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;ResourceResponseModel&gt;" />
    public class UserUpdateCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the update user.
        /// </summary>
        /// <value>
        /// The update user.
        /// </value>
        public required UserModel User { get; set; }
    }
}
