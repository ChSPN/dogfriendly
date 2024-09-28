using DogFriendly.Application.Queries.Users;
using DogFriendly.Domain.Enums;
using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using DogFriendly.Infrastructure.Context;
using DogFriendly.Infrastructure.Repositories;
using EntityFrameworkCore.Repository;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

// Create a new app builder.
var builder = WebApplication.CreateBuilder(args);

// Add the configurations.
builder.Configuration.AddUserSecrets<Program>();
builder.Services
    .Configure<FileStorageOption>(builder.Configuration.GetSection("FileStorage"));

// Add the Firebase JWT Bearer authentication.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{builder.Configuration["Firebase:ProjectId"]}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{builder.Configuration["Firebase:ProjectId"]}",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Firebase:ProjectId"],
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DogFriendlyContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DogFriendlyContext"));
});
builder.Services.AddScoped<DbContext>(s => s.GetRequiredService<DogFriendlyContext>());
builder.Services.AddUnitOfWork<DogFriendlyContext>();
builder.Services.AddUnitOfWork();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IFileStorageRepository, CloudflareStorageRepository>();
builder.Services.AddMediatR(o =>
{
    o.Lifetime = ServiceLifetime.Scoped;
    o.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    o.RegisterServicesFromAssembly(typeof(NewsType).Assembly);
    o.RegisterServicesFromAssembly(typeof(UserEmailExistQueryHandler).Assembly);
});

// Add configuring Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add XML comments to Swagger.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Add JWT Bearer authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Build the app.
var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DogFriendlyContext>();
    await dbContext.Database.MigrateAsync();
}

// Add middleware to the pipeline.
app.UseCors(o =>
{
    o.AllowAnyHeader();
    o.AllowAnyMethod();
    o.AllowAnyOrigin();
});
app.UseStaticFiles();
app.UseSwagger(); 
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogFriendly API V1");
    c.RoutePrefix = string.Empty;
    c.InjectStylesheet("https://www.gstatic.com/firebasejs/ui/6.1.0/firebase-ui-auth.css");
    c.InjectStylesheet("/FireBaseAuth.css");
    c.InjectJavascript("https://code.jquery.com/jquery-3.7.1.min.js");
    c.InjectJavascript("https://www.gstatic.com/firebasejs/10.0.0/firebase-app-compat.js");
    c.InjectJavascript("https://www.gstatic.com/firebasejs/10.0.0/firebase-auth-compat.js");
    c.InjectJavascript("https://www.gstatic.com/firebasejs/ui/6.1.0/firebase-ui-auth.js");
    c.InjectJavascript("/FireBaseAuth.js");
});
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Map controllers.
app.MapControllers();

// Run the app.
app.Run();
