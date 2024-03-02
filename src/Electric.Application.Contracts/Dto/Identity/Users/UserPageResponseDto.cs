using Electric.Application.Contracts.Dto.Identity.Commons;

namespace Electric.Application.Contracts.Dto.Identity.Users
{
    /// <summary>
    /// 用户翻页响应对象
    /// </summary>
    public class UserPageResponseDto : PageResponseDto
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserDto> Users { get; set; }
    }
}
