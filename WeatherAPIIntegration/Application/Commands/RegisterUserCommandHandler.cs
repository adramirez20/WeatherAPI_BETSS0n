using MediatR;
using System.Text.RegularExpressions;
using WeatherAPIIntegration.Application.Exeptions;
using WeatherAPIIntegration.Application.Services;
using WeatherAPIIntegration.Domain.Entities;
using WeatherAPIIntegration.DTOs;
using WeatherAPIIntegration.Infrastructure.Repositories;

namespace WeatherAPIIntegration.Application.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryService _countryService;
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+\d{1,3}\d{4,14}$");
        public RegisterUserCommandHandler(IUserRepository userRepository, ICountryService countryService)
        {
            _userRepository = userRepository;
            _countryService = countryService;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!PhoneNumberRegex.IsMatch(request.PhoneNumber))
            {
                throw new InvalidPhoneNumberFormatException("Invalid phone number format. It must start with a '+' followed by the country code.");
            }

            var username = UsernameGenerator.Generate(request.FirstName, request.LastName);
            if (await _userRepository.UsernameExistsAsync(username))
            {
                throw new UsernameAlreadyExistsException("Username already exists.");
            }

            var countryCode = await _countryService.GetCountryCode(request.LivingCountry);
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new InvalidCountryCodeException("Invalid country code.");
            }


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
                CitizenCountry = request.CitizenCountry,
                CountryCode = countryCode
            };

            await _userRepository.AddAsync(user);

            return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };
        }
    }
}