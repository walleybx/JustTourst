using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Domain.Model.Identity
{
    /// <summary>
    /// 用户角色关系表
    ///</summary>
    [SugarTable("UserRole")]
    public partial class TrsUserRole
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; protected set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
    }
}
