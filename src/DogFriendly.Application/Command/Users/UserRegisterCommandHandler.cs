using DogFriendly.Domain.Command.Users;
using DogFriendly.Domain.Entitites;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using MediatR;

namespace DogFriendly.Application.Command.Users
{
    /// <summary>
    /// Handler for user register command.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Command.Users.UserRegisterCommand, System.Boolean&gt;" />
    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, bool>
    {
        private readonly IRepository<UserEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserRegisterCommandHandler(IRepository<UserEntity> repository, 
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var user = new UserEntity
                {
                    Email = request.User.Email,
                    Name = request.User.Name,
                    PhotoUri = request.User.PictureUri,
                    CreatedBy = request.User.Email,
                    CreatedAt = DateTimeOffset.UtcNow
                };
                await _repository.AddAsync(user);
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
