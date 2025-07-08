using Application.Common.Interfaces;
using Application.Patients.Queries.GetPatients;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Commands;
public class GetPatientsQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Two_Patients()
    {
        var patients = new List<Patient>
    {
        new() { Id = Guid.NewGuid(), Name = "Teszt Béla" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária" }
    };

        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetPagedAsync(1, 10)).ReturnsAsync(patients);

        var handler = new GetPatientsQueryHandler(repo.Object);
        var result = await handler.Handle(new GetPatientsQuery(1, 10), default);

        result.Patients.Should().HaveCount(2);
        result.Patients.Select(p => p.Name).Should().Contain(new[] { "Teszt Béla", "Teszt Mária" });
    }

    [Fact]
    public async Task Should_Return_Ten_Patients()
    {
        // Arrange
        var patients = new List<Patient>
    {
        new() { Id = Guid.NewGuid(), Name = "Teszt Béla" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária2" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária3" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária4" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária5" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária6" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária7" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária8" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária9" },
        new() { Id = Guid.NewGuid(), Name = "Teszt Mária10" }
    };

        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetPagedAsync(1, 10)).ReturnsAsync(patients.Take(10).ToList());
        repo.Setup(r => r.CountAsync()).ReturnsAsync(patients.Count);

        var handler = new GetPatientsQueryHandler(repo.Object);
        var query = new GetPatientsQuery
        {
            Page = 1,
            PageSize = 10
        };

        var result = await handler.Handle(query, default);

        result.Patients.Should().HaveCount(10);
        result.TotalCount.Should().Be(11);
        result.NextPageLink.Should().NotBeNull();
        result.PreviousPageLink.Should().BeNull();
        result.Patients.Select(p => p.Name).Should().Contain(new[]
        {
        "Teszt Béla", "Teszt Mária", "Teszt Mária2", "Teszt Mária3", "Teszt Mária4",
        "Teszt Mária5", "Teszt Mária6", "Teszt Mária7", "Teszt Mária8", "Teszt Mária9"
    });
    }

}