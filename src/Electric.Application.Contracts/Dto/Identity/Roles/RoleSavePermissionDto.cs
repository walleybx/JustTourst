namespace Electric.Application.Contracts.Dto.Identity.Roles
{
    /// <summary>
    /// 角色分配权限
    /// </summary>
    public class RoleSavePermissionDto
    {
        /// <summary>
        /// 权限Id列表
        /// </summary>
        public List<Guid> PermissionIds { get; set; }
    }
}
