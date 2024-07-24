using MediatR;
using WeatherAPIIntegration.Domain.Services;
using WeatherAPIIntegration.DTOs;
using WeatherAPIIntegration.Infrastructure.Repositories;

namespace WeatherAPIIntegration.Application.Queries
{
    public class GetWeatherQueryHandler : IRequestHandler<GetWeatherQuery, WeatherDto>
    {
        private readonly IWeatherService _weatherService;
        private readonly ICacheService _cacheService;
        private readonly IUserRepository _userRepository;

        public GetWeatherQueryHandler(IWeatherService weatherService, ICacheService cacheService, IUserRepository userRepository)
        {
            _weatherService = weatherService;
            _cacheService = cacheService;
            _userRepository = userRepository;
        }

        public async Task<WeatherDto> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var cacheKey = $"weather_{user.LivingCountry}";
            var cachedWeather = await _cacheService.GetAsync<WeatherDto>(cacheKey);
            if (cachedWeather != null)
            {
                return cachedWeather;
            }

            var weatherData = await _weatherService.GetWeatherAsync(user.LivingCountry);
            await _cacheService.SetAsync(cacheKey, weatherData, TimeSpan.FromMinutes(30));

            return weatherData;
        }
    }
}
