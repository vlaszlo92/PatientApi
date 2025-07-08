using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using AssistantClient.Infrastructure;
using AssistantClient.Services;
using Shared.Models;

namespace AssistantClient.ViewModels;

public class PatientViewModel : INotifyPropertyChanged
{
    private readonly IPatientService _service;

    public ObservableCollection<PatientDto> Patients { get; } = new();

    private string _name = "";
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private string _address = "";
    public string Address
    {
        get => _address;
        set { _address = value; OnPropertyChanged(); }
    }

    private string _healthInsuranceNumber = "";
    public string HealthInsuranceNumber
    {
        get => _healthInsuranceNumber;
        set { _healthInsuranceNumber = value; OnPropertyChanged(); }
    }

    private string _complaints = "";
    public string Complaints
    {
        get => _complaints;
        set { _complaints = value; OnPropertyChanged(); }
    }

    private string _statusMessage = "";
    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPropertyChanged();
                RaisePagingCanExecuteChanged();
            }
        }
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set
        {
            if (_totalPages != value)
            {
                _totalPages = value;
                OnPropertyChanged();
                RaisePagingCanExecuteChanged();
            }
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ICommand SubmitCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    public PatientViewModel(IPatientService service)
    {
        _service = service;

        SubmitCommand = new RelayCommand(async _ => await SubmitAsync());
        RefreshCommand = new RelayCommand(async _ => await LoadPatientsAsync());
        NextPageCommand = new RelayCommand(async _ => await LoadPatientsAsync(CurrentPage + 1), _ => CurrentPage < TotalPages);
        PreviousPageCommand = new RelayCommand(async _ => await LoadPatientsAsync(CurrentPage - 1), _ => CurrentPage > 1);

        _ = LoadPatientsAsync();
    }

    private async Task SubmitAsync()
    {
        if (!Validate())
        {
            StatusMessage = "Hibás adat!";
            return;
        }

        var patient = new PatientDto
        {
            Name = Name,
            Address = Address,
            HealthInsuranceNumber = HealthInsuranceNumber,
            Complaints = Complaints,
            CreatedAt = DateTime.Now
        };

        try
        {
            await _service.AddPatientAsync(patient);
            Name = Address = HealthInsuranceNumber = Complaints = "";
            StatusMessage = "Mentve!";
            await LoadPatientsAsync();
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                "Nem sikerült kapcsolódni a szerverhez.\nKérlek, ellenőrizd hogy a szerver fut-e.\n\nRészletek:\n" + ex.Message,
                "Kapcsolódási hiba",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }

        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Address));
        OnPropertyChanged(nameof(HealthInsuranceNumber));
        OnPropertyChanged(nameof(Complaints));
        OnPropertyChanged(nameof(StatusMessage));
    }

    private async Task LoadPatientsAsync(int page = 1)
    {
        IsLoading = true;
        var result = await _service.GetPatientsAsync(page);

        Patients.Clear();
        foreach (var p in result.Patients.OrderBy(p => p.CreatedAt))
            Patients.Add(p);

        CurrentPage = result.CurrentPage;
        TotalPages = result.TotalPages;

        IsLoading = false;
    }

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(Name)) return false;
        if (!Regex.IsMatch(HealthInsuranceNumber, @"^\d{3}-\d{3}-\d{3}$")) return false;
        return true;
    }

    private void RaisePagingCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
