namespace EcoCredit.API.Models;

public class Company {
    public string CompanyId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Country { get; set; } = "Brazil";
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    public ICollection<CarbonCredit> Credits { get; set; } = new List<CarbonCredit>();
}
