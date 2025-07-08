using Application.Patients.Commands.CreatePatient;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Moq;
using FluentValidation;

namespace UnitTests.Application.Commands;

public class CreatePatientCommandHandlerTests
{
    [Fact]
    public async Task Should_Create_Patient_And_Return_Id()
    {
        // Arrange
        var repositoryMock = new Mock<IPatientRepository>();
        var command = new CreatePatientCommand("Teszt Elek", "Debrecen", "123-456-789", "Fejfájás");
        var handler = new CreatePatientCommandHandler(repositoryMock.Object);

        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Patient>()))
            .ReturnsAsync(Guid.NewGuid());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBe(Guid.Empty);
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_HealthInsuranceNumber_Already_Exists()
    {
        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.ExistsByHealthInsuranceNumberAsync("123-456-789")).ReturnsAsync(true);

        var handler = new CreatePatientCommandHandler(repo.Object);
        var command = new CreatePatientCommand("Teszt Elek", "Debrecen", "123-456-789", "Fejfájás");

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*TAJ*");
    }

}
