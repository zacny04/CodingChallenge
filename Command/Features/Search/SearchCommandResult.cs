namespace Command.Features.Search;

public class SearchCommandResult
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Availability { get; set; }
}
