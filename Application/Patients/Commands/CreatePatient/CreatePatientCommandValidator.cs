using FluentValidation;

namespace Application.Patients.Commands.CreatePatient;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(x => x.HealthInsuranceNumber)
            .NotEmpty().WithMessage("A TAJ-szám kötelező.")
            .Matches(@"^\d{3}-\d{3}-\d{3}$").WithMessage("Érvénytelen TAJ-formátum.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("A név megadása kötelező.")
            .MinimumLength(2).WithMessage("A név túl rövid.")
            .Matches(@"^[A-Za-zÁÉÍÓÖŐÚÜŰáéíóöőúüű\s\-]+$").WithMessage("A név csak betűket, szóközt és kötőjelet tartalmazhat.")
            .MaximumLength(32).WithMessage("A név legfeljebb 32 karakter lehet.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("A cím megadása kötelező.")
            .MaximumLength(64).WithMessage("A cím legfeljebb 64 karakter lehet.");

        RuleFor(x => x.Complaints)
            .NotEmpty().WithMessage("A panasz megadása kötelező.")
            .MaximumLength(100).WithMessage("A panasz legfeljebb 100 karakter lehet.");
    }
}
