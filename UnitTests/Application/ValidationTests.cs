using Application.Patients.Commands.CreatePatient;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace UnitTests.Application.Commands;

public class CreatePatientCommandValidatorTests
{
    private readonly CreatePatientCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreatePatientCommand("Kovács Béla", "Debrecen, Piac utca 1.", "123-456-789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var command = new CreatePatientCommand("", "Debrecen", "123-456-789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Too_Short()
    {
        var command = new CreatePatientCommand("A", "Debrecen", "123-456-789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Too_Long()
    {
        var command = new CreatePatientCommand(new string('A', 33), "Debrecen", "123-456-789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Name_Has_Invalid_Characters()
    {
        var command = new CreatePatientCommand("Teszt123", "Debrecen", "123-456-789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Taj_Is_Invalid_Format()
    {
        var command = new CreatePatientCommand("Teszt Elek", "Debrecen", "123456789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.HealthInsuranceNumber);
    }

    [Fact]
    public void Should_Fail_When_Address_Is_Too_Long()
    {
        var command = new CreatePatientCommand("Teszt Elek", new string('B', 65), "123-456-789", "Fejfájás");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void Should_Fail_When_Complaints_Is_Too_Long()
    {
        var command = new CreatePatientCommand("Teszt Elek", "Debrecen", "123-456-789", new string('C', 101));

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Complaints);
    }

    [Fact]
    public void Should_Fail_When_All_Fields_Are_Empty()
    {
        var command = new CreatePatientCommand("", "", "", "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Address);
        result.ShouldHaveValidationErrorFor(x => x.HealthInsuranceNumber);
        result.ShouldHaveValidationErrorFor(x => x.Complaints);
    }
}
