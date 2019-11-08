using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public interface IBaseRepository<T> where T : class, new()
    {
        #region 新增
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="sOperator">操作人</param>
        /// <returns></returns>
        int Append(T entity, string sOperator);
        int AppendRange(List<T> lsT, string sOperator);
        Task<int> AppendRangeAsync(List<T> lsT, string sOperator);
        #endregion
        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="sOperator">操作人</param>
        /// <returns></returns>
        int Update(T entity, string sOperator);
        int UpdateRange(List<T> lsT, string sOperator);
        /// <summary>
        /// 插入实体前
        /// </summary>
        /// <param name="DB">上下文</param>
        /// <param name="entity">实体</param>
        /// <param name="sOperator">操作人</param>
        /// <returns></returns>
        bool BeforeAppend(DbContext DB, T entity, string sOperator);
        /// <summary>
        /// 插入实体后
        /// </summary>
        /// <param name="DB">上下文</param>
        /// <param name="entity">实体</param>
        /// <param name="sOperator">操作人</param>
        void AfterAppend(DbContext DB, T entity, string sOperator);
        /// <summary>
        /// 修改前
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="entity"></param>
        /// <param name="sOperator"></param>
        /// <returns></returns>
        bool BefoUpdate(DbContext DB, T entity, string sOperator);
        /// <summary>
        /// 修改后
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="entity"></param>
        /// <param name="sOperator"></param>
        void AfterUpdate(DbContext DB, T entity, string sOperator);
        #endregion
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sOperator"></param>
        /// <returns></returns>
        int Delete(T entity, string sOperator);
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sOperator"></param>
        /// <returns></returns>
        int Delete(int id, string sOperator);
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="lstT"></param>
        /// <param name="sOperator"></param>
        /// <returns></returns>
        int DeleteRange(List<T> lstT, string sOperator);
        /// <summary>
        /// 根据ID批量删除
        /// </summary>
        /// <param name="arrId"></param>
        /// <param name="sOperator"></param>
        /// <returns></returns>
        int DeleteRange(int[] arrId, string sOperator);
        /// <summary>
        /// 删除前
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="entity"></param>
        /// <param name="sOperator"></param>
        /// <returns></returns>
        bool BeforDelete(DbContext DB, T entity, string sOperator);
        /// <summary>
        /// 删除后
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="entity"></param>
        /// <param name="sOperator"></param>
        void AfterDelete(DbContext DB, T entity, string sOperator);
        #endregion
        #region 查询
        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="oSearchEntity">条件</param>
        /// <param name="sOperator">操作人</param>
        /// <param name="iOrderGroup">排序</param>
        /// <param name="sSortName">排序名称</param>
        /// <param name="sSortOrder">排序方式</param>
        /// <returns>实体</returns>
        T Select(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, string sSortName = null, string sSortOrder = null);
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="sOperator">操作人</param>
        /// <returns>实体</returns>
        T Select(int id, string sOperator = null);
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="oSearchEntity">条件</param>
        /// <param name="sOperator">操作人</param>
        /// <param name="iOrderGroup">排序</param>
        /// <param name="iMaxCount">最大条数</param>
        /// <param name="sSortName">排序名称</param>
        /// <param name="sSortOrder">排序方式</param>
        /// <returns>集合</returns>
        List<T> SelectALL(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, int iMaxCount = 0, string sSortName = null, string sSortOrder = null);
        PageInfo<T> GetPageList(PageInfo<T> pageInfo, T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, string sSortName = null, string sSortOrder = null);
        /// <summary>
        /// 查询实体是否存在(不返回消息)
        /// </summary>
        /// <param name="DB">上下文</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Exists(DbContext DB, T entity);
        /// <summary>        
        ///  查询实体是否存在(不返回消息)
        /// </summary>
        /// <param name="DB">上下文</param>
        /// <param name="entity">实体</param>
        /// <param name="sErrorMessage">错误消息</param>
        /// <returns></returns>
        bool Exists(DbContext DB, T entity, out string sErrorMessage);
        void ChangeDataDeleteKey(T entity, string sOperator);
        PageInfo<T> GetPageList<Tkey>(PageInfo<T> pageInfo, Expression<Func<T, bool>> whereLambda = null, Func<T, Tkey> orderbyLambda = null, bool isAsc = true);

        Task<int> AppendAsync(T entity, string sOperator);
        Task<T> SelectAsync(int id, string sOperator = null);
        Task<T> SelectAsync(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, string sSortName = null, string sSortOrder = null);

        Task<List<T>> SelectALLAsync(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, int iMaxCount = 0, string sSortName = null, string sSortOrder = null);
        Task<PageInfo<T>> GetPageListAsync(PageInfo<T> pageInfo, int iOrderGroup = 0, string sOperator = null);
        #endregion
    }
}
