using DogFriendly.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

string apiUrl = string.Empty;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddHttpClient("DogFriendly", (s, c) =>
{
    c.BaseAddress = new Uri(apiUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    if (AuthenticationService.JwtToken is JwtSecurityToken token)
    {
        c.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RawData}");
    }
});

var app = builder.Build();

var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
apiUrl = await jsRuntime.InvokeAsync<string>("getApiUrl");
await jsRuntime.InvokeVoidAsync("updateFirebaseAuth");

await app.RunAsync();
