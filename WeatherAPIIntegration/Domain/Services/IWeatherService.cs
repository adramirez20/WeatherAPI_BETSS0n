using WeatherAPIIntegration.DTOs;

namespace WeatherAPIIntegration.Domain.Services
{
    public interface IWeatherService
    {
       public Task<WeatherDto> GetWeatherAsync(string location);
    }
}
