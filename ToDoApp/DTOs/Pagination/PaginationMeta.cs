namespace ToDoApp.DTOs.Pagination;

public class PaginationMeta
{
    /// <summary>
    /// 
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int TotalPages { get; set; }
    /// <summary>
    /// Method for calculating meta information about pagination
    /// </summary>
    /// <param name="page">number of current page</param>
    /// <param name="pageSize">How many items per page</param>
    /// <param name="totalCount">How many pages</param>

    public PaginationMeta(int page, int pageSize, int totalCount)
    {
        Page = page;
        PageSize = pageSize;
        TotalPages = Convert.ToInt32(Math.Ceiling(1.0f * totalCount/pageSize));
    }
}
