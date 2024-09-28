using DogFriendly.Domain.Resources;
using DogFriendly.Web.Client.Services;
using DogFriendly.Web.Components;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add configurations.
builder.Configuration.AddUserSecrets<Program>();

// Configurations for the API.
var apiConfig = new Action<HttpClient>((c) =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add services to the container.
builder.Services.AddHttpClient("DogFriendly", apiConfig);
builder.Services.AddScoped<AuthenticationService>();
builder.Services
    .AddRefitClient<IUserResource>()
    .ConfigureHttpClient(apiConfig);
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

// Run the application.
app.Run();
