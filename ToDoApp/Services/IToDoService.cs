using ToDoApp.DTOs;
using ToDoApp.DTOs.Pagination;

namespace ToDoApp.Services;

public interface IToDoService
{
    Task<PaginationListDTO<ToDoItemDTO>> GetToDoItemsAsync(
        int page,
        int pageSize,
        string? search,
        bool? isCompleted,
        DateTimeOffset? from,
        DateTimeOffset? to);
    Task<ToDoItemDTO> GetToDoItemAsync(int id);
    Task<ToDoItemDTO> CreateToDoAsync(ToDoItemCreateDTO request);
    Task<ToDoItemDTO> ChangeToDoItemStatusAsync(int id, bool isCompleted);
}
