namespace EcoCredit.API.Models;

public class CarbonCredit {
    public string CreditId { get; set; } = Guid.NewGuid().ToString();
    public string CompanyId { get; set; } = string.Empty;
    public decimal QuantityTco2 { get; set; }
    public string CreditType { get; set; } = string.Empty;
    public string Status { get; set; } = "AVAILABLE";
    public decimal? PriceUsd { get; set; }
    public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }
    public string? RegistryCode { get; set; }

    public Company? Company { get; set; }
}
