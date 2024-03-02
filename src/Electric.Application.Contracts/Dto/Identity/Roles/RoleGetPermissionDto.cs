using Electric.Domain.Shared.Entitys.Identity;

namespace Electric.Application.Contracts.Dto.Identity.Roles
{
    /// <summary>
    /// 角色的权限列表
    /// </summary>
    public class RoleGetPermissionDto
    {
        /// <summary>
        /// 权限Id列表
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 菜单类型：菜单权限、元素权限、Api权限、数据权限
        /// </summary>
        public PermissionType PermissionType { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
