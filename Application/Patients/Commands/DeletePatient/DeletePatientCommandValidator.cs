using FluentValidation;

namespace Application.Patients.Commands.DeletePatient;

public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
{
    public DeletePatientCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("A páciens azonosítója kötelező.");
    }
}
