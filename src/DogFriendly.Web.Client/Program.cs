using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using DogFriendly.Domain.Resources;
using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Refit;
using ServiceCollectionExtensions;
using System.IdentityModel.Tokens.Jwt;

// Configurations appsetings.
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

// Create the builder.
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add options to the builder.
builder.Configuration.AddInMemoryCollection();

// Add Blazorise and Bootstrap.
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// Add services to the container.
builder.Services.AddGeolocationService();
builder.Services.AddNominatimGeocoderService();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddSingleton<SearchService>();
builder.Services.AddScoped<UnauthorizedService>();
builder.Services.AddHttpClient("DogFriendly", apiConfig);
builder.Services.AddRefitClient<INominatimResource>()
    .ConfigureHttpClient((c) =>
    {
        c.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
    });
builder.Services.AddRefitClient<IPlaceTypeResource>()
    .ConfigureHttpClient(apiConfig)
    .AddHttpMessageHandler<UnauthorizedService>();
builder.Services.AddRefitClient<IUserResource>()
    .ConfigureHttpClient(apiConfig)
    .AddHttpMessageHandler<UnauthorizedService>();
builder.Services.AddRefitClient<IPlaceResource>()
    .ConfigureHttpClient(apiConfig)
    .AddHttpMessageHandler<UnauthorizedService>();
builder.Services.AddRefitClient<IAmenityResource>()
    .ConfigureHttpClient(apiConfig)
    .AddHttpMessageHandler<UnauthorizedService>();

// Build the app.
var app = builder.Build();

// Get the appsettings.
var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
apiUrl = await jsRuntime.InvokeAsync<string>("getApiUrl");
app.Configuration["PhotoUrl"] = await jsRuntime.InvokeAsync<string>("getPhotoUrl");

// Update the Firebase Auth.
await jsRuntime.InvokeVoidAsync("updateFirebaseAuth");

// Run the application.
await app.RunAsync();
