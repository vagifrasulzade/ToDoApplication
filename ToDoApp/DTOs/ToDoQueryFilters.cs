using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.DTOs;

public class ToDoQueryFilters
{
    [FromQuery(Name = "search")]
    public string? Search { get; set; }

    [FromQuery(Name = "isCompleted")]
    public bool? IsCompleted { get; set; }

    [FromQuery(Name = "from")]
    public DateTimeOffset? From { get; set; }
    [FromQuery(Name = "to")]
    public DateTimeOffset? To { get; set; }
}
