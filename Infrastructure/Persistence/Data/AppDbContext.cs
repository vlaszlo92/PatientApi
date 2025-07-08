using System.Collections.Generic;
using System.Reflection.Emit;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients => Set<Patient>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PatientConfiguration());
    }
}
