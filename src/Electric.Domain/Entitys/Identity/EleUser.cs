using Electric.Core;
using Electric.Domain.Entitys.Commons;
using Electric.Domain.Shared.Entitys.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Electric.Domain.Entitys.Identity
{
    public class EleUser : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// 标准化用户名
        /// </summary>
        public string NormalizedUserName { get; protected internal set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; protected set; }

        /// <summary>
        /// 标准化Email 
        /// </summary>
        public string? NormalizedEmail { get; protected set; }

        /// <summary>
        /// 邮箱是否确认
        /// </summary>
        public bool EmailConfirmed { get; protected set; }

        /// <summary>
        /// 密码哈希
        /// </summary>
        public string PasswordHash { get; protected set; }

        /// <summary>
        /// 一个随机值，每当用户凭据更改时（密码更改、登录删除），该值都必须更改
        /// </summary>
        public string SecurityStamp { get; protected set; }

        /// <summary>
        /// 一个随机值，每当用户被持久化到存储时，该值必须更改
        /// </summary>
        public string ConcurrencyStamp { get; protected set; }

        /// <summary>
        /// 获取或设置用户的电话号码。
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置一个标志，该标志指示用户是否已确认其电话地址。
        /// </summary>
        public bool PhoneNumberConfirmed { get; protected set; }

        /// <summary>
        /// 获取或设置一个标志，该标志指示是否为此用户启用了双因素身份验证。
        /// </summary>
        public bool TwoFactorEnabled { get; protected set; }

        /// <summary>
        /// 获取或设置任何用户锁定结束的日期和时间（以UTC为单位）。
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; protected set; }

        /// <summary>
        /// 获取或设置一个标志，该标志指示用户是否可以被锁定。
        /// </summary>
        public bool LockoutEnabled { get; protected set; }

        /// <summary>
        /// 获取或设置当前用户登录尝试失败的次数。
        /// </summary>
        public int AccessFailedCount { get; protected set; }

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
        public List<EleUserRole> Roles { get; protected set; }

        /// <summary>
        /// 用户声明列表
        /// </summary>
        public List<EleUserClaim> Claims { get; protected set; }

        /// <summary>
        /// 用户登录列表
        /// </summary>
        public ICollection<EleUserLogin> Logins { get; protected set; }

        /// <summary>
        /// token列表
        /// </summary>
        public List<EleUserToken> Tokens { get; protected set; }

        protected EleUser()
        {
        }

        public EleUser(Guid id, string userName, string? email = null, string? fullName = null, string? remark = null) : base(id)
        {
            Check.NotNull(userName, nameof(userName));

            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
            Email = email ?? string.Empty;
            FullName = fullName;
            NormalizedEmail = email?.ToUpperInvariant() ?? string.Empty;
            EmailConfirmed = true;
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            SecurityStamp = Guid.NewGuid().ToString();
            Status = UserStatus.Normal;
            Remark = remark;

            Roles = new List<EleUserRole>();
            Claims = new List<EleUserClaim>();
            Logins = new List<EleUserLogin>();
            Tokens = new List<EleUserToken>();
        }

        public void AddRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            Roles.Add(new EleUserRole(Id, roleId));
        }

        public void RemoveRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (!IsInRole(roleId))
            {
                return;
            }

            Roles.RemoveAll(r => r.RoleId == roleId);
        }

        public void RemoveAllRoles()
        {
            Roles = new List<EleUserRole>();
        }

        public bool IsInRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return Roles.Any(r => r.RoleId == roleId);
        }

        public void SetUserName(string userName)
        {
            Check.NotNull(userName, nameof(userName));

            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
        }

        public void SetEmail(string email)
        {
            Email = email;
            NormalizedEmail = email?.ToUpperInvariant();
        }

        public void SetPasswordHash(string passwordHash)
        {
            Check.NotNull(passwordHash, nameof(passwordHash));

            var ph = new PasswordHasher<EleUser>();
            PasswordHash = ph.HashPassword(this, passwordHash);
        }

        public void AddClaim(Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            if (!Claims.Any(x => x.UserId == Id && x.ClaimType == claim.Type))
            {
                Claims.Add(new EleUserClaim(Guid.NewGuid(), Id, claim));
            }
        }

        public void RemoveClaim(Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(x => x.UserId == Id && x.ClaimType == claim.Type);
        }

        public void AddLogin(UserLoginInfo login)
        {
            Check.NotNull(login, nameof(login));

            Logins.Add(new EleUserLogin(login, Id));
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            Logins.RemoveAll(userLogin =>
                userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
        }

        public EleUserToken FindToken(string loginProvider, string name)
        {
            return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public void SetToken(string loginProvider, string name, string value)
        {
            var token = FindToken(loginProvider, name);
            if (token == null)
            {
                Tokens.Add(new EleUserToken(Id, loginProvider, name, value));
            }
            else
            {
                token.SetValue(value);
            }
        }

        public void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }
    }
}