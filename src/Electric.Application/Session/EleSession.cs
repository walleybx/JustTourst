using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Electric.Application.Session
{
    public class EleSession : IEleSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EleSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;
            }
        }

        /// <summary>
        /// 登录Id
        /// </summary>
        public Guid UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid) ?? string.Empty;
                return Guid.Parse(id);
            }
        }
    }
}