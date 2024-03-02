namespace Electric.Core
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// 从集合移除给定条件的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IList<T> RemoveAll<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
            List<T> list = source.Where(predicate).ToList();
            foreach (T item in list)
            {
                source.Remove(item);
            }

            return list;
        }
    }
}