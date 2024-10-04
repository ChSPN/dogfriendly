namespace DogFriendly.Domain.ViewModels.Places
{
    /// <summary>
    /// View model for place.
    /// </summary>
    public class PlaceViewModel : PlaceListViewModel
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public required string Address { get; set; }

        /// <summary>
        /// Gets or sets the amenities.
        /// </summary>
        /// <value>
        /// The amenities.
        /// </value>
        public List<KeyValuePair<string, string?>>? Amenities { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public required string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public required string Country { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the opening hours.
        /// </summary>
        /// <value>
        /// The opening hours.
        /// </value>
        public string? OpeningHours { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public required string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the reviews count.
        /// </summary>
        /// <value>
        /// The reviews count.
        /// </value>
        public int ReviewsCount { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string? Website { get; set; }
    }
}
