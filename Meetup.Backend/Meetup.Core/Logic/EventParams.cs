namespace Meetup.Core.Logic;

public class EventParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 3;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string City { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string OrderByDateTime { get; set; } = "Upcoming";
}