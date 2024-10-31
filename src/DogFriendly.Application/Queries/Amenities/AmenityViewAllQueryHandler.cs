using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Amenities;
using DogFriendly.Domain.ViewModels.Amenities;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Amenities
{
    /// <summary>
    /// Handler for <see cref="AmenityViewAllQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Amenities.AmenityViewAllQuery, System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Amenities.AmenityListViewModel&gt;&gt;" />
    public class AmenityViewAllQueryHandler : IRequestHandler<AmenityViewAllQuery, List<AmenityListViewModel>>
    {
        private readonly IRepository<AmenityEntity> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityViewAllQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AmenityViewAllQueryHandler(IRepository<AmenityEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<List<AmenityListViewModel>> Handle(AmenityViewAllQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                .SingleResultQuery()
                .OrderBy(p => p.Id);
            return await _repository
                .ToQueryable(query)
                .Select(p => new AmenityListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    IconUri = p.IconUri
                })
                .ToListAsync();
        }
    }
}
