using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    /// <summary>
    /// 角色权限关联
    /// </summary>
    [SugarTable("EleRolePermission")]
    public class EleRolePermissionPo : EntityCreationPo<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid PermissionId { get; set; }
    }
}