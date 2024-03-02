using Electric.Core.UOW;
using Electric.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Electric.EntityFrameworkCore.UOW
{
    /// <summary>
    /// EFCore工作单元
    /// </summary>
    public class EFCoreUnitOfWork : UnitOfWorkBase
    {
        /// <summary>
        /// 是否释放
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ApplicationDbContext Db { get; }

        /// <summary>
        /// 上下文事务
        /// </summary>
        private IDbContextTransaction? _dbContextTransaction = null;

        /// <summary>
        /// 是否提交
        /// </summary>
        private bool _isCommit = false;

        public EFCoreUnitOfWork(ApplicationDbContext db)
        {
            Db = db;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public override void BeginTransaction()
        {
            _dbContextTransaction = Db.Database.BeginTransaction();
            _isCommit = false;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public override void Commit()
        {
            if (_dbContextTransaction != null)
            {
                _dbContextTransaction.Commit();
            }
            _isCommit = true;
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 是否需要释放IDisposable的托管对象
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                if (_dbContextTransaction != null && !_isCommit)
                {
                    _dbContextTransaction.Rollback();
                }

                if (_dbContextTransaction != null)
                {
                    _dbContextTransaction.Dispose();
                }

                //资源已释放完毕，无需再调用析构函数
                GC.SuppressFinalize(this);
            }

            _isDisposed = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~EFCoreUnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// 保存所有
        /// </summary>
        public override void SaveChanges()
        {
            Db.SaveChanges();
        }
    }
}