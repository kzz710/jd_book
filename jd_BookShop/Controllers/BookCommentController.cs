using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookshop.IBLL;
using Bookshop.Model;
using Bookshop.Common;
using jd_BookShop.Models;

namespace jd_BookShop.Controllers
{
    public class BookCommentController : Controller
    {
        //
        // GET: /BookComment/
        IBookCommentService bookCommentService { get; set; }
        IArticel_WordsService articel_WordsService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBookComment() 
        {
            int bookId=Convert.ToInt32(Request["bookId"]);
            List<BookComment> bcList = bookCommentService.LoadEntities(b=>b.BookId==bookId&&b.IsPass==true).ToList();
            if (bcList == null)
            {
                return Content("no");
            }
            List<BookCommentViewModel> bcvmList = new List<BookCommentViewModel>();
            foreach (BookComment bookComment in bcList)
            {
                BookCommentViewModel bcvm = new BookCommentViewModel();
                bcvm.CreateDateTime = WebCommon.GetTimeSpan(DateTime.Now-bookComment.CreateDateTime);

                //bcvm.Msg = bookComment.Msg;
                //将script代码转换为普通字符，避免脚本攻击
                bcvm.Msg = WebCommon.UbbToHtml(bookComment.Msg);
                bcvmList.Add(bcvm);
            }
            
            return Content(SerializeHelper.SerializeToString(bcvmList));
        }

        [ValidateInput(false)]
        public ActionResult AddBookComment() 
        {
            string msg=Request["bookComment"];
            int bookId = Convert.ToInt32(Request["bookId"]);
            bool isPass = true;
            if (articel_WordsService.CheckForbiddenWord(msg))
            {
                return Content("no:你的评论含有禁用词，不能评论");
            }
            if (articel_WordsService.CheckModWord(msg))
            {
                isPass = false;
                return Content("no:你的评论含有敏感词汇，需要审核");
            }
            msg = articel_WordsService.CheckReplaceWord(msg);
            BookComment bookComment = new BookComment() 
            {
                CreateDateTime=DateTime.Now,
                Msg=msg,
                BookId=bookId,
                IsPass=isPass
            };
            BookComment bcAdd = bookCommentService.AddEntity(bookComment);
            if (bcAdd == null)
            {
                return Content("no:评论失败");
            }
            else 
            {
                return Content("ok:评论成功");
            }

            
        }

    }
}
