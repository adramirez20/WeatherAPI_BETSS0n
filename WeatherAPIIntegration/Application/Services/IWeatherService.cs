using WeatherAPIIntegration.DTOs;

namespace WeatherAPIIntegration.Application.Services
{
    public interface IWeatherService
    {
        public Task<WeatherDto> GetWeatherAsync(string location);
    }
}
