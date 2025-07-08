using Application.Patients.Commands.DeletePatient;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Moq;
using FluentValidation;

namespace UnitTests.Application.Commands;
public class DeletePatientCommandHandlerTests
{
    [Fact]
    public async Task Should_Delete_Patient_When_Found()
    {
        var id = Guid.NewGuid();
        var patient = new Patient { Id = id };

        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(patient);

        var handler = new DeletePatientCommandHandler(repo.Object);
        var command = new DeletePatientCommand(id);

        var result = await handler.Handle(command, default);

        result.Should().BeTrue();
        repo.Verify(r => r.DeleteAsync(patient), Times.Once);
    }

    [Fact]
    public async Task Should_Return_False_When_Patient_Not_Found()
    {
        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Patient?)null);

        var handler = new DeletePatientCommandHandler(repo.Object);
        var command = new DeletePatientCommand(Guid.NewGuid());

        var result = await handler.Handle(command, default);

        result.Should().BeFalse();
        repo.Verify(r => r.DeleteAsync(It.IsAny<Patient>()), Times.Never);
    }
}
