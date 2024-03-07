using Electric.Core.UOW;
using Electric.Domain.Entitys.Commons;
using Electric.Domain.Repository;
using Electric.Domain.Specifications;
using Electric.SqlSugarCore.Mapping;
using Electric.SqlSugarCore.Po;
using Electric.SqlSugarCore.UOW;
using SqlSugar;
using System.Linq.Expressions;
using Check = Electric.Core.Check;

namespace Electric.SqlSugarCore.Repository
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="TEntityPo"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class SqlSugarCoreRepository<TEntityPo, TEntity, TKey> : IBasicRepository<TEntity, TKey> where TEntity : Entity<TKey>
        where TEntityPo : EntityPo<TKey>, new()
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        protected ISqlSugarClient _db;

        /// <summary>
        /// 对象映射
        /// </summary>
        protected SqlSugarMapper _sqlSugarMapper;

        /// <summary>
        /// 工作单元
        /// </summary>
        protected SqlSugarCoreUnitOfWork _sqlSugarCoreUnitOfWork;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqlSugarMapper"></param>
        /// <param name="unitOfWork"></param>
        public SqlSugarCoreRepository(ISqlSugarClient db, SqlSugarMapper sqlSugarMapper, IUnitOfWork unitOfWork)
        {
            _sqlSugarMapper = sqlSugarMapper;
            _db = db;
            _sqlSugarCoreUnitOfWork = unitOfWork as SqlSugarCoreUnitOfWork;
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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(entity, nameof(entity));

            var entityPo = _sqlSugarMapper.Map<TEntityPo>(entity);
            var command = _db.DeleteNav(entityPo).IncludesAllFirstLayer();

            if (autoSave)
            {
                await command.ExecuteCommandAsync();
            }
            else
            {
                _sqlSugarCoreUnitOfWork.AddDeleteNavMethodInfo(command);
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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(id, nameof(id));

            var entity = await FindAsync(id);

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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(entities, nameof(entities));

            var entityPos = _sqlSugarMapper.Map<List<TEntityPo>>(entities);
            var command = _db.DeleteNav(entityPos).IncludesAllFirstLayer();

            if (autoSave)
            {
                await command.ExecuteCommandAsync();
            }
            else
            {
                _sqlSugarCoreUnitOfWork.AddDeleteNavMethodInfo(command);
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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(ids, nameof(ids));

            var entities = await _db.Queryable<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();

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

            var query = _db.Queryable<TEntityPo>().IncludeDetails(includeDetails);

            var doEntity = await query.FirstAsync(x => x.Id.Equals(id), cancellationToken);

            return _sqlSugarMapper.Map<TEntity>(doEntity);
        }

        /// <summary>
        /// 返回记录总数量
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Queryable<TEntity>().CountAsync(cancellationToken);
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

            //表达式转换
            var poExpressionVisitor = new PoExpressionVisitor<TEntityPo>();
            var lamdba = poExpressionVisitor.Modify(specification.ToExpression()) as Expression<Func<TEntityPo, bool>>;

            var query = _db.Queryable<TEntityPo>().Where(lamdba);

            return await query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var list = await _db.Queryable<TEntityPo>().ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<TEntity>>(list);
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

            //表达式转换
            var poExpressionVisitor = new PoExpressionVisitor<TEntityPo>();
            var lamdba = poExpressionVisitor.Modify(specification.ToExpression()) as Expression<Func<TEntityPo, bool>>;

            var query = _db.Queryable<TEntityPo>().IncludeDetails(includeDetails)
                .Where(lamdba).Skip(skipCount).Take(maxResultCount);

            if (!string.IsNullOrEmpty(sorting))
            {
                query = query.OrderBy(sorting);
            }

            var list = await query.ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<TEntity>>(list);
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
            var query = _db.Queryable<TEntity>().Skip(skipCount).Take(maxResultCount);

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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(entity, nameof(entity));

            var entityPo = _sqlSugarMapper.Map<TEntityPo>(entity);
            var command = _db.InsertNav(entityPo).IncludesAllFirstLayer();

            if (autoSave)
            {
                await command.ExecuteCommandAsync();
            }
            else
            {
                _sqlSugarCoreUnitOfWork.AddInsertNavMethodInfo(command);
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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(entities, nameof(entities));

            var entityPos = _sqlSugarMapper.Map<List<TEntityPo>>(entities);
            var command = _db.InsertNav(entityPos).IncludesAllFirstLayer();

            if (autoSave)
            {
                await command.ExecuteCommandAsync();
            }
            else
            {
                _sqlSugarCoreUnitOfWork.AddInsertNavMethodInfo(command);
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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(entity, nameof(entity));

            var entityPo = _sqlSugarMapper.Map<TEntityPo>(entity);
            var command = _db.UpdateNav(entityPo).IncludesAllFirstLayer();


            if (autoSave)
            {
                await command.ExecuteCommandAsync();
            }
            else
            {
                _sqlSugarCoreUnitOfWork.AddUpdateNavMethodInfo(command);
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
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(entities, nameof(entities));

            var entityPos = _sqlSugarMapper.Map<List<TEntityPo>>(entities);
            var command = _db.UpdateNav(entityPos).IncludesAllFirstLayer();

            if (autoSave)
            {
                await command.ExecuteCommandAsync();
            }
            else
            {
                _sqlSugarCoreUnitOfWork.AddUpdateNavMethodInfo(command);
            }
        }
    }
}
