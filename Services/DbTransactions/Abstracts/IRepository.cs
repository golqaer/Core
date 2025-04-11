using Database.Abstracts;

namespace Services.DbTransactions.Abstracts;

public interface IRepository<TEntity> where TEntity : class, IBaseEntity, new()
{
    /// <summary>
    /// Удалить сущность полностью из БД
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TEntity RemoveExactly(TEntity entity);

    /// <summary>
    /// Удалить сущность полностью из БД
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity RemoveExactly(long id);

    /// <summary>
    /// Создать новую сущность
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Удалить сущность (IsDeleted=true)
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TEntity Remove(TEntity entity);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Получить IQueryable по сущности
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> GetAll();
}