using Electric.Domain.Entitys.Identity;

namespace Electric.Domain.Repository.Identity
{
    public interface IPermissionRepository : IBasicRepository<ElePermission, Guid>
    {
        /// <summary>
        /// 根据权限Id列表，获取权限列表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ElePermission>> GetListAsync(List<Guid> ids, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据权限编码，获取权限记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ElePermission> FindByCodeAsync(string Code, bool includeDetails = true, CancellationToken cancellationToken = default);
    }
}
