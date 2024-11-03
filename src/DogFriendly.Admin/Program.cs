using DogFriendly.Admin.Components;
using DogFriendly.Admin.Services;
using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using DogFriendly.Domain.Resources;
using DogFriendly.Infrastructure.Context;
using DogFriendly.Infrastructure.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using Refit;
using Microsoft.AspNetCore.Identity;
using Radzen;

// Create a new app builder.
var builder = WebApplication.CreateBuilder(args);

// Add the configurations.
builder.Configuration
    .AddUserSecrets<Program>();
builder.Services
    .Configure<FileStorageOption>(builder.Configuration.GetSection("FileStorage"));

// Configure Firebase.
var firebaseSection = builder.Configuration.GetSection("Firebase");
var firebaseJson = JsonConvert.SerializeObject(firebaseSection
    .GetChildren()
    .ToDictionary(x => x.Key, x => x.Value))
    .Replace("\\\\n", "\n");
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(firebaseJson)
});
builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);

// Add services to the container.
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddDbContext<DogFriendlyContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DogFriendlyContext"));
});
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.TicketDataFormat = new TicketDataFormat(
            new EphemeralDataProtectionProvider()
                .CreateProtector("AuthenticationCookie"));
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>((s) => s.GetRequiredService<AuthenticationProvider>());
builder.Services.AddScoped<DogFriendly.Admin.Services.AuthenticationService>();
builder.Services.AddScoped<DbContext>(s => s.GetRequiredService<DogFriendlyContext>());
builder.Services.AddScoped<IFileStorageRepository, CloudflareStorageRepository>();
builder.Services.AddScoped<IDesignTimeDbContextFactory<DogFriendlyContext>, DogFriendlyContextFactory>();
builder.Services.AddRefitClient<INominatimResource>()
    .ConfigureHttpClient((c) =>
    {
        c.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
        c.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
    });
builder.Services.AddControllersWithViews();
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Run the app.
app.Run();
