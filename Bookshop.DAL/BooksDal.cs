using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookshop.IDAL;

namespace Bookshop.DAL
{
    public partial class BooksDal:BaseDal<Books>,IBooksDal
    {
        
    }
}
