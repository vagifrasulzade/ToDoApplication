using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.DTOs;
using ToDoApp.DTOs.Pagination;
using ToDoApp.Providers;
using ToDoApp.Services;

namespace ToDoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _service;
    private readonly IRequestUserProvider _userProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public ToDoController(IToDoService service, IRequestUserProvider userProvider)
    {
        _service = service;
        _userProvider = userProvider;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="queryFilters"></param>
    /// <returns></returns>

    [HttpGet]
    //[Authorize(Roles = "admin,moderator,user, x")]
    //[Authorize(Policy = "CanView")]
    public async Task<ActionResult<PaginationListDTO<ToDoItemDTO>>> Get(
        [FromQuery] PaginationRequest request,
        [FromQuery] ToDoQueryFilters queryFilters
        )
    {
        var user = _userProvider.GetUserInfo();

        return await _service.GetToDoItemsAsync(
            user!.Id,
            request.Page,
            request.PageSize,
            queryFilters.Search,
            queryFilters.IsCompleted,
            queryFilters.From,
            queryFilters.To);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpGet("{id}")]
   
    public async Task<ActionResult<ToDoItemDTO>> Get(int id)
    {
        var user = _userProvider.GetUserInfo();
        var item = await _service.GetToDoItemAsync(user.Id,id);
        return item is not null ? item : NotFound();
    }
    /// <summary>
    ///  Create ToDo Item
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="201">Success</response>
    /// <response code="409">Task already created</response>
    /// <response code="403">Forbidden</response>
    [HttpPost]
    public async Task<ActionResult<ToDoItemDTO>> Post([FromBody] ToDoItemCreateDTO request)
    {
        var user = _userProvider.GetUserInfo();
        return await _service.CreateToDoAsync(user.Id, request);
    }
    /// <summary>
    /// Change ToDo Item 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isCompleted"></param>
    /// <returns>Todo item with changed status</returns>

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ToDoItemDTO>> Patch(int id, [FromBody] bool isCompleted)
    {
        var user = _userProvider.GetUserInfo();
        var todoItem = await _service.ChangeToDoItemStatusAsync(user.Id,id, isCompleted);

        return todoItem is not null ? todoItem : NotFound();
    }


    
}
