using PatientApi.Controllers;
using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Patients.Commands.CreatePatient;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(PatientsController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// saját DI extension metódusaink
var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

builder.Services.AddSingleton<IConfiguration>(config);
builder.Services.AddApplication(); // Te hozod létre

var patientDbString = config.GetConnectionString("DefaultConnection")
                     ?? config.GetConnectionString("patientdb");

if (string.IsNullOrWhiteSpace(patientDbString))
{
    throw new ArgumentNullException("ConnectionString nem található!");
}
var isSqLite = config.GetValue<bool>("UseSqlite", false);
builder.Services.AddInfrastructure(patientDbString, isSqLite, builder.Environment);

builder.Services.AddValidatorsFromAssemblyContaining<CreatePatientCommandValidator>();
builder.Services.AddFluentValidationAutoValidation(); // automatic integration with [ApiController]

Console.WriteLine($"[DEBUG] UseSqlite = {isSqLite}");
Console.WriteLine($"[DEBUG] Connection string = {patientDbString}");

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    await DbSeeder.SeedAsync(dbContext);
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
