using Bookshop.IDAL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.DAL
{
    public class BaseDal<T> where T:class ,new()
    {
        public DbContext db = DBContextFactory.CreateDbContext();
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            db.Set<T>().Add(entity);
            
            return entity;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            db.Entry<T>(entity).State = System.Data.EntityState.Deleted;
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntity(T entity)
        {
            db.Entry<T>(entity).State = System.Data.EntityState.Modified;
            return true;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where<T>(whereLambda);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntites<s>(int pageIndex, int pageSize, out int pageCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc)
        {
            var temp = db.Set<T>().Where<T>(whereLambda);
            pageCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<T, s>(orderLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }
    }
}
