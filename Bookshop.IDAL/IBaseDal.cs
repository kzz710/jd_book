using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.IDAL
{
    public interface IBaseDal<T> where T:class,new() 
    {
        //增加
        T AddEntity(T entity);

        //删除
        bool DeleteEntity(T entity);

        //修改
        bool UpdateEntity(T entity);

        //查询
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);

        //分页查询
        IQueryable<T> LoadPageEntites<s>(int pageIndex, int pageSize, out int pageCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderLambda,bool isAsc);


    }
}
