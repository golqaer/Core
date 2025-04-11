using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.Abstracts;
using Services.DbTransactions.Abstracts;

namespace Services.DbTransactions
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : class, IBaseEntity , new()
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Создать новую сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            entity.LastUpdateTick = DateTime.Now.Ticks;
            var result = await _context.Set<TEntity>().AddAsync(entity);

            return result.Entity;
        }

        /// <summary>
        /// Удалить сущность полностью из БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity RemoveExactly(TEntity entity)
        {
            var result = _context.Set<TEntity>().Remove(entity);

            return result.Entity;
        }

        /// <summary>
        /// Удалить сущность полностью из БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity RemoveExactly(long id)
        {
            var result = RemoveExactly(new TEntity { Id = id });

            return result;
        }

        /// <summary>
        /// Удалить сущность (IsDeleted=true)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            var result = Update(entity);

            return result;
        }

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Update(TEntity entity)
        {
            entity.LastUpdateTick = DateTime.Now.Ticks;
            var result = _context.Set<TEntity>().Update(entity);

            return result.Entity;
        }

        /// <summary>
        /// Получить IQueryable по сущности
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }
    }
}
