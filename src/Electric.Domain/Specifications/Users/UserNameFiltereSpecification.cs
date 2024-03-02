using Electric.Core.Linq;
using Electric.Domain.Entitys.Identity;
using System.Linq.Expressions;

namespace Electric.Domain.Specifications.Users
{
    /// <summary>
    /// 用户名过滤规约
    /// </summary>
    public class UserNameFiltereSpecification : Specification<EleUser>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;
        public UserNameFiltereSpecification(string userName)
        {
            _userName = userName;
        }

        /// <summary>
        /// 获取表示当前规约的LINQ表达式
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<EleUser, bool>> ToExpression()
        {
            Expression<Func<EleUser, bool>> filter = PredicateBuilder.True<EleUser>();
            if (!string.IsNullOrEmpty(_userName))
            {
                filter = filter.Compose(x => x.UserName.StartsWith(_userName), Expression.AndAlso);
            }

            return filter;
        }
    }
}