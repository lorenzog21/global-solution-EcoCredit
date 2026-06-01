using Xunit;
using Moq;
using FluentAssertions;
using EcoCredit.API.DTOs.Emission;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;
using EcoCredit.API.Services;

namespace EcoCredit.Tests.Services;

public class EmissionServiceTests {
    private readonly Mock<IEmissionRepository> _emissionRepoMock = new();
    private readonly Mock<IFacilityRepository> _facilityRepoMock = new();
    private readonly Mock<IAlertRepository> _alertRepoMock = new();

    private EmissionService CreateService()
        => new EmissionService(_emissionRepoMock.Object, _facilityRepoMock.Object, _alertRepoMock.Object);

    [Fact]
    public async Task RegisterAsync_ValidEmission_ShouldCreateRecord() {
        var dto = new EmissionCreateDto {
            FacilityId = "fac-001",
            GasType = "CO2",
            QuantityTco2e = 42.7m,
            Source = "IOT_SENSOR"
        };
        _facilityRepoMock.Setup(r => r.GetByIdAsync("fac-001"))
            .ReturnsAsync(new Facility { FacilityId = "fac-001", EmissionLimitTco2 = 5000m });
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync("fac-001"))
            .ReturnsAsync(42.7m);
        _emissionRepoMock.Setup(r => r.AddAsync(It.IsAny<EmissionRecord>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.RegisterAsync(dto);

        result.Should().NotBeNull();
        result.GasType.Should().Be("CO2");
        result.QuantityTco2e.Should().Be(42.7m);
        _emissionRepoMock.Verify(r => r.AddAsync(It.IsAny<EmissionRecord>()), Times.Once);
    }

    [Fact]
    public async Task CheckThreshold_WhenAbove80Pct_ShouldCreateHighAlert() {
        var facilityId = "fac-001";
        var facility = new Facility {
            FacilityId = facilityId,
            Name = "Refinaria RJ",
            EmissionLimitTco2 = 1000m
        };
        _facilityRepoMock.Setup(r => r.GetByIdAsync(facilityId)).ReturnsAsync(facility);
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync(facilityId)).ReturnsAsync(850m);
        _emissionRepoMock.Setup(r => r.AddAsync(It.IsAny<EmissionRecord>())).Returns(Task.CompletedTask);
        _alertRepoMock.Setup(r => r.AddAsync(It.IsAny<Alert>())).Returns(Task.CompletedTask);

        var service = CreateService();
        var dto = new EmissionCreateDto { FacilityId = facilityId, GasType = "CO2", QuantityTco2e = 10m, Source = "IOT_SENSOR" };

        await service.RegisterAsync(dto);

        _alertRepoMock.Verify(r => r.AddAsync(It.Is<Alert>(a =>
            a.Severity == "HIGH" && a.AlertType == "THRESHOLD")), Times.Once);
    }

    [Fact]
    public async Task GetSummaryAsync_ShouldReturnCorrectComplianceStatus() {
        var companyId = "comp-001";
        var facilities = new List<Facility> {
            new Facility { FacilityId = "fac-001", Name = "Planta A", EmissionLimitTco2 = 1000m }
        };
        _facilityRepoMock.Setup(r => r.GetByCompanyAsync(companyId)).ReturnsAsync(facilities);
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync("fac-001")).ReturnsAsync(1050m);

        var service = CreateService();

        var result = (await service.GetSummaryAsync(companyId)).ToList();

        result.Should().HaveCount(1);
        result[0].ComplianceStatus.Should().Be("CRITICAL");
        result[0].PercentUsed.Should().Be(105m);
    }

    [Fact]
    public async Task RegisterAsync_WhenAbove100Pct_ShouldCreateCriticalAlert() {
        var facilityId = "fac-002";
        var facility = new Facility {
            FacilityId = facilityId,
            Name = "Terminal Santos",
            EmissionLimitTco2 = 3000m
        };
        _facilityRepoMock.Setup(r => r.GetByIdAsync(facilityId)).ReturnsAsync(facility);
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync(facilityId)).ReturnsAsync(3100m);
        _emissionRepoMock.Setup(r => r.AddAsync(It.IsAny<EmissionRecord>())).Returns(Task.CompletedTask);
        _alertRepoMock.Setup(r => r.AddAsync(It.IsAny<Alert>())).Returns(Task.CompletedTask);

        var service = CreateService();
        var dto = new EmissionCreateDto { FacilityId = facilityId, GasType = "CH4", QuantityTco2e = 100m, Source = "IOT_SENSOR" };

        await service.RegisterAsync(dto);

        _alertRepoMock.Verify(r => r.AddAsync(It.Is<Alert>(a =>
            a.Severity == "CRITICAL" && a.AlertType == "THRESHOLD")), Times.Once);
    }

    [Fact]
    public async Task GetSummaryAsync_WhenBelow80Pct_ShouldReturnNormalStatus() {
        var companyId = "comp-001";
        var facilities = new List<Facility> {
            new Facility { FacilityId = "fac-001", Name = "Plataforma P-51", EmissionLimitTco2 = 2000m }
        };
        _facilityRepoMock.Setup(r => r.GetByCompanyAsync(companyId)).ReturnsAsync(facilities);
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync("fac-001")).ReturnsAsync(1200m);

        var service = CreateService();

        var result = (await service.GetSummaryAsync(companyId)).ToList();

        result.Should().HaveCount(1);
        result[0].ComplianceStatus.Should().Be("NORMAL");
        result[0].PercentUsed.Should().Be(60m);
    }
}
