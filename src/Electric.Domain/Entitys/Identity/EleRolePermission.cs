using Electric.Domain.Entitys.Commons;

namespace Electric.Domain.Entitys.Identity
{
    /// <summary>
    /// 角色权限关联
    /// </summary>
    public class EleRolePermission : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; protected set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid PermissionId { get; protected set; }

        protected EleRolePermission() { }

        public EleRolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}