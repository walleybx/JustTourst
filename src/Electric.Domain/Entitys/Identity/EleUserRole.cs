using Electric.Core;
using Electric.Domain.Entitys.Commons;

namespace Electric.Domain.Entitys.Identity
{
    /// <summary>
    /// 用户角色关联
    /// </summary>
    public class EleUserRole : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; protected set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; protected set; }

        protected EleUserRole() { }

        public EleUserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}