namespace EcoCredit.API.DTOs.Facility;

public class FacilityCreateDto {
    public string CompanyId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FacilityType { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal EmissionLimitTco2 { get; set; }
}

public class FacilityResponseDto {
    public string FacilityId { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FacilityType { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal EmissionLimitTco2 { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}
