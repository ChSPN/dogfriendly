using DogFriendly.Domain.Entitites;
using MediatR;

namespace DogFriendly.Domain.Queries.Users
{
    /// <summary>
    /// Query for loading user.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;DogFriendly.Domain.Entitites.UserEntity&gt;" />
    public class UserLoadQuery : IRequest<UserEntity>
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public required string Email { get; set; }
    }
}
