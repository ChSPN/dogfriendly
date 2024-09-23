﻿using Microsoft.JSInterop;

namespace DogFriendly.Web.Client.Services
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    public class AuthenticationService
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        public AuthenticationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Determines whether is user authenticated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if is user authenticated; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsUserAuthenticated()
        {
            return await _jsRuntime.InvokeAsync<bool>("isFirebaseUserAuth");
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        public async Task SignOut()
        {
            await _jsRuntime.InvokeVoidAsync("logoutFirebaseAuth");
        }
    }
}