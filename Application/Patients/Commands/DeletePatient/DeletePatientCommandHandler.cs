using Application.Common.Interfaces;
using MediatR;

namespace Application.Patients.Commands.DeletePatient;

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
{
    private readonly IPatientRepository _repository;

    public DeletePatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(request.Id);
        if (patient is null)
            return false;

        await _repository.DeleteAsync(patient);
        return true;
    }
}
