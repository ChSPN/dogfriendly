using DogFriendly.Domain.Models;
using MediatR;

namespace DogFriendly.Domain.Command.Users
{
    /// <summary>
    /// Command to register a user.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;ResourceResponseModel&gt;" />
    public class UserRegisterCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the register user.
        /// </summary>
        /// <value>
        /// The register user.
        /// </value>
        public required UserModel User { get; set; }
    }
}
