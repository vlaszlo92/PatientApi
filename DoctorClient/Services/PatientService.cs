using Shared.Models;
using Microsoft.Extensions.Configuration;
using Shared.Mock;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;


namespace DoctorClient.Services;

public class PatientService : IPatientService
{
    private readonly HttpClient? _client;
    private readonly bool _useMock;
    
    public PatientService()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var env = config["Environment"] ?? throw new ArgumentNullException(nameof(config), "Environment configuration is missing.");
        var baseUrl = config["ApiBaseUrl"];

        _useMock = env.Equals("Development", StringComparison.OrdinalIgnoreCase);

        if (!_useMock)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("ApiBaseUrl nincs beállítva az appsettings.json fájlban.");

            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }
        else
        {
            _client = new HttpClient();
        }
    }

    public async Task<GetPatientsResult> GetPatientsAsync(int page = 1, int pageSize = 10)
    {
        if (_useMock)
        {
            await Task.Delay(100);
            var paged = MockDataStore.Patients
                .OrderBy(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    HealthInsuranceNumber = p.HealthInsuranceNumber,
                    Complaints = p.Complaints,
                    Diagnosis = p.Diagnosis,
                    CreatedAt = p.CreatedAt
                })
                .ToList();

            return new GetPatientsResult
            {
                Patients = paged,
                TotalCount = MockDataStore.Patients.Count,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)MockDataStore.Patients.Count / pageSize),
                PreviousPageLink = page > 1 ? $"?page={page - 1}&pageSize={pageSize}" : null,
                NextPageLink = page * pageSize < MockDataStore.Patients.Count ? $"?page={page + 1}&pageSize={pageSize}" : null
            };
        }

        var response = await _client!.GetAsync($"/api/v1/patients?page={page}&pageSize={pageSize}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GetPatientsResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task UpdateDiagnosisAsync(Guid id, string diagnosis)
    {
        if (_useMock)
        {
            await Task.Delay(100); 
            var patient = MockDataStore.Patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                patient.Diagnosis = diagnosis;
            }
            return;
        }

        var payload = new { Diagnosis = diagnosis, Id = id };
        var content = JsonContent.Create(payload);

        var response = await _client!.PutAsync($"/api/v1/patients/{id}/diagnosis", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePatientAsync(Guid id)
    {
        if (_useMock)
        {
            await Task.Delay(100); 
            var patient = MockDataStore.Patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                MockDataStore.Patients.Remove(patient);
            }
            return;
        }
        var response = await _client!.DeleteAsync($"/api/v1/patients/{id}");
        response.EnsureSuccessStatusCode();
    }
}
