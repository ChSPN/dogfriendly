using DogFriendly.Domain.Resources;
using DogFriendly.Domain.ViewModels.Users;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DogFriendly.Web.Client.Pages
{
    /// <summary>
    /// Register page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Register : ComponentBase
    {
        /// <summary>
        /// The maximum file size.
        /// </summary>
        private const long MaxFileSize = 5 * 1024 * 1024;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        protected string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>
        /// The navigation manager.
        /// </value>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the user resource.
        /// </summary>
        /// <value>
        /// The user resource.
        /// </value>
        [Inject]
        protected IUserResource UserResource { get; set; }

        /// <summary>
        /// Gets or sets the register user model.
        /// </summary>
        /// <value>
        /// The register user model.
        /// </value>
        protected UserProfilViewModel ViewModel { get; set; }

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (AuthenticationService.JwtToken == null
                    || await UserResource.IsExist())
                {
                    NavigationManager.NavigateTo("/login");
                    return;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            ViewModel = new UserProfilViewModel();
            if (AuthenticationService.JwtToken != null)
            {
                ViewModel.UserName = AuthenticationService.JwtToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?
                    .Value;
                ViewModel.UserPicture = AuthenticationService.JwtToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Picture)?
                    .Value;
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        protected async Task RegisterUser()
        {
            if (ViewModel.PictureContent != null)
            {
                ViewModel.UserPicture = null;
            }

            var response = await UserResource.Register(ViewModel);
            if (response.IsSuccess)
            {
                NavigationManager.NavigateTo("/login");
                return;
            }
            else
            {
                ErrorMessage = response.Message;
                StateHasChanged();
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
    }
}
