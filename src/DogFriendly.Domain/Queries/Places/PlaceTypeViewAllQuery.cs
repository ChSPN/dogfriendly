using DogFriendly.Domain.ViewModels.Places;
using MediatR;

namespace DogFriendly.Domain.Queries.Places
{
    /// <summary>
    /// Load all place types for view.
    /// </summary>
    /// <seealso cref="MediatR.IRequest&lt;DogFriendly.Domain.ViewModels.Places.PlaceTypeViewModel&gt;" />
    public class PlaceTypeViewAllQuery : IRequest<List<PlaceTypeViewModel>>
    {
    }
}
