using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    [SugarTable("EleUserClaim")]
    public class EleUserClaimPo : EntityKeyPo<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 声明类型
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// 声明类型的值
        /// </summary>
        public string ClaimValue { get; set; }
    }
}