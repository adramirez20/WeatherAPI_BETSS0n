using Moq;
using WeatherAPIIntegration.Application.Commands;
using WeatherAPIIntegration.Application.Exeptions;
using WeatherAPIIntegration.Application.Services;
using WeatherAPIIntegration.Infrastructure.Repositories;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICountryService> _countryServiceMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _countryServiceMock = new Mock<ICountryService>();
        _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _countryServiceMock.Object);
    }

    [Fact]
    public async Task Handle_InvalidPhoneNumber_ThrowsInvalidPhoneNumberFormatException()
    {
        var command = new RegisterUserCommand
        {
            PhoneNumber = "1234567890",
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "FirstName@example.com",
            Password = "Password123",
            Address = "123 Main St",
            Birthdate = DateTime.Now.AddYears(-25),
            LivingCountry = "Malta",
            CitizenCountry = "Mdina"
        };
        await Assert.ThrowsAsync<InvalidPhoneNumberFormatException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UsernameAlreadyExists_ThrowsUsernameAlreadyExistsException()
    {
        var command = new RegisterUserCommand
        {
            PhoneNumber = "+11234567890",
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "FirstName@example.com",
            Password = "Password123",
            Address = "123 Main St",
            Birthdate = DateTime.Now.AddYears(-25),
            LivingCountry = "Colombia",
            CitizenCountry = "Bogota"
        };

        _userRepositoryMock.Setup(repo => repo.UsernameExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<UsernameAlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidCountryCode_ThrowsInvalidCountryCodeException()
    {
        var command = new RegisterUserCommand
        {
            PhoneNumber = "+11234567890",
            FirstName = "FirstName" + new Random().Next(10000, 99999),
            LastName = "LastName" + new Random().Next(10000, 99999),
            Email = "FirstName@example.com",
            Password = "Password123",
            Address = "123 Main St",
            Birthdate = DateTime.Now.AddYears(-25),
            LivingCountry = "InvalidCountry",
            CitizenCountry = "cityTest"
        };

        _countryServiceMock.Setup(service => service.GetCountryCode(It.IsAny<string>())).ReturnsAsync(string.Empty);

        await Assert.ThrowsAsync<InvalidCountryCodeException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsUserDto()
    {
        var command = new RegisterUserCommand
        {
            PhoneNumber = "+11234567890",
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "FirstName@example.com",
            Password = "Password123",
            Address = "123 Main St",
            Birthdate = DateTime.Now.AddYears(-25),
            LivingCountry = "Malta",
            CitizenCountry = "Mdina"
        };

        _countryServiceMock.Setup(service => service.GetCountryCode(It.IsAny<string>())).ReturnsAsync("US");
        _userRepositoryMock.Setup(repo => repo.UsernameExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(command.Email, result.Email);
    }
}
