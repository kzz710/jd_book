using Bookshop.DALFactory;
using Bookshop.IBLL;
using Bookshop.IDAL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bookshop.BLL
{
    public class BooksService:BaseService<Books>,IBooksService
    {

        public override void SetCurrentDal()
        {
            this.CurrentDal = this.CurrentDBSession.BooksDal;
        }

        public void CreateHtmlPage(Books book)
        {
            //获得模板文件
            string template = HttpContext.Current.Request.MapPath("/jd_BookShop/template/HtmlPageTemplate.html");
            string fileContent = File.ReadAllText(template);

            //替换占位符
            fileContent = fileContent.Replace("$BookImg", "/jd_BookShop/image/BookCovers/" + book.ISBN + ".jpg").Replace("$BookTitle", book.Title).Replace("$BookContent", book.ContentDescription).Replace("$BookPrice", book.UnitPrice.ToString("0.00")).Replace("$BookId",book.Id.ToString()); ;
            string str = "/jd_BookShop/htmlPage/"+book.PublishDate.Year+"/"+book.PublishDate.Month+"/"+book.PublishDate.Day+"/";
            if (!Directory.Exists(HttpContext.Current.Request.MapPath(str)))
            {
                Directory.CreateDirectory(HttpContext.Current.Request.MapPath(str));
            }
            string fullPath = str + book.Id + ".html";
            File.WriteAllText(HttpContext.Current.Request.MapPath(fullPath),fileContent,System.Text.Encoding.UTF8);
        }


        public int GetCount(System.Linq.Expressions.Expression<Func<Books, bool>> whereLambda)
        {
            int Count = this.CurrentDBSession.BooksDal.LoadEntities(whereLambda).Count();
            return Count;
        }
    }
}
