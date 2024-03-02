using Electric.Application.Auth;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Electric.WebAPI.Controllers
{
    /// <summary>
    /// 账号
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EletricAuthorize]
    public class AccountsController : ControllerBase
    {
        private IAccountAppService _accountAppService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="accountAppService"></param>
        public AccountsController(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }


        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //返回结果
            var accountDto = await _accountAppService.GetAsync();

            return Ok(accountDto);
        }

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            //返回结果
            var list = await _accountAppService.GetPermissionsAsync();

            return Ok(list);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountUpdatePassword"></param>
        [HttpPatch("password")]
        public async Task<IActionResult> Put([FromBody] AccountUpdatePasswordDto accountUpdatePassword)
        {
            //返回结果
            await _accountAppService.UpdatePasswordAnsync(accountUpdatePassword);

            return NoContent();
        }
    }
}
