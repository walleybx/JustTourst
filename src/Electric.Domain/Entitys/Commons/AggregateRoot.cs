namespace Electric.Domain.Entitys.Commons
{
    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class AggregateRoot<TKey> : Entity<TKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected AggregateRoot() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        protected AggregateRoot(TKey id) : base(id)
        {
        }
    }
}