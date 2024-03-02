using Electric.Application.Contracts.Dto.Identity.Accounts;

namespace Electric.Application.Contracts.AppService.Identity
{
    public interface IAccountAppService
    {
        /// <summary>
        /// 获取当前登录用户的账号信息
        /// </summary>
        /// <returns></returns>
        public Task<AccountDto> GetAsync();

        /// <summary>
        /// 获取当前登录用户的权限列表
        /// </summary>
        /// <returns></returns>
        public Task<List<AccountPermissionsDto>> GetPermissionsAsync();

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountUpdatePassword"></param>
        public Task UpdatePasswordAnsync(AccountUpdatePasswordDto accountUpdatePassword);
    }
}
