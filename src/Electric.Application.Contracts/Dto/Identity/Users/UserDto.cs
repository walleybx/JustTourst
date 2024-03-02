using Electric.Domain.Shared.Entitys.Identity;

namespace Electric.Application.Contracts.Dto.Identity.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 标准化用户名
        /// </summary>
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 标准化Email 
        /// </summary>
        public string? NormalizedEmail { get; set; }

        /// <summary>
        /// 邮箱是否确认
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 密码哈希
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 一个随机值，每当用户凭据更改时（密码更改、登录删除），该值都必须更改
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// 一个随机值，每当用户被持久化到存储时，该值必须更改
        /// </summary>
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// 获取或设置用户的电话号码。
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置一个标志，该标志指示用户是否已确认其电话地址。
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 获取或设置一个标志，该标志指示是否为此用户启用了双因素身份验证。
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 获取或设置任何用户锁定结束的日期和时间（以UTC为单位）。
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 获取或设置一个标志，该标志指示用户是否可以被锁定。
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 获取或设置当前用户登录尝试失败的次数。
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 全名：姓名
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// 状态，0：禁用，1：正常
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public ICollection<string> Roles { get; set; }
    }
}
