using DogFriendly.Web.Client.Services;
using DogFriendly.Web.Components;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add configurations.
builder.Configuration.AddUserSecrets<Program>();


// Add services to the container.
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddHttpClient("DogFriendly", async (c) =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    if (AuthenticationService.JwtToken is JwtSecurityToken token)
    {
        c.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RawData}");
    }
});
builder.Services
    .AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DogFriendly.Web.Client._Imports).Assembly);

app.Run();
