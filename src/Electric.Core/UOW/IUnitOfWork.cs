namespace Electric.Core.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 异步提交事务
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// 保存所有更新
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 异步保存所有更新
        /// </summary>
        Task SaveChangesAsync();
    }
}