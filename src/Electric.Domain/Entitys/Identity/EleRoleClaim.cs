using Electric.Core;
using Electric.Domain.Entitys.Commons;
using System.Security.Claims;

namespace Electric.Domain.Entitys.Identity
{
    public class EleRoleClaim : Entity<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; protected set; }

        /// <summary>
        /// 声明类型
        /// </summary>
        public string ClaimType { get; protected set; }

        /// <summary>
        /// 类型值
        /// </summary>
        public string ClaimValue { get; protected set; }

        protected EleRoleClaim() { }
        public EleRoleClaim(Guid id, Guid roleId, Claim claim) : base(id)
        {
            Check.NotNull(claim, nameof(claim));

            RoleId = roleId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public void SetClaimValue(string claimValue)
        {
            if (claimValue == null)
            {
                throw new ArgumentNullException(nameof(claimValue));
            }

            ClaimValue = claimValue;
        }
    }
}