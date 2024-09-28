namespace DogFriendly.Domain.ViewModels
{
    /// <summary>
    /// Model for resource response.
    /// </summary>
    public class ResponseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this resource is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this resource is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string? Message { get; set; }
    }
}
