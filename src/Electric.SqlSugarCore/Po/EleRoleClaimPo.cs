using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    [SugarTable("EleRoleClaim")]
    public class EleRoleClaimPo : EntityKeyPo<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 声明类型
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// 类型值
        /// </summary>
        public string ClaimValue { get; set; }
    }
}