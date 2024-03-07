using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    public class EntityKeyPo<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]//数据库是自增才配自增 
        public TKey Id { get; set; }
    }

    public class EntityCreationPo<TKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(IsNullable = true)]

        public TKey? CreatorId { get; set; }
    }

    public class EntityPo<TKey> : EntityKeyPo<TKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public TKey? CreatorId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改者
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public TKey? LastModifierId { get; set; }
    }
}