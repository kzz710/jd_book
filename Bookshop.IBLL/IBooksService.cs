using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.IBLL
{
    public partial interface IBooksService:IBaseService<Books>
    {

        void CreateHtmlPage(Books book);

        int GetCount(System.Linq.Expressions.Expression<Func<Books, bool>> whereLambda);

    }
}
