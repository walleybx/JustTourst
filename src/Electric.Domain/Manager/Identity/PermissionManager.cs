using Electric.Core;
using Electric.Core.Exceptions;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;

namespace Electric.Domain.Manager.Identity
{
    public class PermissionManager : IDomainService
    {
        private IPermissionRepository _permissionRepository;
        public PermissionManager(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="elePermission"></param>
        /// <returns></returns>
        public async Task<ElePermission> CreateAsync(ElePermission elePermission)
        {
            Check.NotNull(elePermission, nameof(elePermission));

            //判断权限编码是否存在
            var permission = await _permissionRepository.FindByCodeAsync(elePermission.Code);
            if (permission != null)
            {
                throw new BusinessException($"权限编码'{elePermission.Code}'已存在！");
            }

            return await _permissionRepository.InsertAsync(elePermission);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="elePermission"></param>
        /// <returns></returns>
        public async Task<ElePermission> UpdateAsync(ElePermission elePermission)
        {
            Check.NotNull(elePermission, nameof(elePermission));

            //判断权限编码是否存在
            var permission = await _permissionRepository.FindByCodeAsync(elePermission.Code);
            if (permission != null && permission.Id != elePermission.Id)
            {
                throw new BusinessException($"权限编码'{elePermission.Code}'已存在！");
            }

            return await _permissionRepository.UpdateAsync(elePermission);
        }
    }
}