using MediatR;
using WeatherAPIIntegration.DTOs;

namespace WeatherAPIIntegration.Application.Queries
{
    public class GetWeatherQuery : IRequest<WeatherDto>
    {
        public string Username { get; set; }

        public GetWeatherQuery(string username)
        {
            Username = username;
        }
    }
}
