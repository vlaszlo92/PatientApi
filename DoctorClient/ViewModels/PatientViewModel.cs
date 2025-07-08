using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DoctorClient.Infrastructure;
using DoctorClient.Services;
using Shared.Models;

namespace DoctorClient.ViewModels;

public class PatientViewModel : INotifyPropertyChanged
{
    private readonly IPatientService _service;

    public PatientViewModel(IPatientService service)
    {
        _service = service;

        SaveDiagnosisCommand = new RelayCommand(async _ => await SaveDiagnosisAsync(), _ => SelectedPatient != null);
        DeleteCommand = new RelayCommand(async _ => await DeletePatientAsync(), _ => SelectedPatient != null);
        RefreshCommand = new RelayCommand(async _ => await LoadPatientsAsync());

        PreviousPageCommand = new RelayCommand(_ => ChangePage(-1), _ => HasPreviousPage);
        NextPageCommand = new RelayCommand(_ => ChangePage(1), _ => HasNextPage);

        _ = LoadPatientsAsync();
    }

    public ObservableCollection<PatientDto> Patients { get; } = new();

    private PatientDto? _selectedPatient;
    public PatientDto? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            if (_selectedPatient != value)
            {
                _selectedPatient = value;
                OnPropertyChanged();
                RaiseCommandStates();
            }
        }
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set { _currentPage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasPreviousPage)); OnPropertyChanged(nameof(HasNextPage)); }
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set { _totalPages = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasPreviousPage)); OnPropertyChanged(nameof(HasNextPage)); }
    }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    private const int PageSize = 10;

    public ICommand SaveDiagnosisCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand NextPageCommand { get; }

    private async Task LoadPatientsAsync()
    {
        try
        {
            var result = await _service.GetPatientsAsync(CurrentPage, PageSize);

            Patients.Clear();
            foreach (var p in result.Patients.OrderBy(p => p.CreatedAt))
                Patients.Add(p);

            TotalPages = result.TotalPages;
            CurrentPage = result.CurrentPage;

            RaiseCommandStates();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Hiba a betöltés során: {ex.Message}";
        }
    }

    private async Task SaveDiagnosisAsync()
    {
        if (SelectedPatient == null) return;

        try
        {
            await _service.UpdateDiagnosisAsync(SelectedPatient.Id, SelectedPatient.Diagnosis);
            StatusMessage = "Diagnózis mentve.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Hiba a mentés során: {ex.Message}";
        }
    }

    private async Task DeletePatientAsync()
    {
        if (SelectedPatient == null) return;

        try
        {
            await _service.DeletePatientAsync(SelectedPatient.Id);
            StatusMessage = "Páciens törölve.";
            await LoadPatientsAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Hiba a törlés során: {ex.Message}";
        }
    }

    private async void ChangePage(int delta)
    {
        CurrentPage += delta;
        await LoadPatientsAsync();
    }

    private void RaiseCommandStates()
    {
        (SaveDiagnosisCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
