using Bookshop.IBLL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.BLL
{
    public class AddressInfoService:BaseService<AddressInfo>,IAddressInfoService
    {
        public override void SetCurrentDal()
        {
            this.CurrentDal = this.CurrentDBSession.AddressInfoDal;
        }
    }
}
