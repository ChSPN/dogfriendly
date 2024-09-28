using System.ComponentModel.DataAnnotations;

namespace DogFriendly.Domain.ViewModels.Users
{
    /// <summary>
    /// Model for profile a user.
    /// </summary>
    public class UserProfilViewModel
    {
        /// <summary>
        /// Gets or sets the content of the picture.
        /// </summary>
        /// <value>
        /// The content of the picture.
        /// </value>
        public byte[]? PictureContent { get; set; }

        /// <summary>
        /// Gets or sets the name of the picture.
        /// </summary>
        /// <value>
        /// The name of the picture.
        /// </value>
        public string? PictureName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required(ErrorMessage = "Le pseudo est requis.")]
        [MaxLength(20, ErrorMessage = "Le pseudo peut contenir au maximum 20 caractères.")]
        [MinLength(3, ErrorMessage = "Le pseudo doit contenir au minimum 3 caractères.")]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the user picture.
        /// </summary>
        /// <value>
        /// The user picture.
        /// </value>
        public string? UserPicture { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string? UserEmail { get; set; }
    }
}
