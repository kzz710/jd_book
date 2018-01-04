using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.IBLL
{
    public interface IUsersService:IBaseService<Users>
    {
        void SendFindPwdEmail(Users user);
    }
}
