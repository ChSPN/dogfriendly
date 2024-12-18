﻿namespace DogFriendly.Domain.Models
{
    /// <summary>
    /// Base model.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public DateTimeOffset CreatedAt { get; protected set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; protected set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>
        /// The updated at.
        /// </value>
        public DateTimeOffset? UpdatedAt { get; protected set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>
        /// The updated by.
        /// </value>
        public string? UpdatedBy { get; protected set; }
    }
}
