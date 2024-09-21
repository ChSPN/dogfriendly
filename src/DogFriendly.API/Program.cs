using DogFriendly.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add configurations.
builder.Configuration.AddUserSecrets<Program>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DogFriendlyContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DogFriendlyContext"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DogFriendlyContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseSwagger(); 
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogFriendly API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors(o =>
{
    o.AllowAnyHeader();
    o.AllowAnyMethod();
    o.AllowAnyOrigin();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
