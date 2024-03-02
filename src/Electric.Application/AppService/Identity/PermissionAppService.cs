using Electric.Application.AppService.Base;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Permissions;
using Electric.Core.Exceptions;
using Electric.Core;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Electric.Domain.Shared.Entitys.Identity;
using Electric.Domain.Manager.Identity;

namespace Electric.Application.AppService.Identity
{
    public class PermissionAppService : BaseAppService, IPermissionAppService
    {
        /// <summary>
        /// 权限
        /// </summary>
        private IPermissionRepository _permissionRepository;

        /// <summary>
        /// 权限服务
        /// </summary>
        private PermissionManager _permissionManager;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="permissionRepository"></param>
        /// <param name="permissionManager"></param>
        public PermissionAppService(IPermissionRepository permissionRepository, PermissionManager permissionManager)
        {
            _permissionRepository = permissionRepository;
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permissionCreateDto"></param>
        /// <returns></returns>
        public async Task<PermissionDto> InsertAsync(PermissionCreateDto permissionCreateDto)
        {
            //权限API方法：字符串转为枚举类型
            var permissionApiMethod = PermissionApiMethod.GET;
            if (!string.IsNullOrEmpty(permissionCreateDto.ApiMethod))
            {
                permissionApiMethod = permissionCreateDto.ApiMethod.ToUpper().ToEnum<PermissionApiMethod>();
            }

            //创建权限实体
            var permission = new ElePermission(Guid.NewGuid(), permissionCreateDto.ParentId, permissionCreateDto.Name, permissionCreateDto.Code,
                permissionCreateDto.PermissionType, permissionApiMethod, permissionCreateDto.Status,
                permissionCreateDto.Icon, permissionCreateDto.Url, permissionCreateDto.Remark)
            {
                CreatorId = _eleSession.UserId
            };

            //保存
            permission = await _permissionManager.CreateAsync(permission);

            //提交所有更新
            _unitOfWork.SaveChanges();

            return _mapper.Map<PermissionDto>(permission);
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissionUpdateDto"></param>
        /// <returns></returns>
        public async Task<PermissionDto> UpdateAsync(Guid id, PermissionUpdateDto permissionUpdateDto)
        {
            //判断权限Id是否存在
            var entity = await _permissionRepository.FindAsync(id);
            if (entity == null)
            {
                throw new BusinessException("该权限Id不存在，Id:" + id.ToString());
            }

            //更新权限的信息
            entity.LastModifierId = _eleSession.UserId;
            entity.SetName(permissionUpdateDto.Name);
            entity.SetCode(permissionUpdateDto.Code);
            entity.Url = permissionUpdateDto.Url;
            entity.Component = permissionUpdateDto.Component;
            entity.Icon = permissionUpdateDto.Icon;
            entity.PermissionType = permissionUpdateDto.PermissionType;
            entity.SetApiMethod(permissionUpdateDto.ApiMethod.ToEnum<PermissionApiMethod>());
            entity.Sort = permissionUpdateDto.Sort;
            entity.Status = permissionUpdateDto.Status;
            entity.ParentId = permissionUpdateDto.ParentId;
            entity.Remark = permissionUpdateDto.Remark;

            //更新权限
            await _permissionManager.UpdateAsync(entity);

            //提交所有更新
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PermissionDto>(entity);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            //判断权限Id是否存在
            var permission = await _permissionRepository.FindAsync(id);
            if (permission == null)
            {
                return;
            }

            //删除权限
            await _permissionRepository.DeleteAsync(id);

            //提交所有更新
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionDto>> GetAllAsync()
        {
            var permissions = await _permissionRepository.GetListAsync();

            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}
