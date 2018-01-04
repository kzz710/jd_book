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
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        IUsersService UsersService { get; set; }
        ICartService cartService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckUserInfo()
        {
            string action = Request["action"];
            if (action.Equals("CheckUserName"))
            {
                string userName = Request["UserName"];
                Users user = UsersService.LoadEntities(u => u.LoginId == userName).FirstOrDefault();
                if (user == null)
                {
                    return Content("ok");
                }
            }
            if (action.Equals("CheckTelphone"))
            {
                string telphone = Request["Telphone"];
                Users user = UsersService.LoadEntities(u => u.Phone == telphone).FirstOrDefault();
                if (user == null)
                {
                    return Content("ok");
                }
            }
            if (action.Equals("CheckEmail"))
            {
                string email = Request["Email"];
                Users user = UsersService.LoadEntities(u => u.Mail == email).FirstOrDefault();
                if (user == null)
                {
                    return Content("ok");
                }
            }
            if (action.Equals("CheckValidateCode"))
            {
                string code = Request["ValidateCode"];
                string vcode = Session["validateCode"].ToString();
                if (code.Equals(vcode))
                {
                    return Content("ok");
                }
            }
            return Content("no");
        }

        public ActionResult UserLogin()
        {
            string userName = Request["loginName"];
            string password = Request["loginPwd"];
            bool isSuccess = true;
            password = Md5Helper.EncryptString(Md5Helper.EncryptString(password));
            Users user = UsersService.LoadEntities(u => u.LoginId == userName && u.LoginPwd == password).FirstOrDefault();
            if (user == null)
            {
                return Content("no:账户名和密码不匹配，请重新输入");
            }
            //Session["UserInfo"] = user;
            string sessionId = Guid.NewGuid().ToString();
            //将登录信息存入memcache中
            MemcacheHelper.Set(sessionId, SerializeHelper.SerializeToString(user), DateTime.Now.AddMinutes(20));
            //将memcache的键存入cookie中
            Response.Cookies["sessionId"].Value = sessionId;
            HttpCookie ck=Request.Cookies["commodity"];
            if (ck!=null)
            {
                List<BookViewModel> list = SerializeHelper.DeserializeToObject<List<BookViewModel>>(ck.Value);
                foreach (BookViewModel bvm in list)
                {
                    Cart cart = new Cart();
                    cart.BookId = bvm.bookId;
                    cart.Count = bvm.count;
                    cart.UserId = user.Id;
                    Cart cart1=cartService.LoadEntities(c=>c.BookId==bvm.bookId&&c.UserId==user.Id).FirstOrDefault();
                    if (cart1==null)
                    {
                        if (cartService.AddEntity(cart) == null)
                        {
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        cart1.Count += 1;
                        if (!cartService.EditEntity(cart1))
                        {
                            isSuccess = false;
                        }
                    }
                    

                }
                if (isSuccess)
                {
                    ck.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(ck);
                    Request.Cookies.Remove("commodity");
                }
            }
            
            
            return Content("ok:登录成功");
        }
        public Users LoginUser { get; set; }
        public ActionResult CheckSession()
        {
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
                return Content("no");
            }
            else
            {
                //Users user = (Users)Session["UserInfo"];
                return Content(LoginUser.LoginId);
            }
        }

        public ActionResult LoginOut()
        {
            string sessionId = Request.Cookies["sessionId"].Value;
            MemcacheHelper.Delete(sessionId);
            Response.Redirect("/jd_BookShop/index.html");
            return View();
        }

        public ActionResult FindPwd()
        {
            string userName = Request["userName"];
            string email = Request["email"];
            Users user = UsersService.LoadEntities(u => u.LoginId == userName).FirstOrDefault();
            if (user != null)
            {
                if (user.Mail == email)
                {
                    UsersService.SendFindPwdEmail(user);
                    return Content("ok:更换密码邮件已发送，请前往邮箱进行修改");
                }
                else
                {
                    return Content("no:用户名和邮箱不匹配");
                }
            }
            else
            {
                return Content("no:用户名不存在");
            }
        }

        public ActionResult UpdatePwdPage()
        {
            if (Request["id"] == null || Request["validate"] == null)
            {
                Response.Redirect("/jd_BookShop/login.html");
            }
            else
            {
                ViewBag.Id = Request["id"];
                ViewBag.Validate = Request["validate"];
            }
            return View();
        }

        public ActionResult UpdatePwd()
        {
            int id = Convert.ToInt32(Request["Id"]);
            string validate = Request["Validate"];
            string pwd = Request["password"];
            string cpwd = Request["confirmPwd"];
            if (pwd != null && cpwd != null)
            {
                if (pwd.Equals(cpwd))
                {
                    Users user = UsersService.LoadEntities(u => u.Id == id).FirstOrDefault();
                    if (user != null)
                    {
                        if (Md5Helper.EncryptString(user.LoginId).Equals(validate))
                        {
                            user.LoginPwd = Md5Helper.EncryptString(Md5Helper.EncryptString(pwd));
                            if (UsersService.EditEntity(user))
                            {
                                return Content("ok:密码更改成功");
                            }
                            else
                            {
                                return Content("no:密码更改失败，请稍后再试！！！");
                            }
                        }
                        else
                        {
                            return Content("no:此链接无效，请重新申请链接");
                        }

                    }
                    else
                    {
                        return Content("no:密码更改失败，请稍后再试！！");
                    }
                }
                else
                {
                    return Content("no:两次密码不一致");
                }
            }
            else
            {
                return Content("no:输入不能为空");

            }

        }
    }
}
