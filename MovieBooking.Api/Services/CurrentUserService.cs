using MovieBooking.Application.Contracts.Application;
using System.Security.Claims;

namespace MovieBooking.API.Services
{
   public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
   {
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
    private ClaimsPrincipal? _user;
   }
}
