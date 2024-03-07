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
    /// 角色仓储
    /// </summary>
    public class SqlSugarCoreRoleRepository : SqlSugarCoreRepository<EleRolePo, EleRole, Guid>, IRoleRepository
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqlSugarMapper"></param>
        /// <param name="unitOfWork"></param>
        public SqlSugarCoreRoleRepository(ISqlSugarClient db, SqlSugarMapper sqlSugarMapper, IUnitOfWork unitOfWork) : base(db, sqlSugarMapper, unitOfWork)
        {
        }

        /// <summary>
        /// 根据角色名称获取角色记录
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EleRole> FindByNameAsync(string roleName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleName, nameof(roleName));

            var query = _db.Queryable<EleRolePo>().IncludeDetails(includeDetails);

            var entity = await query.FirstAsync(x => x.Name.Equals(roleName), cancellationToken);

            return _sqlSugarMapper.Map<EleRole>(entity);
        }

        /// <summary>
        /// 根据角色名列表，获取角色列表
        /// </summary>
        /// <param name="roleNames"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<EleRole>> GetListByNamesAsync(List<string> roleNames, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleNames, nameof(roleNames));

            var list = await _db.Queryable<EleRolePo>().IncludeDetails(includeDetails).Where(x => roleNames.Contains(x.Name)).ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<EleRole>>(list);
        }

        /// <summary>
        /// 获取角色的权限列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ElePermission>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleName, nameof(roleName));

            var list = await _db.Queryable<EleRolePermissionPo>()
                  .InnerJoin<EleRolePo>((rp, r) => rp.RoleId == r.Id && r.Name == roleName)
                  .InnerJoin<ElePermissionPo>((rp, r, e) => rp.PermissionId == e.Id)
                  .Select((rp, r, e) => e)
                  .ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<ElePermission>>(list);
        }

        /// <summary>
        /// 根据角色Id，获取关联的权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ElePermission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleId, nameof(roleId));

            var list = await _db.Queryable<EleRolePermissionPo>()
                 .InnerJoin<EleRolePo>((rp, r) => rp.RoleId == r.Id && r.Id == roleId)
                 .InnerJoin<ElePermissionPo>((rp, r, e) => rp.PermissionId == e.Id)
                  .Select((rp, r, e) => e)
                 .ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<ElePermission>>(list);
        }
    }
}
