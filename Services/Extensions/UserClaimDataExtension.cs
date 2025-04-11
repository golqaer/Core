using DTO.UserSecurityManager;
using Services.User.Provider;

namespace Services.Extensions;

public static class UserClaimDataExtension
{
    public static bool VerifyAdminRights(this UserClaimData user, bool withException = false)
    {
        if (!user.Admin) return withException ? throw new UserProviderException.AccessDenied(UserProviderException.AccessDenied.NEED_ADMIN): false;
        return true;
    }

    public static bool Verify(this UserClaimData? user, bool withException = false)
    {
        if (user is null) return withException ? throw new UserProviderException.AccessDenied(UserProviderException.AccessDenied.NEED_USER): false;
        return true;
    }
}