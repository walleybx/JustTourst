using Electric.Core;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Microsoft.EntityFrameworkCore;

namespace Electric.EntityFrameworkCore.Repository.Identity
{
    /// 权限仓储
    /// </summary>
    public class EFCorePermissionRepository : EfCoreRepository<ElePermission, Guid>, IPermissionRepository
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        public EFCorePermissionRepository(ApplicationDbContext db) : base(db)
        {
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

            var query = _db.Set<ElePermission>().IncludeDetails(includeDetails);

            return await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
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

            var query = _db.Set<ElePermission>().IncludeDetails(includeDetails);

            return await query.FirstOrDefaultAsync(x => code.Equals(x.Code), cancellationToken);
        }
    }
}