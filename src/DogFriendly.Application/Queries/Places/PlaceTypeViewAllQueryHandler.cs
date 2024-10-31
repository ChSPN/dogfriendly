using DogFriendly.Domain.Entitites;
using DogFriendly.Domain.Queries.Places;
using DogFriendly.Domain.ViewModels.Places;
using EntityFrameworkCore.Repository.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Application.Queries.Places
{
    /// <summary>
    /// Handler for <see cref="PlaceTypeViewAllQuery"/>.
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler&lt;DogFriendly.Domain.Queries.Places.PlaceTypeViewAllQuery, System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Places.PlaceTypeViewModel&gt;&gt;" />
    public class PlaceTypeViewAllQueryHandler : IRequestHandler<PlaceTypeViewAllQuery, List<PlaceTypeViewModel>>
    {
        private readonly IRepository<PlaceTypeEntity> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceTypeViewAllQueryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public PlaceTypeViewAllQueryHandler(IRepository<PlaceTypeEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<List<PlaceTypeViewModel>> Handle(PlaceTypeViewAllQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                .SingleResultQuery()
                .OrderBy(p => p.Id);
            return await _repository
                .ToQueryable(query)
                .Select(p => new PlaceTypeViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    IconUri = p.IconUri,
                    Color = p.Color
                })
                .ToListAsync();
        }
    }
}
