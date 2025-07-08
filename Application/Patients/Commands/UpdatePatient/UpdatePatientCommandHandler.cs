using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Patients.Commands.UpdatePatient;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, bool>
{
    private readonly IPatientRepository _repository;

    public UpdatePatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing is null)
            return false;

        existing.Diagnosis = request.Diagnosis;

        await _repository.UpdateAsync(existing);
        return true;
    }
}
