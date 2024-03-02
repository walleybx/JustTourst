using Electric.Application.Contracts.Dto.Identity.Commons;

namespace Electric.Application.Contracts.Dto.Identity.Roles
{
    /// <summary>
    /// 角色翻页查询
    /// </summary>
    public class RolePageRequestDto : PageRequestDto
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string? Name { get; set; }
    }
}
