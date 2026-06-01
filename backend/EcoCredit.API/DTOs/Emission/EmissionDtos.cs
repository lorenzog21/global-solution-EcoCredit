namespace EcoCredit.API.DTOs.Emission;

public class EmissionCreateDto {
    public string FacilityId { get; set; } = string.Empty;
    public string GasType { get; set; } = string.Empty;
    public decimal QuantityTco2e { get; set; }
    public string Source { get; set; } = "IOT_SENSOR";
    public string? SensorId { get; set; }
    public decimal? RawPpm { get; set; }
    public decimal? SatelliteCo2Regional { get; set; }
}

public class EmissionSummaryDto {
    public string FacilityId { get; set; } = string.Empty;
    public string FacilityName { get; set; } = string.Empty;
    public decimal TotalEmittedTco2e { get; set; }
    public decimal EmissionLimitTco2 { get; set; }
    public decimal PercentUsed { get; set; }
    public string ComplianceStatus { get; set; } = string.Empty;
}
