using DogFriendly.Domain.ViewModels.Amenities;
using MediatR;

namespace DogFriendly.Domain.Queries.Amenities
{
    /// <summary>
    /// Query to get all amenities.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;System.Collections.Generic.List&lt;DogFriendly.Domain.ViewModels.Amenities.AmenityListViewModel&gt;&gt;" />
    public class AmenityViewAllQuery : IRequest<List<AmenityListViewModel>>
    {
    }
}
