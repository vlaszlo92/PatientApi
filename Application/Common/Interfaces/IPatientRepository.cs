using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IPatientRepository
{
    Task<Guid> AddAsync(Patient patient);
    Task<Patient?> GetByIdAsync(Guid id);
    Task UpdateAsync(Patient patient);
    Task<int> CountAsync();
    Task<List<Patient>> GetPagedAsync(int page, int pageSize);
    Task<bool> ExistsByHealthInsuranceNumberAsync(string hin);
    Task DeleteAsync(Patient patient);
}
