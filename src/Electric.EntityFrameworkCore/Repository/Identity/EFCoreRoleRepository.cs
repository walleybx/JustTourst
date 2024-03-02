using Electric.Core;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Microsoft.EntityFrameworkCore;

namespace Electric.EntityFrameworkCore.Repository.Identity
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class EFCoreRoleRepository : EfCoreRepository<EleRole, Guid>, IRoleRepository
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        public EFCoreRoleRepository(ApplicationDbContext db) : base(db)
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

            var query = _db.Set<EleRole>().IncludeDetails(includeDetails);

            return await query.FirstOrDefaultAsync(x => x.Name.Equals(roleName), cancellationToken);
        }

        /// <summary>
        /// 根据角色名称获取权限列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ElePermission>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken = default)
        {
            //校验参数
            Check.NotNull(roleName, nameof(roleName));

            //返回权限列表
            return await(from r in _db.Roles
                         join rp in _db.RolePermissions on r.Id equals rp.RoleId
                         join p in _db.Permissions on rp.PermissionId equals p.Id
                         where r.Name == roleName
                         select p).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据角色名称列表，获取角色列表
        /// </summary>
        /// <param name="roleNames"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<EleRole>> GetListByNamesAsync(List<string> roleNames, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleNames, nameof(roleNames));

            return await _db.Roles.IncludeDetails(includeDetails).Where(x => roleNames.Contains(x.Name)).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据角色Id，获取角色的权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ElePermission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleId, nameof(roleId));

            return await (from r in _db.Roles
                          join rp in _db.RolePermissions on r.Id equals rp.RoleId
                          join p in _db.Permissions on rp.PermissionId equals p.Id
                          where r.Id == roleId
                          select p).ToListAsync(cancellationToken);
        }
    }
}
