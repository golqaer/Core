using System.Security.Claims;
using DTO.UserSecurityManager;
using Microsoft.AspNetCore.Http;


namespace Services.User.Provider;

public class UserProvider : IUserProvider
{
    private readonly IHttpContextAccessor _accessor;

    public UserProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    /// <summary>
    /// Получить текущего пользователя
    /// </summary>
    /// <returns></returns>
    public UserClaimData? GetUser()
    {
        var user = _accessor.HttpContext?.User;

        if (user is not null && (user.Identity?.IsAuthenticated ?? false))
        {
            var userId = user.FindFirst(ClaimTypes.UserData)?.Value;
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            var userAdmin = user.FindFirst(ClaimTypes.Role)?.Value;

            return new UserClaimData
            {
                UserId = !string.IsNullOrWhiteSpace(userId) ? Convert.ToInt64(userId) : null,
                UserName = userName,
                Admin = !string.IsNullOrWhiteSpace(userAdmin) ? Convert.ToBoolean(userAdmin) : false,
            };
        }

        return null;
    }
}