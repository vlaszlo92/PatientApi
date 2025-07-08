using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IPatientRepository _repository;

    public CreatePatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByHealthInsuranceNumberAsync(request.HealthInsuranceNumber))
            throw new ValidationException("Már létezik páciens ezzel a TAJ-számmal.");  

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            HealthInsuranceNumber = request.HealthInsuranceNumber,
            Name = request.Name,
            Address = request.Address,
            Complaints = request.Complaints,
            CreatedAt = DateTime.UtcNow
        };

        return await _repository.AddAsync(patient);
    }
}
