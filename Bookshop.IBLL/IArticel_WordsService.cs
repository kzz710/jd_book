using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.IBLL
{
    public partial interface IArticel_WordsService:IBaseService<Articel_Words>
    {
        bool CheckForbiddenWord(string Msg);

        bool CheckModWord(string Msg);

        string CheckReplaceWord(string Msg);
    }
}
