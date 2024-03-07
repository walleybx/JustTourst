using System.Linq.Expressions;

namespace Electric.SqlSugarCore.Repository
{
    /// <summary>
    /// Po转换类
    /// </summary>
    public class PoExpressionVisitor<TEntityPo> : ExpressionVisitor
    {
        /// <summary>
        /// 替换参数
        /// </summary>
        private ParameterExpression _parameter { get; set; }

        public PoExpressionVisitor()
        {
            var parameter = Expression.Parameter(typeof(TEntityPo), "a");
            _parameter = parameter;
        }

        /// <summary>
        /// 修改表达式
        /// </summary>
        /// <param name="expression">待修改的表达式</param>
        /// <returns>修改后的表达式</returns>
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        /// <summary>
        /// Lambda表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(node.Body, _parameter);
        }
    }
}