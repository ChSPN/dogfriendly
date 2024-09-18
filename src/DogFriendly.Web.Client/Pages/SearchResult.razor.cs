using Microsoft.AspNetCore.Components;

namespace DogFriendly.Web.Client.Pages
{
    public partial class SearchResult : ComponentBase
    {
        [Parameter]
        public int? placeId { get; set; }

        [Parameter]
        public int? placeTypeId { get; set; }

    }
}
