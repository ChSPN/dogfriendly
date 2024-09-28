using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Users;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;

namespace DogFriendly.Application.Queries.Users
{
    /// <summary>
    /// Handler for <see cref="UserLoadQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Application.Queries.Users.UserExistQuery, System.Boolean&gt;" />
    public class UserLoadQueryHandler : IRequestHandler<UserLoadQuery, UserEntity>
    {
        private readonly IRepository<UserEntity> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameExistQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserLoadQueryHandler(IRepository<UserEntity> userRepository) 
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
        public async Task<UserEntity> Handle(UserLoadQuery request, CancellationToken cancellationToken)
        {
            var query = _userRepository
                .SingleResultQuery()
                .AndFilter(u => u.Email == request.Email);
            return await _userRepository.FirstOrDefaultAsync(query, cancellationToken);
        }
    }
}
