using Application.Patients.Commands.UpdatePatient;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Commands;

public class UpdatePatientCommandHandlerTests
{
    [Fact]
    public async Task Should_Update_Patient_When_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new Patient
        {
            Id = id,
            Diagnosis = "Panasz"
        };

        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        var handler = new UpdatePatientCommandHandler(repo.Object);

        var command = new UpdatePatientCommand(id, "Új kezelés");

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().BeTrue();
        existing.Diagnosis.Should().Be("Új kezelés");
        repo.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_False_When_Patient_Not_Found()
    {
        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Patient?)null);
        var handler = new UpdatePatientCommandHandler(repo.Object);

        var command = new UpdatePatientCommand(Guid.NewGuid(), "X");
        var result = await handler.Handle(command, default);

        result.Should().BeFalse();
        repo.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Never);
    }
}
