using Electric.Core;
using Electric.Domain.Entitys.Commons;
using Electric.Domain.Shared.Entitys.Identity;

namespace Electric.Domain.Entitys.Identity
{
    public class ElePermission : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Vue页面组件
        /// </summary>
        public string? Component { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 菜单类型：菜单权限、元素权限、Api权限、数据权限
        /// </summary>
        public PermissionType PermissionType { get; set; }

        /// <summary>
        /// API方法
        /// </summary>
        public string ApiMethod { get; protected set; }

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
        public string? Remark { get; set; }

        protected ElePermission()
        {
        }

        public ElePermission(Guid id, Guid? parentId, string name, string code, PermissionType permissionType, PermissionApiMethod apiMethod,
            PermissionStatus permissionStatus, string? icon = null, string? url = null, string? remark = null) : base(id)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(code, nameof(code));

            ParentId = parentId;
            Name = name;
            Code = code;
            PermissionType = permissionType;
            ApiMethod = apiMethod.ToString();
            Status = permissionStatus;
            Sort = 0;
            Icon = icon;
            Url = url;
            Remark = remark;
        }

        /// <summary>
        /// 设置API方法
        /// </summary>
        /// <param name="apiMethod"></param>
        public void SetApiMethod(PermissionApiMethod apiMethod)
        {
            ApiMethod = apiMethod.ToString();
        }

        public void SetName(string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
        }

        public void SetCode(string code)
        {
            Check.NotNull(code, nameof(code));

            Code = code;
        }
    }
}