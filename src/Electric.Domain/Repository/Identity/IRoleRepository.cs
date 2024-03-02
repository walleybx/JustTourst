using Electric.Domain.Entitys.Identity;

namespace Electric.Domain.Repository.Identity
{
    public interface IRoleRepository : IBasicRepository<EleRole, Guid>
    {
        /// <summary>
        /// 根据角色名称获取角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EleRole> FindByNameAsync(string roleName, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据角色名称获取权限列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<ElePermission>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据角色名称列表，获取角色列表
        /// </summary>
        /// <param name="roleNames"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<EleRole>> GetListByNamesAsync(List<string> roleNames, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据角色Id，获取角色的权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<ElePermission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}
