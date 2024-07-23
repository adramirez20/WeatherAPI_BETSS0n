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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(new { Token = token });
        }

        [HttpGet("weather/{username}")]
        public async Task<IActionResult> GetWeather(string username)
        {
            var result = await _mediator.Send(username);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
