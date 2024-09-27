using MediatR;

namespace DogFriendly.Domain.Queries.Users
{
    /// <summary>
    /// Query to check if user exists.
    /// </summary>
    public class UserEmailExistQuery : IRequest<bool>
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
