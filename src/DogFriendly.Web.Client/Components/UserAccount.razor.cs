using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DogFriendly.Web.Client.Components
{
    /// <summary>
    /// User account componant.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class UserAccount : ComponentBase
    {
        /// <summary>
        /// The maximum file size.
        /// </summary>
        private const long MaxFileSize = 5 * 1024 * 1024;

        /// <summary>
        /// Gets or sets the button submit.
        /// </summary>
        /// <value>
        /// The button submit.
        /// </value>
        [Parameter]
        public string ButtonSubmit { get; set; } = "";

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [Parameter]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the on submit.
        /// </summary>
        /// <value>
        /// The on submit.
        /// </value>
        [Parameter]
        public EventCallback<UserProfilViewModel?> OnSubmit { get; set; }

        /// <summary>
        /// Gets or sets the register user model.
        /// </summary>
        /// <value>
        /// The register user model.
        /// </value>
        [Parameter]
        public UserProfilViewModel? ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the user picture.
        /// </summary>
        /// <value>
        /// The user picture.
        /// </value>
        protected string? UserPicture { get; set; }

        /// <summary>
        /// Handles the file selected.
        /// </summary>
        /// <param name="e">The <see cref="InputFileChangeEventArgs"/> instance containing the event data.</param>
        protected async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file.Size > MaxFileSize)
            {
                ErrorMessage = "La taille du fichier dépasse la limite de 5 Mo.";
                return;
            }

            ErrorMessage = string.Empty;
            using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            ViewModel.PictureContent = memoryStream.ToArray();
            ViewModel.PictureName = file.Name;
            ViewModel.UserPicture = $"data:{file.ContentType};base64,{Convert.ToBase64String(ViewModel.PictureContent)}";

            StateHasChanged();
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (ViewModel == null)
            {
                ViewModel = new UserProfilViewModel();
            }

            if (AuthenticationService.JwtToken != null)
            {
                UserPicture = AuthenticationService.JwtToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Picture)?
                    .Value;
            }
        }

        /// <summary>
        /// Resets the picture.
        /// </summary>
        protected void ResetPicture()
        {
            if (AuthenticationService.JwtToken != null)
            {
                ViewModel.PictureContent = null;
                ViewModel.PictureName = null;
                ViewModel.UserPicture = AuthenticationService.JwtToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Picture)?
                    .Value;
            }
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        protected async Task Submit()
        {
            await OnSubmit.InvokeAsync(ViewModel);
        }
    }
}
