using Electric.Core.UOW;
using SqlSugar;

namespace Electric.SqlSugarCore.UOW
{
    /// <summary>
    /// SqlSugarCore工作单元
    /// </summary>
    public class SqlSugarCoreUnitOfWork : UnitOfWorkBase
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarClient _sqlSugarClient;

        /// <summary>
        /// 上下文事务
        /// </summary>
        private SugarUnitOfWork? sugarUnitOfWork = null;

        /// <summary>
        /// 是否释放
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// 导航更新列表
        /// </summary>
        private List<UpdateNavMethodInfo> updateNavMethodInfos = new List<UpdateNavMethodInfo>();

        /// <summary>
        /// 导航插入列表
        /// </summary>
        private List<InsertNavMethodInfo> insertNavMethodInfos = new List<InsertNavMethodInfo>();

        /// <summary>
        /// 导航删除列表
        /// </summary>
        private List<DeleteNavMethodInfo> deleteNavMethodInfos = new List<DeleteNavMethodInfo>();

        public SqlSugarCoreUnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~SqlSugarCoreUnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public override void BeginTransaction()
        {
            sugarUnitOfWork = _sqlSugarClient.CreateContext();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public override void Commit()
        {
            if (sugarUnitOfWork != null)
            {
                //提交所有
                ExecuteNavCommand();
                _sqlSugarClient.SaveQueues();

                sugarUnitOfWork.Commit();
            }
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
                if (sugarUnitOfWork != null)
                {
                    sugarUnitOfWork.Dispose();
                }

                //资源已释放完毕，无需再调用析构函数
                GC.SuppressFinalize(this);
            }

            _isDisposed = true;
        }

        /// <summary>
        /// 保存所有
        /// </summary>
        public override void SaveChanges()
        {
            //开启事务
            using (var uow = _sqlSugarClient.CreateContext(_sqlSugarClient.Ado.IsNoTran()))
            {
                //提交所有
                ExecuteNavCommand();
                _sqlSugarClient.SaveQueues();

                uow.Commit();
            }
        }

        /// <summary>
        /// 执行所有导航操作
        /// </summary>
        private void ExecuteNavCommand()
        {
            foreach (var item in updateNavMethodInfos)
            {
                item.ExecuteCommand();
            }
            foreach (var item in insertNavMethodInfos)
            {
                item.ExecuteCommand();
            }
            foreach (var item in deleteNavMethodInfos)
            {
                item.ExecuteCommand();
            }
            updateNavMethodInfos = new List<UpdateNavMethodInfo>();
            insertNavMethodInfos = new List<InsertNavMethodInfo>();
            deleteNavMethodInfos = new List<DeleteNavMethodInfo>();
        }

        /// <summary>
        /// 插入导航更新
        /// </summary>
        /// <param name="updateNavMethodInfo"></param>
        public void AddUpdateNavMethodInfo(UpdateNavMethodInfo updateNavMethodInfo)
        {
            updateNavMethodInfos.Add(updateNavMethodInfo);
        }

        /// <summary>
        /// 插入导航插入
        /// </summary>
        /// <param name="insertNavMethodInfo"></param>
        public void AddInsertNavMethodInfo(InsertNavMethodInfo insertNavMethodInfo)
        {
            insertNavMethodInfos.Add(insertNavMethodInfo);
        }

        /// <summary>
        /// 插入导航删除
        /// </summary>
        /// <param name="deleteNavMethodInfo"></param>
        public void AddDeleteNavMethodInfo(DeleteNavMethodInfo deleteNavMethodInfo)
        {
            deleteNavMethodInfos.Add(deleteNavMethodInfo);
        }
    }
}