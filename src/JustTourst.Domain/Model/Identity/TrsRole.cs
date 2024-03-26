using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Domain.Model.Identity
{
    /// <summary>
    /// 权限
    /// </summary>
    [SugarTable("Role")]
    public class TrsRole
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; protected set; }

        /// <summary>
        /// 角色编码 
        ///</summary>
        [SugarColumn(ColumnName = "RoleCode")]
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// 逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 最后修改者
        /// </summary>
        public Guid? LastModifierId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; } = true;
    }
}
