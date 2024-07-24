using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using WeatherAPIIntegration.Application.Commands;
using WeatherAPIIntegration.Application.Exeptions;
using WeatherAPIIntegration.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherAPIIntegration.Domain.Entities;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _handler = new LoginCommandHandler(_userRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task Handle_InvalidUser_ThrowsUserNotFoundException()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

        var command = new LoginCommand { Username = "invaliduser", Password = "invalidpassword" };

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    private bool IsTokenValid(string token)
    {
        var key = Encoding.ASCII.GetBytes("your-secret-key");
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience"
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
