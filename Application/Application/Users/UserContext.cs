using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Security.Claims;

namespace Application.Users;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User ?? throw new InvalidOperationException("User context is not present.");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(ClaimTypes.Email)!.Value;
        var roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value);
        return new CurrentUser(userId, email, roles);
    }
}