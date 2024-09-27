using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Users;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;

namespace DogFriendly.Application.Queries.Users
{
    /// <summary>
    /// Handler for <see cref="UserEmailExistQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Application.Queries.Users.UserExistQuery, System.Boolean&gt;" />
    public class UserEmailExistQueryHandler : IRequestHandler<UserEmailExistQuery, bool>
    {
        private readonly IRepository<UserEntity> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEmailExistQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserEmailExistQueryHandler(IRepository<UserEntity> userRepository) 
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> Handle(UserEmailExistQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(x => x.Email == request.Email);
        }
    }
}
