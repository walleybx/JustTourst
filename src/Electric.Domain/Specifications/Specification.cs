using Electric.Domain.Entitys.Commons;
using System.Linq.Expressions;

namespace Electric.Domain.Specifications
{
    /// <summary>
    /// 规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Specification<T> : ISpecification<T> where T : Entity
    {
        /// <summary>
        /// 用来验证模型是否满足规约要求
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return ToExpression().Compile()(obj);
        }

        /// <summary>
        /// 获取表示当前规范的LINQ表达式
        /// </summary>
        /// <returns></returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// 将规范隐式转换为表达式。
        /// </summary>
        /// <param name="specification"></param>
        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.ToExpression();
        }
    }
}