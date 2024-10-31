using MediatR;

namespace DogFriendly.Domain.Queries.Users
{
    /// <summary>
    /// Query to check if user exists.
    /// </summary>
    public class UserNameExistQuery : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public required string Name { get; set; }
    }
}
