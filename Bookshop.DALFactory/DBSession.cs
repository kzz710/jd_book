using Bookshop.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshop.DAL;

namespace Bookshop.DALFactory
{
    /// <summary>
    /// 数据会话层：就是一个工厂类，负责完成所有数据操作实例的创建，然后业务层通过数据会话层来获取
    /// 要操作的数据类实例。所以数据会话层将业务层与数据层解耦
    /// 在数据会话层中提供一个方法，完成所有数据的保存
    /// </summary>
    public class DBSession:IDBSession
    {

        public System.Data.Entity.DbContext Db
        {
            get { return DBContextFactory.CreateDbContext(); }
        }
        public IBooksDal BooksDal 
        {
            get { return AbstractFactory.CreateBooksDal(); }
        }
        public IUsersDal UsersDal 
        {
            get { return AbstractFactory.CreateUserDal(); }
        }


        public IBookCommentDal BookCommentDal
        {
            get { return AbstractFactory.CreateBookCommentDal(); }
        }


        public IArticel_WordsDal Articel_WordsDal
        {
            get { return AbstractFactory.CreateArticel_WordsDal(); }
        }

        public ICartDal CartDal
        {
            get { return AbstractFactory.CreateCartDal(); }
        }

        public IAddressInfoDal AddressInfoDal
        {
            get { return AbstractFactory.CreateAddressInfoDal(); }
        }

        public IOrdersDal OrderDal
        {
            get { return AbstractFactory.CreateOrdersDal(); }
        }

        public IOrderBookDal OrderBookDal
        {
            get { return AbstractFactory.CreateOrderBookDal(); }
        }

        public bool SaveChanges()
        {
            return Db.SaveChanges()>0;
        }



        
    }
}
