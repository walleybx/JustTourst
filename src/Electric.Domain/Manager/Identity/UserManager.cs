using Electric.Core.Exceptions;
using Electric.Core.UOW;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Electric.Domain.Specifications.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Electric.Domain.Manager.Identity
{
    public class UserManager : UserManager<EleUser>, IDomainService
    {
        private UserStore _userStore;

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleRepository _roleRepository;

        /// <summary>
        /// 用户
        /// </summary>
        private IUserRepository _userRepository;

        public UserManager(UserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<EleUser> passwordHasher,
            IEnumerable<IUserValidator<EleUser>> userValidators,
            IEnumerable<IPasswordValidator<EleUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<EleUser>> logger,
            IRoleRepository roleRepository,
            IUserRepository userRepository
            ) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userStore = store;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 设置用户的角色列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        public async Task SetRolesAsync(EleUser user, List<string> roles)
        {
            var roleNames = await this.GetRolesAsync(user);

            //待删除的角色
            var removeRoleNames = new List<string>();
            foreach (var role in roleNames)
            {
                if (!roles.Contains(role))
                {
                    removeRoleNames.Add(role);
                }
            }

            //移除角色
            var roleList = await _roleRepository.GetListByNamesAsync(removeRoleNames, false);
            foreach (var role in roleList)
            {
                user.RemoveRole(role.Id);
            }

            //新增角色
            var roleNewList = await _roleRepository.GetListByNamesAsync(roles, false);
            foreach (var role in roleNewList)
            {
                user.AddRole(role.Id);
            }
        }

        /// <summary>
        /// 根据用户名搜索用户列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="page"></param>
        /// <param name="prePage"></param>
        /// <returns></returns>
        public async Task<List<EleUser>> GetListAsync(string userName, int page, int prePage)
        {
            //搜索条件
            var specification = new UserNameFiltereSpecification(userName);

            //返回用户列表
            var skipCount = (page <= 0 ? 0 : page - 1) * prePage;
            var users = await _userRepository.GetListAsync(specification, skipCount, prePage, includeDetails: true, sorting: nameof(EleUser.CreationTime) + " desc");

            return users;
        }

        /// <summary>
        /// 根据用户名搜索返回总记录数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(string userName)
        {
            //搜索条件
            var specification = new UserNameFiltereSpecification(userName);

            //返回用户列表的总记录数
            var total = await _userRepository.GetCountAsync(specification);

            return total;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteAsync(Guid id)
        {
            var user = await _userRepository.FindAsync(id);

            if (user == null)
            {

            }

            await _userRepository.DeleteAsync(id);

            return IdentityResult.Success;
        }
    }
}
