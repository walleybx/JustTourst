using Electric.Core;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;

namespace Electric.EntityFrameworkCore.Repository.Identity
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class EFCoreUserRepository : EfCoreRepository<EleUser, Guid>, IUserRepository
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        public EFCoreUserRepository(ApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// 根据用户名返回用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EleUser> FindByNameAsync(string userName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(userName, nameof(userName));

            var query = _db.Set<EleUser>().IncludeDetails(includeDetails);

            return await query.FirstOrDefaultAsync(x => x.UserName.Equals(userName), cancellationToken);
        }

        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<EleUser>> GetListByRoleNameAsync(string roleName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            Check.NotNull(roleName, nameof(roleName));

            var role = await _db.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync(cancellationToken);

            if (role == null)
            {
                return new List<EleUser>();
            }

            return await _db.Set<EleUser>().IncludeDetails(includeDetails)
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据用户Id，获取角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<string>> GetRoleNamesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Check.NotNull(id, nameof(id));

            var query = from userRole in _db.Set<EleUserRole>()
                        join role in _db.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role.Name;

            return await query.ToListAsync(cancellationToken);
        }
    }
}
