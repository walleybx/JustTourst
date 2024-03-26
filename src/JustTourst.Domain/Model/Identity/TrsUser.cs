using JustTourst.Domain.Shared.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Domain.Model.Identity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("User")]
    public class TrsUser
    {

        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; protected set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? Nick { get; set; }

        /// <summary>
        /// 邮箱号
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public long? Phone { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string? Introduction { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum Sex { get; set; } = SexEnum.Unknown;

        /// <summary>
        /// 个人权限
        /// </summary>
        public List<Guid>? RoleIds { get; set; }
        public List<Guid>? PostIds { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public Guid? DeptId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; } = true;
    }
}
