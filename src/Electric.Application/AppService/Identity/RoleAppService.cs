using Electric.Application.AppService.Base;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Roles;
using Electric.Core.Exceptions;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Manager.Identity;
using Electric.Domain.Repository.Identity;
using Microsoft.AspNetCore.Identity;

namespace Electric.Application.AppService.Identity
{
    public class RoleAppService : BaseAppService, IRoleAppService
    {
        /// <summary>
        /// 角色管理器
        /// </summary>
        private RoleManager _roleManager;

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleRepository _roleRepository;

        /// <summary>
        /// 用户管理器
        /// </summary>
        private UserManager _userManager;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="roleRepository"></param>
        /// <param name="userManager"></param>
        public RoleAppService(RoleManager roleManager, IRoleRepository roleRepository, UserManager userManager)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleCreateDto"></param>
        /// <returns></returns>
        public async Task<RoleDto> InsertAsync(RoleCreateDto roleCreateDto)
        {
            var role = new EleRole(Guid.NewGuid(), roleCreateDto.Name)
            {
                CreatorId = _eleSession.UserId,
                Status = roleCreateDto.Status,
                Remark = roleCreateDto.Remark
            };

            //保存
            var result = await _roleManager.CreateAsync(role);

            //新增失败
            if (!result.Succeeded)
            {
                ThrowBusinessException(result, "新增角色失败！");
            }

            return _mapper.Map<RoleDto>(role);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleUpdateDto"></param>
        /// <returns></returns>
        public async Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto roleUpdateDto)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new BusinessException("该角色Id不存在，Id:" + id.ToString());
            }

            role.SetName(roleUpdateDto.Name);
            role.Status = roleUpdateDto.Status;
            role.Remark = roleUpdateDto.Remark;

            var result = await _roleManager.UpdateAsync(role);

            //更新失败
            if (!result.Succeeded)
            {
                ThrowBusinessException(result, "更新角色失败！");
            }

            return _mapper.Map<RoleDto>(role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            //判断角色Id是否存在
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return;
            }

            await _roleRepository.DeleteAsync(id);

            //提交所有更新
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _roleRepository.GetListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        /// <summary>
        /// 角色搜索
        /// </summary>
        /// <param name="rolePageRequestDto"></param>
        /// <returns></returns>
        public async Task<RolePageResponseDto> GetPagedListAsync(RolePageRequestDto rolePageRequestDto)
        {
            //根据搜索条件，获取角色列表
            var roles = await _roleManager.GetListAsync(rolePageRequestDto.Name, rolePageRequestDto.Page, rolePageRequestDto.PrePage);
            //根据搜索条件，获取总记录数
            var total = await _roleManager.GetCountAsync(rolePageRequestDto.Name);

            //获取角色的创建者
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            foreach (var roleDto in roleDtos)
            {
                var role = roles.Find(x => x.Id == roleDto.Id);
                if (role.CreatorId != null && !default(Guid).Equals(role.CreatorId))
                {
                    roleDto.Creator = (await _userManager.FindByIdAsync(role.CreatorId.ToString()))?.UserName ?? string.Empty;
                }
            }

            //返回
            return new RolePageResponseDto()
            {
                Page = rolePageRequestDto.Page,
                PrePage = rolePageRequestDto.PrePage,
                Roles = roleDtos,
                Total = total
            };
        }

        /// <summary>
        /// 获取角色的权限列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<RoleGetPermissionDto>> GetPermissionsAsync(Guid id)
        {
            var permissions = await _roleManager.GetPermissionsAsync(id);

            return _mapper.Map<List<RoleGetPermissionDto>>(permissions);
        }

        /// <summary>
        /// 保存角色的权限列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolePermissionDto"></param>
        /// <returns></returns>
        public async Task SavePermissionsAsync(Guid id, RoleSavePermissionDto rolePermissionDto)
        {
            //判断角色Id是否存在
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new BusinessException("该角色Id不存在，Id:" + id.ToString());
            }

            //设置角色关联的权限列表
            _roleManager.SetPermissions(role, rolePermissionDto.PermissionIds);

            //更新角色
            var result = await _roleManager.UpdateAsync(role);

            //更新失败
            if (!result.Succeeded)
            {
                ThrowBusinessException(result, "更新角色的权限列表失败！");
            }
        }
    }
}
