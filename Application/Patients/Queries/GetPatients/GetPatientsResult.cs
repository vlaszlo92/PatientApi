public class GetPatientsResult
{
    public List<PatientDto> Patients { get; set; } = [];
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public int PageSize { get; set; }

    public string? PreviousPageLink { get; set; }
    public string? NextPageLink { get; set; }
}
