namespace Domain.Entities;

public class Patient
{
    public Guid Id { get; set; }
    public string HealthInsuranceNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Complaints { get; set; } = default!;
    public string? Diagnosis { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Address { get; set; }
}
