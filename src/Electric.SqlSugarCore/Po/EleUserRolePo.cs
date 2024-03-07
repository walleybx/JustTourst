using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    /// <summary>
    /// 用户角色关联
    /// </summary>
    [SugarTable("EleUserRole")]
    public class EleUserRolePo
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid RoleId { get; set; }
    }
}