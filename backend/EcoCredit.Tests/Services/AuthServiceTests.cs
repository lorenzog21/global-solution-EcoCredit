using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using EcoCredit.API.DTOs.Auth;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;
using EcoCredit.API.Services;

namespace EcoCredit.Tests.Services;

public class AuthServiceTests {
    private readonly Mock<IUserRepository> _userRepoMock = new();

    private AuthService CreateService() {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> {
                ["Jwt:Key"]             = "ecocredit-super-secret-key-for-tests-2026",
                ["Jwt:Issuer"]          = "EcoCredit.API",
                ["Jwt:Audience"]        = "EcoCredit.Client",
                ["Jwt:ExpirationHours"] = "8"
            })
            .Build();
        return new AuthService(_userRepoMock.Object, config);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken() {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Senha@123", workFactor: 4);
        _userRepoMock.Setup(r => r.FindByEmailAsync("admin@petro.com"))
            .ReturnsAsync(new User {
                UserId       = "usr-001",
                Email        = "admin@petro.com",
                CompanyId    = "comp-001",
                Role         = "ADMIN",
                PasswordHash = passwordHash
            });

        var service = CreateService();
        var result  = await service.LoginAsync(new LoginRequestDto {
            Email    = "admin@petro.com",
            Password = "Senha@123"
        });

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Role.Should().Be("ADMIN");
        result.UserId.Should().Be("usr-001");
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ShouldReturnNull() {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPass", workFactor: 4);
        _userRepoMock.Setup(r => r.FindByEmailAsync("user@petro.com"))
            .ReturnsAsync(new User {
                UserId       = "usr-002",
                Email        = "user@petro.com",
                CompanyId    = "comp-001",
                Role         = "ANALYST",
                PasswordHash = passwordHash
            });

        var service = CreateService();
        var result  = await service.LoginAsync(new LoginRequestDto {
            Email    = "user@petro.com",
            Password = "WrongPass"
        });

        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_WithUnknownEmail_ShouldReturnNull() {
        _userRepoMock.Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var service = CreateService();
        var result  = await service.LoginAsync(new LoginRequestDto {
            Email    = "ghost@nowhere.com",
            Password = "AnyPass"
        });

        result.Should().BeNull();
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_ShouldReturnTrue() {
        _userRepoMock.Setup(r => r.FindByEmailAsync("new@petro.com"))
            .ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();
        var result  = await service.RegisterAsync(new RegisterUserDto {
            Email     = "new@petro.com",
            Password  = "NewPass@123",
            CompanyId = "comp-001",
            Role      = "ANALYST"
        });

        result.Should().BeTrue();
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ShouldReturnFalse() {
        _userRepoMock.Setup(r => r.FindByEmailAsync("existing@petro.com"))
            .ReturnsAsync(new User { Email = "existing@petro.com" });

        var service = CreateService();
        var result  = await service.RegisterAsync(new RegisterUserDto {
            Email    = "existing@petro.com",
            Password = "Pass@123",
            CompanyId = "comp-001"
        });

        result.Should().BeFalse();
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }
}
