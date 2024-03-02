namespace Electric.Domain.Entitys.Commons
{
    /// <summary>
    /// 包含创建审计信息的聚合根
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>
    {
        public DateTime CreationTime { get; protected set; }

        public TKey? CreatorId { get; set; }

        protected CreationAuditedAggregateRoot()
        {
        }

        protected CreationAuditedAggregateRoot(TKey id)
            : base(id)
        {
            CreationTime = DateTime.Now;
        }
    }
}