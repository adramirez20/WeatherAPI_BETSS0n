using MediatR;
using WeatherAPIIntegration.Domain.Entities;
using WeatherAPIIntegration.Domain.Services;
using WeatherAPIIntegration.DTOs;
using WeatherAPIIntegration.Infrastructure.Repositories;

namespace WeatherAPIIntegration.Application.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryService _countryService;

        public RegisterUserCommandHandler(IUserRepository userRepository, ICountryService countryService)
        {
            _userRepository = userRepository;
            _countryService = countryService;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var countryCode = await _countryService.GetCountryCode(request.LivingCountry);
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new Exception("Invalid country code.");
            }

            var username = UsernameGenerator.Generate(request.FirstName, request.LastName);
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = username,
                Email = request.Email,
                Password = request.Password,
                Address = request.Address,
                Birthdate = request.Birthdate,
                PhoneNumber = request.PhoneNumber,
                LivingCountry = request.LivingCountry,
                CitizenCountry = request.CitizenCountry
            };

            await _userRepository.AddAsync(user);

            return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };
        }
    }
}