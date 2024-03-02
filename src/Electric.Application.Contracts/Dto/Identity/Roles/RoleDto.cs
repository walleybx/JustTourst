using Electric.Domain.Shared.Entitys.Identity;
using System.ComponentModel.DataAnnotations;

namespace Electric.Application.Contracts.Dto.Identity.Roles
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string? Creator { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastModificationTime { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 状态，0：禁用，1：正常
        /// </summary>
        public RoleStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string? Remark { get; set; }
    }
}
