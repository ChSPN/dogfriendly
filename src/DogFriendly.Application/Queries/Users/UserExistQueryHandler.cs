using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;

namespace DogFriendly.Application.Queries.Users
{
    /// <summary>
    /// Handler for <see cref="UserExistQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Application.Queries.Users.UserExistQuery, System.Boolean&gt;" />
    public class UserExistQueryHandler : IRequestHandler<UserExistQuery, bool>
    {
        private readonly IRepository<UserEntity> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserExistQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserExistQueryHandler(IRepository<UserEntity> userRepository) 
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
        public async Task<bool> Handle(UserExistQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(x => x.Email == request.Email);
        }
    }
}
