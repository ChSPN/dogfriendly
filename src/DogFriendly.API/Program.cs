using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

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
