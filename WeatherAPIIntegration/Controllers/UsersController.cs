using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherAPIIntegration.Application.Commands;
using WeatherAPIIntegration.Application.Queries;

namespace WeatherAPIIntegration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var username = await _mediator.Send(command);
            return Ok(new { Username = username });
        }

        [HttpGet("weather/{username}")]
        public async Task<IActionResult> GetWeather(string username)
        {
            var query = new GetWeatherQuery(username);
            var weather = await _mediator.Send(query);
            return Ok(weather);
        }
    }
}
