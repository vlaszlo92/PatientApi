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
        new() { Id = Guid.NewGuid(), Name = "Teszt B�la" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria" }
    };

        var repo = new Mock<IPatientRepository>();
        repo.Setup(r => r.GetPagedAsync(1, 10)).ReturnsAsync(patients);

        var handler = new GetPatientsQueryHandler(repo.Object);
        var result = await handler.Handle(new GetPatientsQuery(1, 10), default);

        result.Patients.Should().HaveCount(2);
        result.Patients.Select(p => p.Name).Should().Contain(new[] { "Teszt B�la", "Teszt M�ria" });
    }

    [Fact]
    public async Task Should_Return_Ten_Patients()
    {
        // Arrange
        var patients = new List<Patient>
    {
        new() { Id = Guid.NewGuid(), Name = "Teszt B�la" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria2" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria3" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria4" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria5" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria6" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria7" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria8" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria9" },
        new() { Id = Guid.NewGuid(), Name = "Teszt M�ria10" }
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
        "Teszt B�la", "Teszt M�ria", "Teszt M�ria2", "Teszt M�ria3", "Teszt M�ria4",
        "Teszt M�ria5", "Teszt M�ria6", "Teszt M�ria7", "Teszt M�ria8", "Teszt M�ria9"
    });
    }

}