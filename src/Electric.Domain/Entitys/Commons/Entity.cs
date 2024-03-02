namespace Electric.Domain.Entitys.Commons
{
    /// <summary>
    /// 实体
    /// </summary>
    public interface IEntity
    {

    }
    /// <summary>
    /// 实体
    /// </summary>
    public class Entity : IEntity
    {

    }

    /// <summary>
    /// 带主键的实体
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class Entity<TKey> : Entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public TKey Id { get; protected set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected Entity()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected Entity(TKey id)
        {
            Id = id;
        }
    }
}