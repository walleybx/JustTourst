using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Application.Contracts.Dtos.Accounts
{
    public class LoginUserDto
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        public string? UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>

        [Required]
        public string? Password { get; set; }
    }
}
