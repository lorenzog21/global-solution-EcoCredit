using Xunit;
using Moq;
using FluentAssertions;
using EcoCredit.API.DTOs.Credit;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;
using EcoCredit.API.Services;

namespace EcoCredit.Tests.Services;

public class CreditServiceTests {
    private readonly Mock<ICreditRepository> _repoMock = new();

    private CreditService CreateService() => new CreditService(_repoMock.Object);

    [Fact]
    public async Task GetBalanceAsync_ShouldSumAvailableCredits() {
        var companyId = "comp-001";
        _repoMock.Setup(r => r.GetByCompanyAsync(companyId)).ReturnsAsync(new List<CarbonCredit> {
            new() { CreditId = "c1", CompanyId = companyId, QuantityTco2 = 2000m, Status = "AVAILABLE", CreditType = "CBIO", ExpiryDate = DateTime.UtcNow.AddYears(1) },
            new() { CreditId = "c2", CompanyId = companyId, QuantityTco2 = 1500m, Status = "AVAILABLE", CreditType = "REDD", ExpiryDate = DateTime.UtcNow.AddYears(1) },
            new() { CreditId = "c3", CompanyId = companyId, QuantityTco2 = 800m,  Status = "USED",      CreditType = "VCM",  ExpiryDate = DateTime.UtcNow.AddYears(1) },
        });

        var service = CreateService();
        var result  = await service.GetBalanceAsync(companyId);

        result.TotalAvailableTco2.Should().Be(3500m);
        result.TotalUsedTco2.Should().Be(800m);
        result.AvailableCount.Should().Be(2);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetStatusAvailable() {
        var companyId = "comp-001";
        var dto = new CreditCreateDto {
            QuantityTco2 = 500m,
            CreditType   = "GOLD",
            ExpiryDate   = DateTime.UtcNow.AddYears(2)
        };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CarbonCredit>())).Returns(Task.CompletedTask);

        var service = CreateService();
        var result  = await service.CreateAsync(companyId, dto);

        result.Status.Should().Be("AVAILABLE");
        result.CompanyId.Should().Be(companyId);
        result.QuantityTco2.Should().Be(500m);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<CarbonCredit>()), Times.Once);
    }

    [Fact]
    public async Task UseCreditAsync_WhenAvailable_ShouldMarkUsed() {
        var credit = new CarbonCredit {
            CreditId     = "c1",
            CompanyId    = "comp-001",
            QuantityTco2 = 1000m,
            Status       = "AVAILABLE",
            CreditType   = "CBIO",
            ExpiryDate   = DateTime.UtcNow.AddYears(1)
        };
        _repoMock.Setup(r => r.GetByIdAsync("c1")).ReturnsAsync(credit);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<CarbonCredit>())).Returns(Task.CompletedTask);

        var service = CreateService();
        var result  = await service.UseCreditAsync("c1");

        result.Should().NotBeNull();
        result!.Status.Should().Be("USED");
        _repoMock.Verify(r => r.UpdateAsync(It.Is<CarbonCredit>(c => c.Status == "USED")), Times.Once);
    }

    [Fact]
    public async Task UseCreditAsync_WhenAlreadyUsed_ShouldReturnNull() {
        var credit = new CarbonCredit {
            CreditId = "c2",
            Status   = "USED",
            CreditType = "VCM",
            ExpiryDate = DateTime.UtcNow.AddYears(1)
        };
        _repoMock.Setup(r => r.GetByIdAsync("c2")).ReturnsAsync(credit);

        var service = CreateService();
        var result  = await service.UseCreditAsync("c2");

        result.Should().BeNull();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<CarbonCredit>()), Times.Never);
    }

    [Fact]
    public async Task UseCreditAsync_WhenNotFound_ShouldReturnNull() {
        _repoMock.Setup(r => r.GetByIdAsync("missing")).ReturnsAsync((CarbonCredit?)null);

        var service = CreateService();
        var result  = await service.UseCreditAsync("missing");

        result.Should().BeNull();
    }
}
