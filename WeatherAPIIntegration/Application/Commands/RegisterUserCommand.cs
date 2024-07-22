﻿using MediatR;
using WeatherAPIIntegration.DTOs;

namespace WeatherAPIIntegration.Application.Commands
{
    public class RegisterUserCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhoneNumber { get; set; }
        public string LivingCountry { get; set; }
        public string CitizenCountry { get; set; }
    }
}
