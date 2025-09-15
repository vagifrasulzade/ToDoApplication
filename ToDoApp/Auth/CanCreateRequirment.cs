using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace ToDoApp.Auth;

public class CanCreateRequirment : IAuthorizationRequirement, IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == "permissions");
        if (claim is not null)
        {
            var permissions = JsonSerializer.Deserialize<string[]>(claim.Value);
            if (permissions!.Contains("CanCreate"))
            {
                context.Succeed(this);
            }
            else
            {
                context.Fail();
            }
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}

