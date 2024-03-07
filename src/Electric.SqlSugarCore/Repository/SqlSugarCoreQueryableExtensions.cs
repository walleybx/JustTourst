using SqlSugar;

namespace Electric.SqlSugarCore.Repository
{
    public static class SqlSugarCoreQueryableExtensions
    {
        /// <summary>
        /// 是否包含子集合记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public static ISugarQueryable<TEntity> IncludeDetails<TEntity>(this ISugarQueryable<TEntity> query, bool includeDetails)
        {
            if (includeDetails)
            {
                //获取子集合
                var _properties = typeof(TEntity).GetProperties().Where(m =>
                m.PropertyType.IsGenericType &&
                m.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
                ).ToList();

                var ignoreProperyNameList = new List<string>();
                foreach (var property in _properties)
                {
                    query = query.IncludesByNameString(property.Name);
                }
            }

            return query;
        }
    }
}
