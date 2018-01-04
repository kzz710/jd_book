using Bookshop.Common;
using Bookshop.IBLL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jd_BookShop.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        IArticel_WordsService articel_WordsService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddArticelWordsPage() 
        {
            return View();
        }
        public ActionResult AddArticelWords() 
        {
           
                string msg=Request["txtMsg"];
                msg = msg.Trim();
                string[] words = msg.Split(new char[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string[] w = word.Split('=');
                    Articel_Words awModel = new Articel_Words();
                    awModel.WordPattern = w[0];
                    if (w[1]=="{BANNED}")
                    {
                        awModel.IsForbid = true;
                    }
                    else if (w[1]=="{MOD}")
                    {
                        awModel.IsMod = true;
                    }
                    else
                    {
                        awModel.ReplaceWord = w[1];
                    }
                    Articel_Words awModel1=articel_WordsService.AddEntity(awModel);
                    if (awModel1==null)
                    {
                        ViewBag.Msg = "no:添加失败";
                    }
                    else
                    {
                        ViewBag.Msg = "ok:添加成功";
                        MemcacheHelper.Delete("forbiddenWords");
                        MemcacheHelper.Delete("modWords");
                        MemcacheHelper.Delete("replaceWords");
                    }
                
               
            }
                return View("AddArticelWordsPage");
        }
    }
}
