namespace EcoCredit.API.DTOs.Company;

public class CompanyCreateDto {
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Country { get; set; } = "Brazil";
}

public class CompanyResponseDto {
    public string CompanyId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}
