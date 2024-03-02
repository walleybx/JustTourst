using Electric.Domain.Entitys.Commons;
using Microsoft.EntityFrameworkCore;

namespace Electric.EntityFrameworkCore.Repository
{
    public static class EfCoreQueryableExtensions
    {
        /// <summary>
        /// 是否包含子集合记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> IncludeDetails<TEntity>(this IQueryable<TEntity> query, bool includeDetails) where TEntity : Entity
        {
            if (includeDetails)
            {
                //获取子集合
                var _properties = typeof(TEntity).GetProperties()
                    .Where(m => m.PropertyType.IsGenericType && m.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    .ToList();

                foreach (var property in _properties)
                {
                    query = query.Include(property.Name);
                }
            }

            return query;
        }
    }
}