using DTO.UserSecurityManager;

namespace Services.User.Provider;

public interface IUserProvider
{

    /// <summary>
    /// Получить текущего пользователя
    /// </summary>
    /// <returns></returns>
    UserClaimData? GetUser();
}