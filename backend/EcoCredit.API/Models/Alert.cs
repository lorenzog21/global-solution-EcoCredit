namespace EcoCredit.API.Models;

public class Alert {
    public string AlertId { get; set; } = Guid.NewGuid().ToString();
    public string FacilityId { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool Resolved { get; set; } = false;
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Facility? Facility { get; set; }
}
