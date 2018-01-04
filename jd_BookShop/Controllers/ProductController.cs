using Bookshop.IBLL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jd_BookShop.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/
        IBooksService booksService { get; set; }
        public ActionResult Index()
        {
            Users user = LoginUser;
            int pageIndex = Request["pageIndex"] == null ? 1 : int.Parse(Request["pageIndex"]);
            int pageSize = 24;
            int totalCount = booksService.GetCount(b => true);
            int pageCount = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize));
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageIndex = pageIndex > pageCount ? pageCount : pageIndex;
            List<Books> books = booksService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, b => true, b => b.Id, true).ToList();
            ViewBag.BookList = books;
            ViewBag.PageIndex = pageIndex;
            ViewBag.pageCount = pageCount;
            ViewBag.User = user;
            
            
            return View();
        }

        

    }
}
