using Shared.Models;

namespace AssistantClient.Services;

public interface IPatientService
{
    Task<GetPatientsResult> GetPatientsAsync(int page = 1, int pageSize = 10);
    Task<bool> AddPatientAsync(PatientDto patient);
}
