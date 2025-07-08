using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.Models;
using Shared.Mock;

namespace AssistantClient.Services;

public class PatientService : IPatientService
{
    private readonly HttpClient? _client;
    private readonly bool _useMock;
    public PatientService(IConfiguration config)
    {
        var env = config["Environment"];
        var baseUrl = config["ApiBaseUrl"];

        _useMock = env.Equals("Development", StringComparison.OrdinalIgnoreCase);

        if (!_useMock)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("ApiBaseUrl nincs beállítva az appsettings.json fájlban.");

            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
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


    public async Task<bool> AddPatientAsync(PatientDto patient)
    {
        if (_useMock)
        {
            await Task.Delay(100);
            patient.CreatedAt = DateTime.Now;
            MockDataStore.Patients.Add(patient);
            return true;
        }

        var response = await _client!.PostAsJsonAsync("api/v1/patients", patient);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[API HIBA] {response.StatusCode}: {errorContent}");
            throw new Exception(errorContent);
        }

        return response.IsSuccessStatusCode;
    }
}
