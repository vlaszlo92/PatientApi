using MediatR;

public record UpdatePatientCommand(
    Guid Id,
    string? Diagnosis
) : IRequest<bool>;
