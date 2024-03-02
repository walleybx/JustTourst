using Electric.Core;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Microsoft.AspNetCore.Identity;

namespace Electric.Domain.Manager.Identity
{
    public class UserStore : IUserStore<EleUser>, IUserPasswordStore<EleUser>, IUserRoleStore<EleUser>
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public bool AutoSaveChanges { get; set; } = true;
        public UserStore(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await _userRepository.InsertAsync(user, AutoSaveChanges, cancellationToken);

            return IdentityResult.Success;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await _userRepository.DeleteAsync(user, AutoSaveChanges, cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 根据用户Id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EleUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(userId, nameof(userId));

            return await _userRepository.FindAsync(Guid.Parse(userId), AutoSaveChanges, cancellationToken);
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EleUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(normalizedUserName, nameof(normalizedUserName));

            return await _userRepository.FindByNameAsync(normalizedUserName, AutoSaveChanges, cancellationToken);
        }

        /// <summary>
        /// 根据格式的用户名获取用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        /// <summary>
        /// 获取密码哈希值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string?> GetPasswordHashAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// 判断用户是否有密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            return Task.FromResult(string.IsNullOrEmpty(user.PasswordHash));
        }

        /// <summary>
        /// 格式化用户名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(EleUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(EleUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            user.SetPasswordHash(passwordHash);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetUserNameAsync(EleUser user, string? userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            user.SetUserName(userName);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            user.LastModificationTime = DateTime.Now;
            await _userRepository.UpdateAsync(user, AutoSaveChanges, cancellationToken);

            return IdentityResult.Success;
        }

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AddToRoleAsync(EleUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(roleName, nameof(roleName));

            var role = await _roleRepository.FindByNameAsync(roleName, cancellationToken: cancellationToken);
            if (role == null)
            {
                throw new Exception("角色不存在，角色名称：" + roleName);
            }

            user.AddRole(role.Id);
        }

        /// <summary>
        /// 移除用户的角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RemoveFromRoleAsync(EleUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(roleName, nameof(roleName));

            var role = await _roleRepository.FindByNameAsync(roleName, cancellationToken: cancellationToken);
            if (role == null)
            {
                return;
            }

            user.RemoveRole(role.Id);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetRolesAsync(EleUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            var roleNames = await _userRepository.GetRoleNamesAsync(user.Id, cancellationToken);

            return roleNames;
        }

        /// <summary>
        /// 判断用户是否包含某角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(EleUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));
            Check.NotNull(roleName, nameof(roleName));

            var roles = await GetRolesAsync(user, cancellationToken);

            return roles.Contains(roleName);
        }

        /// <summary>
        /// 根据角色名获取用户列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<EleUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(roleName, nameof(roleName));

            return await _userRepository.GetListByRoleNameAsync(roleName, cancellationToken: cancellationToken);
        }
    }
}
