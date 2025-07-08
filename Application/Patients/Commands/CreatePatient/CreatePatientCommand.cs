using MediatR;

public record CreatePatientCommand(
    string Name,
    string Address,
    string HealthInsuranceNumber,
    string Complaints
) : IRequest<Guid>;