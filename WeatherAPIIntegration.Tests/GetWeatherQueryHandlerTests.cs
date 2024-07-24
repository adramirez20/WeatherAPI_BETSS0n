using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WeatherAPIIntegration.Application.Queries;
using WeatherAPIIntegration.Application.Services;
using WeatherAPIIntegration.DTOs;
using WeatherAPIIntegration.Infrastructure.Repositories;
using WeatherAPIIntegration.Domain.Entities;

public class GetWeatherQueryHandlerTests
{
    private readonly Mock<IWeatherService> _mockWeatherService;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetWeatherQueryHandler _handler;

    public GetWeatherQueryHandlerTests()
    {
        _mockWeatherService = new Mock<IWeatherService>();
        _mockCacheService = new Mock<ICacheService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetWeatherQueryHandler(_mockWeatherService.Object, _mockCacheService.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsException()
    {

        var query = new GetWeatherQuery("nonexistentUser");
        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(query.Username))
            .ReturnsAsync((User)null);


        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Equal("User not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_CachedWeatherExists_ReturnsCachedWeather()
    {

        var query = new GetWeatherQuery("existingUser");
        var user = new User { Username = query.Username, LivingCountry = "Country" };
        var cachedWeather = new WeatherDto { Location = "Country", Temperature = "20", Condition = "Sunny" };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(query.Username))
            .ReturnsAsync(user);

        _mockCacheService.Setup(cache => cache.GetAsync<WeatherDto>($"weather_{user.LivingCountry}"))
            .ReturnsAsync(cachedWeather);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(cachedWeather, result);
        _mockWeatherService.Verify(service => service.GetWeatherAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CachedWeatherNotExists_FetchesWeatherAndCachesIt()
    {
        var query = new GetWeatherQuery("existingUser");
        var user = new User { Username = query.Username, LivingCountry = "Country" };
        var weatherData = new WeatherDto { Location = "Country", Temperature = "25", Condition = "Cloudy" };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(query.Username))
            .ReturnsAsync(user);

        _mockCacheService.Setup(cache => cache.GetAsync<WeatherDto>($"weather_{user.LivingCountry}"))
            .ReturnsAsync((WeatherDto)null);

        _mockWeatherService.Setup(service => service.GetWeatherAsync(user.LivingCountry))
            .ReturnsAsync(weatherData);

        _mockCacheService.Setup(cache => cache.SetAsync($"weather_{user.LivingCountry}", weatherData, It.IsAny<TimeSpan>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(weatherData, result);
        _mockWeatherService.Verify(service => service.GetWeatherAsync(user.LivingCountry), Times.Once);
        _mockCacheService.Verify(cache => cache.SetAsync($"weather_{user.LivingCountry}", weatherData, It.IsAny<TimeSpan>()), Times.Once);
    }
}
