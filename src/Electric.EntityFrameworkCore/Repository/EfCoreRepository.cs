using Electric.Core;
using Electric.Domain.Entitys.Commons;
using Electric.Domain.Repository;
using Electric.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Electric.EntityFrameworkCore.Repository
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class EfCoreRepository<TEntity, TKey> : IBasicRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected ApplicationDbContext _db;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        public EfCoreRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entity, nameof(entity));

            _db.Remove(entity);

            if (autoSave)
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 根据Id删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(id, nameof(id));

            var entity = _db.Set<TEntity>().Find(id);

            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entities, nameof(entities));

            _db.RemoveRange(entities);

            if (autoSave)
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 根据Id，批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(ids, nameof(ids));

            var entities = await _db.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }

        /// <summary>
        /// 根据Id返回记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(id, nameof(id));

            var query = _db.Set<TEntity>().IncludeDetails(includeDetails);

            return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        /// <summary>
        /// 获取记录总数量
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Set<TEntity>().LongCountAsync(cancellationToken);
        }

        /// <summary>
        /// 根据筛选条件，获取总记录数
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            Check.NotNull(specification, nameof(specification));

            var query = _db.Set<TEntity>().Where(specification.ToExpression());

            return await query.LongCountAsync(cancellationToken);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await _db.Set<TEntity>().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据筛选条件返回列表
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification, int skipCount, int maxResultCount, string sorting, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(specification, nameof(specification));

            var query = _db.Set<TEntity>().IncludeDetails(includeDetails).Where(specification.ToExpression()).Skip(skipCount).Take(maxResultCount);

            if (!string.IsNullOrEmpty(sorting))
            {
                query = query.OrderBy(sorting);
            }

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 翻页返回记录列表
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var query = _db.Set<TEntity>().Skip(skipCount).Take(maxResultCount);

            if (!string.IsNullOrEmpty(sorting))
            {
                query = query.OrderBy(sorting);
            }
            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entity, nameof(entity));

            entity = _db.Add(entity).Entity;

            if (autoSave)
            {
                await _db.SaveChangesAsync(cancellationToken);
            }

            return entity;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entities, nameof(entities));

            _db.AddRange(entities);

            if (autoSave)
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entity, nameof(entity));

            entity = _db.Update(entity).Entity;

            if (autoSave)
            {
                await _db.SaveChangesAsync(cancellationToken);
            }

            return entity;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entities, nameof(entities));

            _db.UpdateRange(entities);

            if (autoSave)
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
