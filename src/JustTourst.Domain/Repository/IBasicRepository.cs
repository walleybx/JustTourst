using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Domain.Repository
{
    public interface IBasicRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        /// <summary>
        /// SqlsugarClient实体
        /// </summary>
        ISqlSugarClient Db { get; }
        /// <summary>
        /// 获取一个自定义的数据库处理对象
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        SqlSugarScope GetCustomDB(ConnectionConfig config);

        #region 新增
        /// <summary>
        /// 同步新增实体。
        /// </summary>
        /// <param name="entity">实体</param>

        /// <returns></returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 异步新增实体。
        /// </summary>
        /// <param name="entity">实体</param>

        /// <returns></returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// 同步批量新增实体。
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        void Insert(List<TEntity> entities);

        /// <summary>
        /// 新增或更新
        /// </summary>
        /// <param name="entities">数据集合</param>
        /// <param name="insertNum">返回新增数量</param>
        /// <param name="updateNum">返回更新数量</param>
        void Save(List<TEntity> entities, out int insertNum, out int updateNum);
        #endregion

        #region 删除
        /// <summary>
        /// 同步物理删除实体。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// 异步物理删除实体。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity entity);

        /// <summary>
        /// 同步物理删除实体。
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        bool Delete(object primaryKey);

        /// <summary>
        /// 异步物理删除实体。
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(object primaryKey);

        /// <summary>
        /// 按主键批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteBatch(IList<dynamic> ids);

        /// <summary>
        /// 按条件批量删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        bool DeleteBatchWhere(string where);
        /// <summary>
        /// 异步按条件批量删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        Task<bool> DeleteBatchWhereAsync(string where);

        #endregion

        #region 更新操作

        #region 更新实体或批量更新

        /// <summary>
        /// 同步更新实体。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Update(TEntity entity);
        /// <summary>
        /// 异步更新实体。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);
        /// <summary>
        /// 同步更新实体。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        bool Update(TEntity entity, string strWhere);

        /// <summary>
        /// 异步更新实体。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity, string strWhere);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns">更新字段</param>
        /// <param name="lstIgnoreColumns">字段值</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");
        #endregion

        #region 更新某一字段值
        /// <summary>
        /// 更新某一字段值
        /// </summary>
        /// <param name="strField">字段</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="where">条件,为空更新所有内容</param>
        /// <returns></returns>
        bool UpdateTableField(string strField, string fieldValue, string where);

        /// <summary>
        /// 异步更新某一字段值
        /// </summary>
        /// <param name="strField">字段</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="where">条件,为空更新所有内容</param>
        /// <returns></returns>
        Task<bool> UpdateTableFieldAsync(string strField, string fieldValue, string where);
        /// <summary>
        /// 更新某一字段值，字段值为数字
        /// </summary>
        /// <param name="strField">字段</param>
        /// <param name="fieldValue">字段值数字</param>
        /// <param name="where">条件,为空更新所有内容</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool UpdateTableField(string strField, int fieldValue, string where);
        /// <summary>
        /// 更新某一字段值，字段值为数字
        /// </summary>
        /// <param name="strField">字段</param>
        /// <param name="fieldValue">字段值数字</param>
        /// <param name="where">条件,为空更新所有内容</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        Task<bool> UpdateTableFieldAsync(string strField, int fieldValue, string where);
        #endregion

        #endregion
    }
}
