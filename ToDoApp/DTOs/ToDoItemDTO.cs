namespace ToDoApp.DTOs;

public class ToDoItemDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool isCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
