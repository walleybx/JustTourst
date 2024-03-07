using Electric.Domain.Shared.Entitys.Identity;
using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    [SugarTable("ElePermission")]
    [SugarIndex("Index_Code", nameof(Code), OrderByType.Asc, IsUnique = true)]
    public class ElePermissionPo : EntityPo<Guid>
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Url { get; set; }

        /// <summary>
        /// Vue页面组件
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Component { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Icon { get; set; }

        /// <summary>
        /// 菜单类型：菜单权限、元素权限、Api权限、数据权限
        /// </summary>
        public PermissionType PermissionType { get; set; }

        /// <summary>
        /// API方法
        /// </summary>
        public string ApiMethod { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 状态，0：禁用，1：正常
        /// </summary>
        public PermissionStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Remark { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(EleRolePermissionPo.RoleId), nameof(Id))]
        public List<EleRolePermissionPo> Roles { get; set; }
    }
}