using Bookshop.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.IBLL
{
    public interface IBaseService<T> where T:class,new()
    {
        IDBSession CurrentDBSession { get; }
        IBaseDal<T> CurrentDal {get;set; }
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int pageCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda, bool isAsc);
        bool DeleteEntity(T entity);
        bool EditEntity(T entity);
        T AddEntity(T entity);
    }
}
