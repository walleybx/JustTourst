namespace Electric.Domain.Entitys.Commons
{
    /// <summary>
    /// 包含审计信息的聚合根
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>
    {
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改者
        /// </summary>

        public TKey? LastModifierId { get; set; }

        protected AuditedAggregateRoot()
        {
        }

        protected AuditedAggregateRoot(TKey id)
            : base(id)
        {
        }
    }
}