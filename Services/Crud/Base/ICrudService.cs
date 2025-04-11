using Database.Abstracts;
using Services.DbTransactions.Abstracts;

namespace Services.Crud.Base;

public interface ICrudService<TEntity> where TEntity : class, IBaseEntity, new()
{
    /// <summary>
    /// Создать новую сущность
    /// </summary>
    /// <returns></returns>
    Task<TEntity> CreateAsync(ITransactionScope scope, TEntity @object);

    /// <summary>
    /// Удалить сущность (IsDeleted=true)
    /// </summary>
    /// <returns></returns>
    TEntity Remove(ITransactionScope scope, TEntity @object);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <returns></returns>
    TEntity Update(ITransactionScope scope, TEntity @object);

    /// <summary>
    /// Получить IQueryable по сущности
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> GetAll(ITransactionScope scope);
}