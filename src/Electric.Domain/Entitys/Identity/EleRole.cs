using Electric.Core;
using Electric.Domain.Entitys.Commons;
using Electric.Domain.Shared.Entitys.Identity;
using System.Security.Claims;

namespace Electric.Domain.Entitys.Identity
{
    public class EleRole : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 标准化角色名称
        /// </summary>
        public string NormalizedName { get; internal set; }

        /// <summary>
        /// 一个随机值，只要角色被持久化到存储中，该值就应该更改
        /// </summary>
        public string ConcurrencyStamp { get; protected set; }

        /// <summary>
        /// 状态，0：禁用，1：正常
        /// </summary>
        public RoleStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 声明列表
        /// </summary>
        public List<EleRoleClaim> Claims { get; protected set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<EleRolePermission> Permissions { get; protected set; }

        protected EleRole()
        {
        }

        public EleRole(Guid id, string name) : base(id)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
            NormalizedName = name.ToUpperInvariant();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            Status = RoleStatus.Normal;

            Claims = new List<EleRoleClaim>();
            Permissions = new List<EleRolePermission>();
        }

        public void SetName(string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
            NormalizedName = name.ToUpperInvariant();
        }

        public void AddClaim(Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            if (!Claims.Any(x => x.RoleId == Id && x.ClaimType == claim.Type))
            {
                Claims.Add(new EleRoleClaim(Guid.NewGuid(), Id, claim));
            }
        }

        public void RemoveClaim(Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(x => x.RoleId == Id && x.ClaimType == claim.Type);
        }

        public void AddPermission(Guid permissionId)
        {
            Permissions.Add(new EleRolePermission(Id, permissionId));
        }

        public void RemovePermission(Guid permissionId)
        {
            Permissions.RemoveAll(x => x.RoleId == Id && x.PermissionId == permissionId);
        }

        public void RemoveAllPermission()
        {
            Permissions = new List<EleRolePermission>();
        }
    }
}