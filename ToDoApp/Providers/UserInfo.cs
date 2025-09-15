namespace ToDoApp.Providers;

/// <summary>
/// 
/// </summary>
public class UserInfo
{
    /// <summary>
    /// 
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userName"></param>
    public UserInfo(string id, string userName)
    {
        Id = id;
        UserName = userName;
    }
}
