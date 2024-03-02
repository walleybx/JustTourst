using Electric.Application.AppService.Base;
using Electric.Application.Auth;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Auths;
using Electric.Core.Exceptions;
using Electric.Domain.Manager.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Electric.Application.AppService.Identity
{
    public class AuthAppService : BaseAppService, IAuthAppService
    {
        /// <summary>
        /// JWT配置
        /// </summary>
        private readonly JwtBearerSetting _jwtBearerSetting;

        /// <summary>
        /// 用户管理器
        /// </summary>
        private readonly UserManager _userManager;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="jwtBearerSetting"></param>
        /// <param name="userManager"></param>
        public AuthAppService(JwtBearerSetting jwtBearerSetting, UserManager userManager)
        {
            _jwtBearerSetting = jwtBearerSetting;
            _userManager = userManager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="authLoginDto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<string> LoginAsync(AuthLoginDto authLoginDto)
        {
            //根据用户名、密码校验
            var user = await _userManager.FindByNameAsync(authLoginDto.UserName);
            if (user == null)
            {
                throw new BusinessException("登录失败，账号或密码错误");
            }

            var succeeded = await _userManager.CheckPasswordAsync(user, authLoginDto.Password);
            if (succeeded)
            {
                //定义JWT的Payload部分
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, authLoginDto.UserName),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                };

                //生成token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerSetting.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var securityToken = new JwtSecurityToken(
                    issuer: _jwtBearerSetting.Issuer,
                    audience: _jwtBearerSetting.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                //返回token
                return token;
            }
            else
            {
                throw new BusinessException("登录失败，账号或密码错误");
            }
        }
    }
}
