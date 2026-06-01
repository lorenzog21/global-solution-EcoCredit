namespace EcoCredit.API.Models;

public class EmissionRecord {
    public string RecordId { get; set; } = Guid.NewGuid().ToString();
    public string FacilityId { get; set; } = string.Empty;
    public string GasType { get; set; } = string.Empty;
    public decimal QuantityTco2e { get; set; }
    public string Source { get; set; } = string.Empty;
    public string? SensorId { get; set; }
    public decimal? RawPpm { get; set; }
    public decimal? SatelliteCo2Regional { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public Facility? Facility { get; set; }
}
