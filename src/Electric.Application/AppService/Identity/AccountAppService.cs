using Electric.Application.AppService.Base;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Accounts;
using Electric.Core.Exceptions;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Manager.Identity;
using Electric.Domain.Repository.Identity;

namespace Electric.Application.AppService.Identity
{
    public class AccountAppService : BaseAppService, IAccountAppService
    {
        /// <summary>
        /// 用户管理器
        /// </summary>
        private UserManager _userManager;

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleRepository _roleRepository;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleRepository"></param>
        public AccountAppService(UserManager userManager, IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 获取当前登录用户的账号信息
        /// </summary>
        /// <returns></returns>
        public async Task<AccountDto> GetAsync()
        {
            //根据登录用户名，返回用户记录
            var user = await _userManager.FindByNameAsync(_eleSession.UserName);
            //获取用户分配的角色列表
            var roleNames = await _userManager.GetRolesAsync(user);

            //返回结果
            var accountDto = new AccountDto()
            {
                Roles = roleNames.ToArray(),
                Name = _eleSession.UserName,
                Avatar = "",
                Introduction = string.IsNullOrEmpty(user.FullName) ? _eleSession.UserName : user.FullName
            };

            return accountDto;
        }

        /// <summary>
        /// 获取当前登录用户的权限列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<AccountPermissionsDto>> GetPermissionsAsync()
        {
            //根据登录用户名，返回用户记录
            var user = await _userManager.FindByNameAsync(_eleSession.UserName);
            //获取用户分配的角色列表
            var roleNames = await _userManager.GetRolesAsync(user);

            //获取角色分配的权限列表
            var rolePermissions = new Dictionary<Guid, ElePermission>();
            foreach (var roleName in roleNames)
            {
                //获取角色列表
                var _permissions = await _roleRepository.GetRolePermissionsAsync(roleName);
                //合并角色重复的权限
                foreach (var permission in _permissions)
                {
                    if (!rolePermissions.ContainsKey(permission.Id))
                    {
                        rolePermissions.Add(permission.Id, permission);
                    }
                }
            }

            //返回权限列表
            var list = rolePermissions.Values.ToList();

            return _mapper.Map<List<AccountPermissionsDto>>(list);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountUpdatePassword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdatePasswordAnsync(AccountUpdatePasswordDto accountUpdatePassword)
        {
            //获取登录的用户名
            var user = await _userManager.FindByNameAsync(_eleSession.UserName);

            //修改密码
            var result = await _userManager.ChangePasswordAsync(user, accountUpdatePassword.OldPassword, accountUpdatePassword.NewPassword);
            
            //修改失败
            if (!result.Succeeded)
            {
                ThrowBusinessException(result, "修改密码失败！");
            }
        }
    }
}
