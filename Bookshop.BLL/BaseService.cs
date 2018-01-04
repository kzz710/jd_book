using Bookshop.DALFactory;
using Bookshop.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.BLL
{
    public abstract class BaseService<T> where T:class,new()
    {
        public IDBSession CurrentDBSession
        {
            get { return DBSessionFactory.CreateDBSession(); }
        }

        public IBaseDal<T> CurrentDal { get; set; }

        public abstract void SetCurrentDal();

        /// <summary>
        /// 创建子类对象时，先执行父类的构造方法
        /// </summary>
        public BaseService()
        {
            SetCurrentDal();
        }
        /// <summary>
        /// 业务查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {

            return CurrentDal.LoadEntities(whereLambda);
            
        }
        /// <summary>
        /// 业务分页查询
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int pageCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntites<s>(pageIndex,pageSize,out pageCount,whereLambda,orderLambda,isAsc);
        }
        /// <summary>
        /// 业务删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 业务修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            CurrentDal.UpdateEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 业务添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            CurrentDBSession.SaveChanges();
            return entity;
        }

    }
}
