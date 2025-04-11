using System.Security.Claims;
using DTO.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Services.User.SystemScopeAccessor;

public class SystemUserScope : IDisposable
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ClaimsPrincipal? _originalUser;

    public SystemUserScope(IHttpContextAccessor contextAccessor, IOptions<SystemUserSettings> sysUser)
    {
        _contextAccessor = contextAccessor;
        _originalUser = _contextAccessor.HttpContext?.User;

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, sysUser.Value.UserName),
            new Claim(ClaimTypes.Role, sysUser.Value.IsAdmin.ToString()),
            new Claim(ClaimTypes.UserData, sysUser.Value.UserId.ToString()),
        };

        var identity = new ClaimsIdentity(claims, "System");
        var principal = new ClaimsPrincipal(identity);

        if (_contextAccessor.HttpContext is not null)
            _contextAccessor.HttpContext.User = principal;
    }


    public void Dispose()
    {
        if (_contextAccessor.HttpContext is not null && _originalUser is not null)
            _contextAccessor.HttpContext.User = _originalUser;
    }
}