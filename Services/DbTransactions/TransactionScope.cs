using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.DbTransactions.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.Abstracts;

namespace Services.DbTransactions
{
    public class TransactionScope: ITransactionScope
    {
        private readonly IDbContextTransaction _transaction;
        private readonly Context _context;

        public TransactionScope(Context context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Сохранить и закомитить изменения в рамках scope
        /// </summary>
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        /// <summary>
        /// Откатить изменения в рамках scope
        /// </summary>
        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        /// <summary>s
        /// Получить репозиторий в рамках scope
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity, new()
        {
            return new Repository<TEntity>(_context);
        }

        /// <summary>
        /// Прикрепить сущность к текущему scope (контексту)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Attach<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new()
        {
            _context.Attach(entity);
        }

        /// <summary>
        /// Открепить сущность от scope (контекста)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void Detach<TEntity>(TEntity entity) where TEntity : class, IBaseEntity, new()
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
