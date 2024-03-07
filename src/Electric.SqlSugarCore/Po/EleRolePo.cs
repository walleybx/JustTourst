using Electric.Domain.Shared.Entitys.Identity;
using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    [SugarTable("EleRole")]
    [SugarIndex("Index_Name", nameof(Name), OrderByType.Asc)]
    [SugarIndex("Index_NormalizedName", nameof(NormalizedName), OrderByType.Asc)]
    public class EleRolePo : EntityPo<Guid>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 标准化角色名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string NormalizedName { get; set; }

        /// <summary>
        /// 一个随机值，只要角色被持久化到存储中，该值就应该更改
        /// </summary>
        [SugarColumn(Length = 50)]
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// 状态，0：禁用，1：正常
        /// </summary>
        public RoleStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Remark { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(EleRoleClaimPo.RoleId), nameof(Id))]
        /// <summary>
        /// 声明列表
        /// </summary>
        public List<EleRoleClaimPo> Claims { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(EleRolePermissionPo.RoleId), nameof(Id))]
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<EleRolePermissionPo> Permissions { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(EleUserRolePo.RoleId), nameof(Id))]
        public List<EleUserRolePo> Roles { get; set; }
    }
}
