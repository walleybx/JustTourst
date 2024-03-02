using System.ComponentModel.DataAnnotations;

namespace Electric.Application.Contracts.Dto.Identity.Auths
{
    public class AuthLoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
