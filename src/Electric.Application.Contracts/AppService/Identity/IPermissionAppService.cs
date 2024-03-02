using Electric.Application.Contracts.Dto.Identity.Permissions;

namespace Electric.Application.Contracts.AppService.Identity
{
    public interface IPermissionAppService
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public Task<List<PermissionDto>> GetAllAsync();

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permissionCreateDto"></param>
        public Task<PermissionDto> InsertAsync(PermissionCreateDto permissionCreateDto);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissionUpdateDto"></param>
        public Task<PermissionDto> UpdateAsync(Guid id, PermissionUpdateDto permissionUpdateDto);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        public Task DeleteAsync(Guid id);
    }
}