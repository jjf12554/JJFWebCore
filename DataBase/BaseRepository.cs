using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        protected readonly DbContext _dbContext;
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //public BaseRepository()
        //{
        //    _dbContext = GetDB();
        //}
        // public abstract DbContext GetDB();
        protected virtual IQueryable<T> SearchFilter(DbContext DB, T oSearchEntity, IQueryable<T> query, string sOperator = null)
        {
            query = FixedQueryFilter(DB, oSearchEntity, query, sOperator);
            return query;
        }
        protected virtual IQueryable<T> SearchFilterB(DbContext DB, T oSearchEntity, IQueryable<T> query, string sOperator = null)
        {
            return query;
        }
        protected virtual IQueryable<T> FixedQueryFilter(DbContext DB, T oSearchEntity, IQueryable<T> query, string sOperator = null)
        {
            return query;
        }
        protected virtual IQueryable<T> SelectFilter(DbContext DB, T oSearchEntity, IQueryable<T> query, string sOperator = null)
        {
            query = FixedQueryFilter(DB, oSearchEntity, query, sOperator);
            return query;
        }
        protected virtual IQueryable<T> SelectFilterB(DbContext DB, T oSearchEntity, IQueryable<T> query, string sOperator = null)
        {
            query = FixedQueryFilter(DB, oSearchEntity, query, sOperator);
            return query;
        }
        protected virtual IQueryable<T> OrderBySingleField(IQueryable<T> query, string sSortName = null, string sSortOrder = null)
        {
            return query;
        }
        protected IQueryable<T> BaseOrderBy(IQueryable<T> query, int iOrderGroup, string sSortName = null, string sSortOrder = null)
        {
            if (!string.IsNullOrWhiteSpace(sSortName))
            {
                return OrderBySingleField(query, sSortName, sSortOrder);
            }
            else
            {
                return OrderBy(query, iOrderGroup);
            }
        }
        protected abstract IQueryable<T> ExistsFilter(out string sErrorMessage, T entity, IQueryable<T> query);
        protected abstract IQueryable<T> OrderBy(IQueryable<T> query, int iOrderGroup = 0);
        public int Append(T entity, string sOperator)
        {
            int iResult = 0;
            if (!BeforeAppend(_dbContext, entity, sOperator))
            {
                return iResult;
            }
            _dbContext.Set<T>().Add(entity);
            iResult += _dbContext.SaveChanges();
            AfterAppend(_dbContext, entity, sOperator);
            return iResult;
        }
        public virtual bool BeforeAppend(DbContext DB, T entity, string sOperator)
        {
            if (Exists(DB, entity, out string sErrorMessage))
            {
                throw new Exception(sErrorMessage + entity.ToString());
            }
            Type t = typeof(T);
            PropertyInfo propertyInfo = t.GetProperty("Id");
            propertyInfo = t.GetProperty("TcreateTime");
            propertyInfo.SetValue(entity, DateTime.Now);
            propertyInfo = t.GetProperty("TmodifyTime");
            propertyInfo.SetValue(entity, DateTime.Now);
            propertyInfo = t.GetProperty("Screater");
            propertyInfo.SetValue(entity, sOperator);
            propertyInfo = t.GetProperty("Smodifier");
            propertyInfo.SetValue(entity, sOperator);
            return true;
        }
        public virtual void AfterAppend(DbContext DB, T entity, string sOperator)
        {
            ChangeDataDeleteKey(entity, sOperator);
        }
        public int Update(T entity, string sOperator)
        {
            int iResult = 0;
            if (!BefoUpdate(_dbContext, entity, sOperator))
            {
                return iResult;
            }
            _dbContext.Set<T>().Update(entity);
            iResult += _dbContext.SaveChanges();
            AfterUpdate(_dbContext, entity, sOperator);
            return iResult;
        }
        public virtual bool BefoUpdate(DbContext DB, T entity, string sOperator)
        {
            if (Exists(DB, entity, out string sErrorMessage))
            {
                throw new Exception(sErrorMessage + entity.ToString());
            }
            Type t = typeof(T);
            PropertyInfo propertyInfo = t.GetProperty("Id");
            propertyInfo = t.GetProperty("TmodifyTime");
            propertyInfo.SetValue(entity, DateTime.Now);
            propertyInfo = t.GetProperty("Smodifier");
            propertyInfo.SetValue(entity, sOperator);
            return true;

        }
        public virtual void AfterUpdate(DbContext DB, T entity, string sOperator)
        {
            ChangeDataDeleteKey(entity, sOperator);
        }
        public int Delete(T entity, string sOperator)
        {
            int iResult = 0;
            if (entity != null)
            {
                if (!BeforDelete(_dbContext, entity, sOperator))
                {
                    return iResult;
                }
                _dbContext.Set<T>().Remove(entity);
                iResult += _dbContext.SaveChanges();
                AfterDelete(_dbContext, entity, sOperator);
            }
            return iResult;
        }
        public int Delete(int id, string sOperator)
        {
            int iResult = 0;
            T entity = _dbContext.Set<T>().Find(id);
            if (entity != null)
            {
                if (!BeforDelete(_dbContext, entity, sOperator))
                {
                    return iResult;
                }
                _dbContext.Set<T>().Remove(entity);
                iResult += _dbContext.SaveChanges();
                AfterDelete(_dbContext, entity, sOperator);
            }
            return iResult;
        }
        public int DeleteRange(List<T> lstT, string sOperator)
        {
            int iResult = 0;
            if (lstT?.Count > 0)
            {
                foreach (T entity in lstT)
                {
                    iResult += Delete(entity, sOperator);
                }
            }
            return iResult;
        }
        public int DeleteRange(int[] arrId, string sOperator)
        {
            int iResult = 0;
            if (arrId?.Length > 0)
            {
                for (int i = 0; i < arrId.Length; i++)
                {
                    iResult += Delete(arrId[i], sOperator);
                }
            }
            return iResult;
        }
        public virtual bool BeforDelete(DbContext DB, T entity, string sOperator)
        {
            return true;
        }
        public virtual void AfterDelete(DbContext DB, T entity, string sOperator)
        {
            ChangeDataDeleteKey(entity, sOperator);
        }
        public bool Exists(DbContext DB, T entity)
        {
            string sErrorMessage = string.Empty;
            return Exists(DB, entity, out sErrorMessage);
        }
        public bool Exists(DbContext DB, T entity, out string sErrorMessage)
        {
            try
            {
                IQueryable<T> query = DB.Set<T>().AsQueryable();
                if (ExistsFilter(out sErrorMessage, entity, query).Count() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
        public T Select(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, string sSortName = null, string sSortOrder = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (oSearchEntity != null)
            {
                query = SelectFilterB(_dbContext, oSearchEntity, query, sOperator);
                query = SelectFilter(_dbContext, oSearchEntity, query, sOperator);
            }
            query = BaseOrderBy(query, iOrderGroup, sSortName, sSortOrder);
            return query.FirstOrDefault();
        }
        public T Select(int id, string sOperator = null)
        {
            T entity = _dbContext.Set<T>().Find(id);
            return entity;
        }
        public List<T> SelectALL(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, int iMaxCount = 0, string sSortName = null, string sSortOrder = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (oSearchEntity != null)
            {
                query = SelectFilterB(_dbContext, oSearchEntity, query, sOperator);
                query = SelectFilter(_dbContext, oSearchEntity, query, sOperator);
            }
            query = BaseOrderBy(query, iOrderGroup, sSortName, sSortOrder);
            iMaxCount = iMaxCount == 0 ? query.Count() : iMaxCount;
            return query.Take(iMaxCount).ToList();
        }
        public PageInfo<T> GetPageList(PageInfo<T> pageInfo, T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, string sSortName = null, string sSortOrder = null)
        {
            if (pageInfo.page > 0 && pageInfo.limit > 0)
            {
                IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
                query = SearchFilterB(_dbContext, oSearchEntity, query, sOperator);
                query = SearchFilter(_dbContext, oSearchEntity, query, sOperator);
                pageInfo.count = query.Count();
                query = BaseOrderBy(query, iOrderGroup, sSortName, sSortOrder);
                IList<T> lsT = query.Skip((pageInfo.page - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();
                pageInfo.data = lsT;
            }
            return pageInfo;
        }
        public PageInfo<T> GetPageList<Tkey>(PageInfo<T> pageInfo, Expression<Func<T, bool>> whereLambda = null, Func<T, Tkey> orderbyLambda = null, bool isAsc = true)
        {
            if (pageInfo.page > 0 && pageInfo.limit > 0)
            {
                //总数
                pageInfo.count = _dbContext.Set<T>().Where(whereLambda).Count();
                if (isAsc)
                {
                    var query = _dbContext.Set<T>().Where(whereLambda)
                         .OrderBy<T, Tkey>(orderbyLambda)
                         .Skip(pageInfo.limit * (pageInfo.page - 1))
                         .Take(pageInfo.limit);
                    pageInfo.data = query.ToList();
                }
                else
                {
                    var query = _dbContext.Set<T>().Where(whereLambda)
                         .OrderByDescending<T, Tkey>(orderbyLambda)
                         .Skip(pageInfo.limit * (pageInfo.page - 1))
                         .Take(pageInfo.limit);
                    pageInfo.data = query.ToList();
                }
            }
            return pageInfo;
        }
        public async Task<int> AppendAsync(T entity, string sOperator)
        {
            int iResult = 0;
            if (!BeforeAppend(_dbContext, entity, sOperator))
            {
                return iResult;
            }
            await _dbContext.Set<T>().AddAsync(entity);
            iResult = await _dbContext.SaveChangesAsync();
            AfterAppend(_dbContext, entity, sOperator);
            return iResult;
        }
        public Task<PageInfo<T>> GetPageListAsync(PageInfo<T> pageInfo, int iOrderGroup = 0, string sOperator = null)
        {
            //using (DbContext DB = GetDB())
            //{
            //    DB.Set<T>().ToListAsync();
            //    IQueryable<T> query = DB.Set<T>().AsQueryable();
            //    pageInfo.count = query.Count();
            //    query = OrderBy(query, iOrderGroup);
            //    IList<T> lstT = query.Skip((pageInfo.page - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();
            //    pageInfo.data = lstT;
            //};
            //return pageInfo;
            return null;
        }

        public async Task<T> SelectAsync(int id, string sOperator = null)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<T> SelectAsync(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, string sSortName = null, string sSortOrder = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (oSearchEntity != null)
            {
                query = SelectFilterB(_dbContext, oSearchEntity, query, sOperator);
                query = SelectFilter(_dbContext, oSearchEntity, query, sOperator);
            }
            query = BaseOrderBy(query, iOrderGroup, sSortName, sSortOrder);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> SelectALLAsync(T oSearchEntity = null, string sOperator = null, int iOrderGroup = 0, int iMaxCount = 0, string sSortName = null, string sSortOrder = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (oSearchEntity != null)
            {
                query = SelectFilterB(_dbContext, oSearchEntity, query, sOperator);
                query = SelectFilter(_dbContext, oSearchEntity, query, sOperator);
            }
            query = BaseOrderBy(query, iOrderGroup, sSortName, sSortOrder);
            iMaxCount = iMaxCount == 0 ? query.Count() : iMaxCount;
            return await query.Take(iMaxCount).ToListAsync();
        }

        public virtual void ChangeDataDeleteKey(T entity, string sOperator)
        {

        }
        public int AppendRange(List<T> lsT, string sOperator)
        {
            int iResult = 0;
            if (lsT.Any())
            {
                foreach (T entity in lsT)
                {
                    iResult += Append(entity, sOperator);
                }
            }
            return iResult;
        }
        public async Task<int> AppendRangeAsync(List<T> lsT, string sOperator)
        {
            int iResult = 0;
            if (lsT.Any())
            {
                foreach (T entity in lsT)
                {
                    iResult += await AppendAsync(entity, sOperator);
                }
            }
            return iResult;
        }

        public int UpdateRange(List<T> lsT, string sOperator)
        {
            int iResult = 0;
            if (lsT.Any())
            {
                foreach (T entity in lsT)
                {
                    iResult += Update(entity, sOperator);
                }
            }
            return iResult;
        }
    }
}
