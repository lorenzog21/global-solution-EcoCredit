namespace EcoCredit.API.Models;

public class Facility {
    public string FacilityId { get; set; } = Guid.NewGuid().ToString();
    public string CompanyId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FacilityType { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal EmissionLimitTco2 { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company? Company { get; set; }
    public ICollection<EmissionRecord> Emissions { get; set; } = new List<EmissionRecord>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
