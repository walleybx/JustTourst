using System.Linq.Expressions;
using Electric.Domain.Entitys.Commons;

namespace Electric.Domain.Specifications
{
    /// <summary>
    /// 规约接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> where T : Entity
    {
        /// <summary>
        /// 用来验证模型是否满足规约要求
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsSatisfiedBy(T obj);

        /// <summary>
        /// 获取表示当前规约的LINQ表达式。
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> ToExpression();
    }
}
