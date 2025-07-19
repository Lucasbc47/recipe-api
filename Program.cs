using Recipe.API.Application.Interfaces;
using Recipe.API.Application.Services;
using Recipe.API.Infrastructure.Data;
using Recipe.API.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Recipe.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});


// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register services
// Factory as project grows..
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<RecipeService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Recipe API",
        Version = "v1",
        Description = "A Web API for managing recipes"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add Health Checks
builder.Services.AddHealthChecks();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

// Global exception handling
app.UseExceptionHandler("/error");

// Only use HTTPS redirection if HTTPS is configured
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Health Check
app.MapHealthChecks("/health");
app.Run();
