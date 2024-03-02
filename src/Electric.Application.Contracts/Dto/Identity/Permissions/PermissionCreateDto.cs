using Electric.Domain.Shared.Entitys.Identity;
using System.ComponentModel.DataAnnotations;

namespace Electric.Application.Contracts.Dto.Identity.Permissions
{
    /// <summary>
    /// 创建权限对象
    /// </summary>
    public class PermissionCreateDto
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 权限编码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Url地址
        /// </summary>
        [MaxLength(200)]
        public string? Url { get; set; }

        /// <summary>
        /// Vue页面组件
        /// </summary>
        [MaxLength(200)]
        public string? Component { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(100)]
        public string? Icon { get; set; }

        /// <summary>
        /// 菜单类型：菜单权限、元素权限、Api权限、数据权限
        /// </summary>
        public PermissionType PermissionType { get; set; }

        /// <summary>
        /// API方法
        /// </summary>
        [MaxLength(50)]
        public string? ApiMethod { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 状态，0：禁用，1：正常
        /// </summary>
        public PermissionStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string? Remark { get; set; }
    }
}
