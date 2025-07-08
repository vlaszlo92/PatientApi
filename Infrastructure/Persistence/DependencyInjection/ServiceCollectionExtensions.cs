using Application.Common.Interfaces;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool isSqLite, IWebHostEnvironment env)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            if (isSqLite)
                options.UseSqlite(connectionString);
            else
                options.UseSqlServer(connectionString);

            if (!env.IsDevelopment())
            {
                options.ConfigureWarnings(w =>
                    w.Ignore(RelationalEventId.PendingModelChangesWarning));
            }
        });

        services.AddScoped<IPatientRepository, PatientRepository>();

        return services;
    }
}
