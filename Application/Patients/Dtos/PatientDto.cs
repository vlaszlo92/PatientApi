public class PatientDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string HealthInsuranceNumber { get; set; } = string.Empty;
    public string Complaints { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
