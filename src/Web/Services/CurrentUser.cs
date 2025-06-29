using System.Security.Claims;

using TaskTimeTracker.Application.Common.Interfaces;

namespace TaskTimeTracker.Web.Services;
public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string UserId => Id ?? throw new UnauthorizedAccessException("User is not authenticated");
}
