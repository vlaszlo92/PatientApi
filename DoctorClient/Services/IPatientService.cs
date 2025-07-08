using Shared.Models;

namespace DoctorClient.Services;

public interface IPatientService
{
    Task<GetPatientsResult> GetPatientsAsync(int page = 1, int pageSize = 10);
    Task UpdateDiagnosisAsync(Guid id, string diagnosis);
    Task DeletePatientAsync(Guid id);
}