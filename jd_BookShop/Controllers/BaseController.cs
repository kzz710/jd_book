using Bookshop.Common;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jd_BookShop.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public Users LoginUser { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool isSuccess = false;
            if (Request.Cookies["sessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                object obj = MemcacheHelper.Get(sessionId);
                if (obj != null)
                {
                    isSuccess = true;
                    Users user = SerializeHelper.DeserializeToObject<Users>(obj.ToString());
                    LoginUser = user;
                    MemcacheHelper.Set(sessionId, obj, DateTime.Now.AddMinutes(20));
                    
                }
            }
            if (!isSuccess)
            {
                filterContext.HttpContext.Response.Redirect("/jd_BookShop/login.html");
            }
            
        }

    }
}
