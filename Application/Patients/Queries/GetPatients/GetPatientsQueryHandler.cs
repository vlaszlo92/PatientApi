using System.ComponentModel;
using System.Net.Sockets;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Patients.Queries.GetPatients;

public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, GetPatientsResult>
{
    private readonly IPatientRepository _repository;

    public GetPatientsQueryHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetPatientsResult> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _repository.CountAsync();
        var patients = await _repository.GetPagedAsync(request.Page, request.PageSize);

        return new GetPatientsResult
        {
            TotalCount = totalCount,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            Patients = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                Name = p.Name,
                HealthInsuranceNumber = p.HealthInsuranceNumber,
                Diagnosis = p.Diagnosis??"",
                Address = p.Address,
                Complaints = p.Complaints,
                CreatedAt = p.CreatedAt
            }).ToList(),
            PreviousPageLink = request.Page > 1 ? $"/api/patients?page={request.Page - 1}&pageSize={request.PageSize}" : null,
            NextPageLink = request.Page * request.PageSize < totalCount ? $"/api/patients?page={request.Page + 1}&pageSize={request.PageSize}" : null
        };
    }
}
