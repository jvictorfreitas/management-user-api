using Infrastructure.database;
using management.user;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHandlers();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApiDocument(options =>
{
    options.Title = "Management User API";
    options.Version = "v1";
    options.Description = "Documentação da API";
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.UseMiddleware<JsonApiExceptionMiddleware>();

app.ConfigureRoutes();

app.UseOpenApi(); // gera /swagger/v1/swagger.json

app.UseSwaggerUi(options =>
{
    options.Path = "/docs";
    options.DocumentPath = "/swagger/v1/swagger.json";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
