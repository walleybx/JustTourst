using Electric.Application.Contracts.Dto.Identity.Commons;

namespace Electric.Application.Contracts.Dto.Identity.Roles
{
    /// <summary>
    /// 角色翻页响应对象
    /// </summary>
    public class RolePageResponseDto : PageResponseDto
    {
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<RoleDto> Roles { get; set; }
    }
}
