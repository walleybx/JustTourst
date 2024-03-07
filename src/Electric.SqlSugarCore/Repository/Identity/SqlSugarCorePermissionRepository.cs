using Electric.Core.UOW;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Electric.SqlSugarCore.Mapping;
using Electric.SqlSugarCore.Po;
using SqlSugar;
using Check = Electric.Core.Check;

namespace Electric.SqlSugarCore.Repository.Identity
{
    /// <summary>
    /// 权限仓储
    /// </summary>
    public class SqlSugarCorePermissionRepository : SqlSugarCoreRepository<ElePermissionPo, ElePermission, Guid>, IPermissionRepository
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqlSugarMapper"></param>
        /// <param name="unitOfWork"></param>
        public SqlSugarCorePermissionRepository(ISqlSugarClient db, SqlSugarMapper sqlSugarMapper, IUnitOfWork unitOfWork) : base(db, sqlSugarMapper, unitOfWork)
        {
        }

        /// <summary>
        /// 根据权限编码，获取权限记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ElePermission> FindByCodeAsync(string code, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(code, nameof(code));

            var query = _db.Queryable<ElePermissionPo>().IncludeDetails(includeDetails);

            var elePermissionPo = await query.FirstAsync(x => code.Equals(x.Code, StringComparison.Ordinal), cancellationToken);

            return _sqlSugarMapper.Map<ElePermission>(elePermissionPo);
        }

        /// <summary>
        /// 根据权限Id列表，获取权限列表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ElePermission>> GetListAsync(List<Guid> ids, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(ids, nameof(ids));

            var query = _db.Queryable<ElePermissionPo>().IncludeDetails(includeDetails);

            var list = await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<ElePermission>>(list);
        }
    }
}
