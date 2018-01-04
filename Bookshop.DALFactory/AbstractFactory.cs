using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Bookshop.IDAL;
using System.Reflection;

namespace Bookshop.DALFactory
{
    /// <summary>
    /// 通过反射创建类的实例，为了实现数据层和业务层的解耦
    /// </summary>
    public partial class AbstractFactory
    {
        private static readonly string AssemblyPath=ConfigurationManager.AppSettings["AssemblyPath"];
        private static readonly string NameSpace = ConfigurationManager.AppSettings["NameSpace"];

        public static IBooksDal CreateBooksDal() 
        {
            string fullClassName = NameSpace + ".BooksDal";
            return CreateIns(fullClassName) as IBooksDal;
        }
        public static IUsersDal CreateUserDal() 
        {
            string fullClassName = NameSpace + ".UsersDal";
            return CreateIns(fullClassName) as IUsersDal;
        }

        public static IBookCommentDal CreateBookCommentDal()
        {
            string fullClassName = NameSpace + ".BookCommentDal";
            return CreateIns(fullClassName) as IBookCommentDal;
        }

        public static IArticel_WordsDal CreateArticel_WordsDal()
        {
            string fullClassName = NameSpace + ".Articel_WordsDal";
            return CreateIns(fullClassName) as IArticel_WordsDal;
        }

        public static ICartDal CreateCartDal() 
        {
            string fullClassName = NameSpace + ".CartDal";
            return CreateIns(fullClassName) as ICartDal;
        }

        public static IAddressInfoDal CreateAddressInfoDal()
        {
            string fullClassName = NameSpace + ".AddressInfoDal";
            return CreateIns(fullClassName) as IAddressInfoDal;
        }

        public static IOrdersDal CreateOrdersDal()
        {
            string fullClassName = NameSpace + ".OrdersDal";
            return CreateIns(fullClassName) as IOrdersDal;
        }

        public static IOrderBookDal CreateOrderBookDal()
        {
            string fullClassName = NameSpace + ".OrderBookDal";
            return CreateIns(fullClassName) as IOrderBookDal;
        }

        private static object CreateIns(string className) 
        {
            var assembly = Assembly.Load(AssemblyPath);
            return assembly.CreateInstance(className);
        }
    }
}
