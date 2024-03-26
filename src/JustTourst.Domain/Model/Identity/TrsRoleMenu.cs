using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Domain.Model.Identity
{
    /// <summary>
    /// 角色菜单关系表
    ///</summary>
    [SugarTable("RoleMenu")]
    public partial class TrsRoleMenu
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; protected set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "RoleId")]
        public Guid RoleId { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "MenuId")]
        public Guid MenuId { get; set; }
    }
}
