using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using WeatherAPIIntegration.Application.Commands;

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
    }
}
