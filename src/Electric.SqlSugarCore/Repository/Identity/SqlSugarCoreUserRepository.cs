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
    /// 用户仓储
    /// </summary>
    public class SqlSugarCoreUserRepository : SqlSugarCoreRepository<EleUserPo, EleUser, Guid>, IUserRepository
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqlSugarMapper"></param>
        /// <param name="unitOfWork"></param>
        public SqlSugarCoreUserRepository(ISqlSugarClient db, SqlSugarMapper sqlSugarMapper, IUnitOfWork unitOfWork) : base(db, sqlSugarMapper, unitOfWork)
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

            var query = _db.Queryable<EleUserPo>().IncludeDetails(includeDetails);

            var eleUserDo = await query.FirstAsync(x => x.NormalizedUserName.Equals(userName), cancellationToken);

            return _sqlSugarMapper.Map<EleUser>(eleUserDo);
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

            var role = await _db.Queryable<EleRolePo>().Where(x => x.Name == roleName).FirstAsync(cancellationToken);

            if (role == null)
            {
                return new List<EleUser>();
            }

            var list = await _db.Queryable<EleUserPo>().IncludeDetails(includeDetails)
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToListAsync(cancellationToken);

            return _sqlSugarMapper.Map<List<EleUser>>(list);
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

            return await _db.Queryable<EleRolePo>()
                 .InnerJoin<EleUserRolePo>((r, ur) => r.Id == ur.RoleId && ur.UserId == id)
                 .Select<string>((r, ur) => r.Name)
                 .ToListAsync(cancellationToken);
        }
    }
}
