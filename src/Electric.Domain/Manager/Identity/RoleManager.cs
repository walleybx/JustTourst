using Electric.Core;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Electric.Domain.Specifications.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Electric.Domain.Manager.Identity
{
    public class RoleManager : RoleManager<EleRole>, IDomainService
    {
        private RoleStore _roleStore;

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleRepository _roleRepository;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="store"></param>
        /// <param name="roleValidators"></param>
        /// <param name="keyNormalizer"></param>
        /// <param name="errors"></param>
        /// <param name="logger"></param>
        /// <param name="roleRepository"></param>
        public RoleManager(RoleStore store, IEnumerable<IRoleValidator<EleRole>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager> logger,
            IRoleRepository roleRepository)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _roleStore = store;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 根据角色名搜索角色列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="page"></param>
        /// <param name="prePage"></param>
        /// <returns></returns>
        public async Task<List<EleRole>> GetListAsync(string roleName, int page, int prePage)
        {
            var specification = new RoleNameFiltereSpecification(roleName);

            //返回角色列表
            var skipCount = (page <= 0 ? 0 : page - 1) * prePage;
            var roles = await _roleRepository.GetListAsync(specification, skipCount, prePage, sorting: nameof(EleRole.CreationTime) + " desc");

            return roles;
        }

        /// <summary>
        /// 根据角色名搜索返回总记录数
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(string roleName)
        {
            var specification = new RoleNameFiltereSpecification(roleName);

            //返回角色列表
            var total = await _roleRepository.GetCountAsync(specification);

            return total;
        }

        /// <summary>
        /// 获取角色的权限
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<List<ElePermission>> GetPermissionsAsync(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return await _roleRepository.GetRolePermissionsAsync(roleId);
        }

        /// <summary>
        /// 设置角色的权限列表
        /// </summary>
        /// <param name="role"></param>
        /// <param name="permissionIds"></param>
        public void SetPermissions(EleRole role, List<Guid> permissionIds)
        {
            role.RemoveAllPermission();
            foreach (var permissionId in permissionIds)
            {
                role.AddPermission(permissionId);
            }
        }
    }
}