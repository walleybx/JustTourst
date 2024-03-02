using Electric.Core;
using Electric.Domain.Entitys.Commons;
using System.Security.Claims;

namespace Electric.Domain.Entitys.Identity
{
    public class EleUserClaim : Entity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; protected set; }

        /// <summary>
        /// 声明类型
        /// </summary>
        public string ClaimType { get; protected set; }

        /// <summary>
        /// 声明类型的值
        /// </summary>
        public string ClaimValue { get; protected set; }

        protected EleUserClaim()
        {

        }

        public EleUserClaim(Guid id, Guid userId, Claim claim) : base(id)
        {
            Check.NotNull(claim, nameof(claim));

            UserId = userId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public void SetClaimValue(string claimValue)
        {
            Check.NotNull(claimValue, nameof(claimValue));

            ClaimValue = claimValue;
        }
    }
}