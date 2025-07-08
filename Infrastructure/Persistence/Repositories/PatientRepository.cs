using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient.Id;
    }

    public async Task<Patient?> GetByIdAsync(Guid id) =>
        await _context.Patients.FindAsync(id);

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync() =>
        await _context.Patients.CountAsync();

    public async Task<List<Patient>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.Patients
            .OrderBy(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<bool> ExistsByHealthInsuranceNumberAsync(string hin)
    {
        return await _context.Patients.AnyAsync(p => p.HealthInsuranceNumber == hin);
    }

    public async Task DeleteAsync(Patient patient)
    {
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }

}
