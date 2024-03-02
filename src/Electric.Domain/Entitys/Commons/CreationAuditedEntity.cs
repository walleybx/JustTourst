namespace Electric.Domain.Entitys.Commons
{
    /// <summary>
    /// 包含创建审计信息的实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class CreationAuditedEntity<TKey> : Entity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; protected set; }

        /// <summary>
        /// 创建者
        /// </summary>

        public TKey? CreatorId { get; set; }

        protected CreationAuditedEntity()
        {
            CreationTime = DateTime.Now;
        }
    }
}