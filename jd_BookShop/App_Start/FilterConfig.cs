using jd_BookShop.Models;
using System.Web;
using System.Web.Mvc;

namespace jd_BookShop
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new MyExceptionAttribute());
        }
    }
}