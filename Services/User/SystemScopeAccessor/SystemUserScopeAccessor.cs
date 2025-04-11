using DTO.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Services.User.SystemScopeAccessor;

public class SystemUserScopeAccessor : ISystemUserScopeAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IOptions<SystemUserSettings> _sysUser;

    public SystemUserScopeAccessor(IHttpContextAccessor contextAccessor, IOptions<SystemUserSettings> sysUser)
    {
        _contextAccessor = contextAccessor;
        _sysUser = sysUser;
    }

    public SystemUserScope Create()
    {
        return new SystemUserScope(_contextAccessor, _sysUser);
    }
}