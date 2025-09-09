using Microsoft.AspNetCore.Mvc;
using ToDoApp.DTOs;
using ToDoApp.DTOs.Pagination;
using ToDoApp.Services;

namespace ToDoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _service;

    public ToDoController(IToDoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PaginationListDTO<ToDoItemDTO>>> Get(
        [FromQuery] PaginationRequest request,
        [FromQuery] ToDoQueryFilters queryFilters
        )
    {
        return await _service.GetToDoItemsAsync(
            request.Page,
            request.PageSize,
            queryFilters.Search,
            queryFilters.IsCompleted,
            queryFilters.From,
            queryFilters.To);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItemDTO>> Get(int id)
    {
        var item = await _service.GetToDoItemAsync(id);
        return item is not null ? item : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<ToDoItemDTO>> Post([FromBody] ToDoItemCreateDTO request)
    {
        return await _service.CreateToDoAsync(request);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ToDoItemDTO>> Patch(int id, [FromBody] bool isCompleted)
    {
        var todoItem = await _service.ChangeToDoItemStatusAsync(id, isCompleted);

        return todoItem is not null ? todoItem : NotFound();
    }


}
