using System.ComponentModel.DataAnnotations;

namespace Electric.Application.Contracts.Dto.Identity.Accounts
{
    /// <summary>
    /// 登录账号密码修改
    /// </summary>
    public class AccountUpdatePasswordDto
    {
        /// <summary>
        /// 原本密码
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
