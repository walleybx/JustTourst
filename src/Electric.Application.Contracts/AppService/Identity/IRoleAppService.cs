using Electric.Application.Contracts.Dto.Identity.Roles;

namespace Electric.Application.Contracts.AppService.Identity
{
    public interface IRoleAppService
    {
        /// <summary>
        /// 根据角色名称，分页返回角色列表
        /// </summary>
        /// <param name="rolePageRequestDto"></param>
        /// <returns></returns>
        public Task<RolePageResponseDto> GetPagedListAsync(RolePageRequestDto rolePageRequestDto);

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns></returns>
        public Task<List<RoleDto>> GetAllAsync();

        /// <summary>
        /// 保存角色的权限列表
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <param name="roleSavePermissionDto">以,分割权限Id</param>
        /// <returns></returns>
        public Task SavePermissionsAsync(Guid id, RoleSavePermissionDto roleSavePermissionDto);

        /// <summary>
        /// 获取角色的权限列表
        /// </summary>
        /// <returns></returns>
        public Task<List<RoleGetPermissionDto>> GetPermissionsAsync(Guid id);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleCreateDto"></param>
        public Task<RoleDto> InsertAsync(RoleCreateDto roleCreateDto);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleUpdateDto"></param>
        public Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto roleUpdateDto);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        public Task DeleteAsync(Guid id);
    }
}
