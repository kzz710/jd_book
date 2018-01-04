using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.IDAL
{
    public partial interface IDBSession
    {
        DbContext Db { get; }

        bool SaveChanges();

        IBooksDal BooksDal { get; }

        IUsersDal UsersDal { get; }

        IBookCommentDal BookCommentDal { get; }

        IArticel_WordsDal Articel_WordsDal { get; }

        IAddressInfoDal AddressInfoDal { get; }

        ICartDal CartDal { get; }

        IOrdersDal OrderDal { get; }

        IOrderBookDal OrderBookDal { get; }
    }
}
