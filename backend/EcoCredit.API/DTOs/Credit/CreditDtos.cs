namespace EcoCredit.API.DTOs.Credit;

public class CreditCreateDto {
    public decimal QuantityTco2 { get; set; }
    public string CreditType { get; set; } = string.Empty;
    public decimal? PriceUsd { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? RegistryCode { get; set; }
}

public class CreditResponseDto {
    public string CreditId { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public decimal QuantityTco2 { get; set; }
    public string CreditType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal? PriceUsd { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? RegistryCode { get; set; }
}

public class CreditBalanceDto {
    public decimal TotalAvailableTco2 { get; set; }
    public decimal TotalUsedTco2 { get; set; }
    public int AvailableCount { get; set; }
    public IEnumerable<CreditResponseDto> Credits { get; set; } = [];
}
