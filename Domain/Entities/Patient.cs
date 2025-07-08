using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Patient
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(11)]
    public string HealthInsuranceNumber { get; set; } = default!;
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = default!;
    public string Complaints { get; set; } = default!;
    public string? Diagnosis { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Address { get; set; } = default!;
}
