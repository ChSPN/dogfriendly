using Microsoft.AspNetCore.Components;

namespace DogFriendly.Web.Client.Components
{
    /// <summary>
    /// Rating component.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Rating : ComponentBase
    {
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [Parameter]
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the star white.
        /// </summary>
        /// <value>
        /// The star white.
        /// </value>
        protected int StarWhite { get; set; }

        /// <summary>
        /// Gets or sets the star dark.
        /// </summary>
        /// <value>
        /// The star dark.
        /// </value>
        protected int StarDark { get; set; }

        /// <inheritdoc />
        override protected void OnInitialized()
        {
            StarWhite = 5 - (int)Math.Ceiling(Value);
            StarDark = (int)Math.Ceiling(Value);
        }
    }
}
