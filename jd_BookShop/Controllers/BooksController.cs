using Bookshop.BLL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookshop.Common;
using Bookshop.IBLL;

namespace jd_BookShop.Controllers
{
    public class BooksController : Controller
    {
        //
        // GET: /Books/
        
        //BooksService booksService = new BooksService();
        IBooksService booksService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBooks() 
        {
            //List<Books> books = booksService.LoadEntities(b => (b.Id > 5000 && b.Id < 5050)).ToList();
            int pageCount=0;
            List<Books> books = booksService.LoadPageEntities<int>(1, 50, out pageCount, b => true, b => b.Id, true).ToList();
            string bookDate = SerializeHelper.SerializeToString(books);
            return Content(bookDate);
        }

        public ActionResult BooksAdmin() 
        {
            return View();
        }

        public ActionResult CreateHtmlPage() 
        {
            int pageCount = 0;
            List<Books> books = booksService.LoadPageEntities<int>(1, 360, out pageCount, b => true, b => b.Id, true).ToList();
            foreach (Books book in books)
            {
                booksService.CreateHtmlPage(book);
            }
            return Content("ok");
        }

        public ActionResult Test() 
        {
            List<Books> books=booksService.LoadEntities(b=>(b.Id>5000&&b.Id<5100)).ToList();
            ViewData.Add("books", books);
            return View();
        }

        public ActionResult GetPageBooks() 
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = 24;
            int pageCount = 0;
            List<Books> books = booksService.LoadPageEntities<int>(pageIndex, pageSize, out pageCount, b => true, b => b.Id, true).ToList();
            string bookDate = SerializeHelper.SerializeToString(books);
            return Content(bookDate);

        }

        public ActionResult AddClickCount() 
        {
            int bookId = Convert.ToInt32(Request["bookId"]);
            Books book = booksService.LoadEntities(b=>b.Id==bookId).FirstOrDefault();
            book.Clicks += 1;
            if (booksService.EditEntity(book))
            {
                return Content("ok");
            }
            else 
            {
                return Content("no");
            }
            

        }

    }
}
