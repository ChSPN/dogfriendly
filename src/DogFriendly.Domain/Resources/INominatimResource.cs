using Refit;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Resource for Nominatim API.
    /// </summary>
    public interface INominatimResource
    {
        /// <summary>
        /// Searches the specified search query.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="countries">The countries.</param>
        /// <returns></returns>
        [Get("/search?format=json")]
        Task<List<NominatimResponse>> Search(
            [AliasAs("q")] string searchQuery,
            [AliasAs("limit")] int limit = 10,
            [AliasAs("countrycodes")] params string[] countries);
    }
}
