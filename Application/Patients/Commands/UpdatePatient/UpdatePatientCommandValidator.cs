using FluentValidation;

namespace Application.Patients.Commands.UpdatePatient;

public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Az azonosító kötelező.");

        RuleFor(x => x.Diagnosis)
            .MaximumLength(100).WithMessage("A javasolt kezelés legfeljebb 100 karakter lehet.");
    }
}
