using DogFriendly.Domain.Resources;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Refit;
using ServiceCollectionExtensions;
using System.IdentityModel.Tokens.Jwt;

// Configurations for the API.
string apiUrl = string.Empty;
var apiConfig = new Action<HttpClient>((c) =>
{
    c.BaseAddress = new Uri(apiUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    if (AuthenticationService.JwtToken is JwtSecurityToken token)
    {
        c.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RawData}");
    }
});

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add services to the container.
builder.Services.AddGeolocationService();
builder.Services.AddNominatimGeocoderService();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddHttpClient("DogFriendly", apiConfig);
builder.Services.AddRefitClient<INominatimResource>()
    .ConfigureHttpClient((c) =>
    {
        c.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
    });
builder.Services.AddRefitClient<IPlaceTypeResource>().ConfigureHttpClient(apiConfig);
builder.Services.AddRefitClient<IUserResource>().ConfigureHttpClient(apiConfig);

var app = builder.Build();

// Get the API URL and update the Firebase Auth.
var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
apiUrl = await jsRuntime.InvokeAsync<string>("getApiUrl");
await jsRuntime.InvokeVoidAsync("updateFirebaseAuth");

// Run the application.
await app.RunAsync();
