using Microsoft.EntityFrameworkCore;
using WeatherAPIIntegration.Infrastructure.Persistence;
using WeatherAPIIntegration.Application.Commands;
using WeatherAPIIntegration.Application.Queries;
using WeatherAPIIntegration.Infrastructure.Repositories;
using MediatR;
using Microsoft.OpenApi.Models;
using WeatherAPIIntegration.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MediatR
builder.Services.AddMediatR(typeof(RegisterUserCommandHandler).Assembly, typeof(GetWeatherQueryHandler).Assembly);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPIIntegration", Version = "v1" });
});

// Register repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpClient<ICountryService, CountryService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPIIntegration v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
