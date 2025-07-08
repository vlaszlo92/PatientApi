using Application.Common.Interfaces;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool isSqLite)
    {

        services.AddDbContext<AppDbContext>(options =>
        {
            if (isSqLite)
                options.UseSqlite(connectionString);
            else
                options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPatientRepository, PatientRepository>();

        return services;
    }
}
