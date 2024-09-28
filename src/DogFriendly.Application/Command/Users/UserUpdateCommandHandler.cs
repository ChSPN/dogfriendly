using DogFriendly.Domain.Command.Users;
using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;

namespace DogFriendly.Application.Command.Users
{
    /// <summary>
    /// Handler for user update command.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Users.UserUpdateCommand, System.Boolean&gt;" />
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, bool>
    {
        private readonly IRepository<UserEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserUpdateCommandHandler(IRepository<UserEntity> repository, 
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var query = _repository
                    .SingleResultQuery()
                    .AndFilter(u => u.Email == request.User.Email);
                var user = await _repository.FirstOrDefaultAsync(query, cancellationToken);
                if (user == null)
                {
                    return false;
                }

                user.Name = request.User.Name;
                user.PhotoUri = request.User.PictureUri;
                _repository.Update(user);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
    }
}
