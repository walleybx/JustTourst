using Electric.Core.Linq;
using Electric.Domain.Entitys.Identity;
using System.Linq.Expressions;

namespace Electric.Domain.Specifications.Roles
{
    /// <summary>
    /// 角色名称过滤规约
    /// </summary>
    public class RoleNameFiltereSpecification : Specification<EleRole>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        private string _roleName;
        public RoleNameFiltereSpecification(string roleName)
        {
            _roleName = roleName;
        }

        /// <summary>
        /// 获取表示当前规约的LINQ表达式
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<EleRole, bool>> ToExpression()
        {
            Expression<Func<EleRole, bool>> filter = PredicateBuilder.True<EleRole>();
            if (!string.IsNullOrEmpty(_roleName))
            {
                filter = filter.Compose(x => x.Name.StartsWith(_roleName), Expression.AndAlso);
            }

            return filter;
        }
    }
}
