using Electric.Domain.Entitys.Identity;

namespace Electric.Domain.Repository.Identity
{
    public interface IUserRepository : IBasicRepository<EleUser, Guid>
    {
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EleUser> FindByNameAsync(string userName, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据用户Id，获取角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<string>> GetRoleNamesAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<EleUser>> GetListByRoleNameAsync(string roleName, bool includeDetails = true, CancellationToken cancellationToken = default);
    }
}
