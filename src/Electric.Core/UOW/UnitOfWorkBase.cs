namespace Electric.Core.UOW
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        public abstract void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        public abstract void Commit();


        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <returns></returns>
        public Task CommitAsync()
        {
            return Task.Run(Commit);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 保存所有更新
        /// </summary>
        public abstract void SaveChanges();

        /// <summary>
        /// 异步保存所有更新
        /// </summary>
        /// <returns></returns>
        public Task SaveChangesAsync()
        {
            return Task.Run(SaveChanges);
        }
    }
}
