using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Abstracts;

namespace Services.DbTransactions.Abstracts
{
    public interface ITransactionScope: IDisposable
    {
        void Dispose();

        /// <summary>
        /// Получить репозиторий в рамках scope
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Сохранить и закомитить изменения в рамках scope
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Откатить изменения в рамках scope
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Прикрепить сущность к текущему scope (контексту)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Attach<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new();

        /// <summary>
        /// Открепить сущность от scope (контекста)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Detach<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new();
    }
}
