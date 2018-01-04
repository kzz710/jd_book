using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jd_BookShop.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ShowResult() 
        {
            int a = 0;
            int b = 1;
            int c = b / a;
            return View();
        }

    }
}
