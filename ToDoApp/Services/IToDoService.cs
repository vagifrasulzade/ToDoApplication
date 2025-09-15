using ToDoApp.DTOs;
using ToDoApp.DTOs.Pagination;

namespace ToDoApp.Services;

public interface IToDoService
{
    Task<PaginationListDTO<ToDoItemDTO>> GetToDoItemsAsync(
        string userId,
        int page,
        int pageSize,
        string? search,
        bool? isCompleted,
        DateTimeOffset? from,
        DateTimeOffset? to);
    Task<ToDoItemDTO> GetToDoItemAsync(string userId,int id);
    Task<ToDoItemDTO> CreateToDoAsync(string userId,ToDoItemCreateDTO request);
    Task<ToDoItemDTO> ChangeToDoItemStatusAsync(string userId, int id, bool isCompleted);
}
