using System.Text.Json.Serialization;

namespace DogFriendly.Domain.Resources
{
    /// <summary>
    /// Response from Nominatim API.
    /// </summary>
    public class NominatimResponse
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [JsonPropertyName("lat")]
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        [JsonPropertyName("lon")]
        public string Longitude { get; set; }
    }
}
