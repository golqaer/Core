using Database.Abstracts;
using Services.DbTransactions.Abstracts;
using Services.Extensions;
using Services.User.Provider;

namespace Services.Crud.Base;

public class CrudService<TEntity> : ICrudService<TEntity> where TEntity : class, IBaseEntity, new()
{
    protected readonly IUserProvider _userSecurityMangerSrv;

    public CrudService(IUserProvider userSecurityMangerSrv)
    {
        _userSecurityMangerSrv = userSecurityMangerSrv;
    }

    /// <summary>
    /// Создать новую сущность
    /// </summary>
    /// <returns></returns>
    public virtual async Task<TEntity> CreateAsync(ITransactionScope scope, TEntity @object)
    {
        return await DoActionWithUserVerify(() => scope.GetRepository<TEntity>().CreateAsync(@object));
    }

    /// <summary>
    /// Получить IQueryable по сущности
    /// </summary>
    /// <returns></returns>
    public virtual IQueryable<TEntity> GetAll(ITransactionScope scope)
    {
        return DoActionWithUserVerify(() => scope.GetRepository<TEntity>().GetAll()).Where(e => !e.IsDeleted);
    }

    /// <summary>
    /// Удалить сущность (IsDeleted=true)
    /// </summary>
    /// <returns></returns>
    public virtual TEntity Remove(ITransactionScope scope, TEntity @object)
    {
        return DoActionWithUserVerify(() => scope.GetRepository<TEntity>().Remove(@object));
    }

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <returns></returns>
    public virtual TEntity Update(ITransactionScope scope, TEntity @object)
    {
        return DoActionWithUserVerify(() => scope.GetRepository<TEntity>().Update(@object));
    }

    private T DoActionWithUserVerify<T>(Func<T> action)
    {
        _userSecurityMangerSrv.GetUser().Verify(true);

        return action();
    }
}