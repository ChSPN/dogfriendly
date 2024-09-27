using DogFriendly.Web.Client.Services;
using DogFriendly.Web.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add configurations.
builder.Configuration.AddUserSecrets<Program>();


// Add services to the container.
builder.Services.AddHttpClient("DogFriendly", async (s, c) =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
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
