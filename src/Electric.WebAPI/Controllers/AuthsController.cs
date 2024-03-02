using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Auths;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electric.WebAPI.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private IAuthAppService _authService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="authAppService"></param>
        public AuthsController(IAuthAppService authAppService)
        {
            _authService = authAppService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="authLoginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto authLoginDto)
        {
            var token = await _authService.LoginAsync(authLoginDto);

            //返回token
            return Ok(token);
        }
    }
}
