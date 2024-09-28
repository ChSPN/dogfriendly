namespace DogFriendly.Domain.Options
{
    /// <summary>
    /// Options for file storage.
    /// </summary>
    public class FileStorageOption
    {
        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        /// <value>
        /// The access key.
        /// </value>
        public required string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public required string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the amenities URI.
        /// </summary>
        /// <value>
        /// The amenities URI.
        /// </value>
        public required string AmenitiesUri { get; set; }

        /// <summary>
        /// Gets or sets the name of the bucket.
        /// </summary>
        /// <value>
        /// The name of the bucket.
        /// </value>
        public required string BucketName { get; set; }

        /// <summary>
        /// Gets or sets the domain URI.
        /// </summary>
        /// <value>
        /// The domain URI.
        /// </value>
        public required string DomainUri { get; set; }

        /// <summary>
        /// Gets or sets the news URI.
        /// </summary>
        /// <value>
        /// The news URI.
        /// </value>
        public required string NewsUri { get; set; }

        /// <summary>
        /// Gets or sets the places URI.
        /// </summary>
        /// <value>
        /// The places URI.
        /// </value>
        public required string PlacesUri { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        public required string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the users URI.
        /// </summary>
        /// <value>
        /// The users URI.
        /// </value>
        public required string UsersUri { get; set; }
    }
}
