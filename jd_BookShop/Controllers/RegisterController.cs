using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookshop.Common;
using Bookshop.IBLL;
using Bookshop.Model;
using jd_BookShop.Emun;

namespace jd_BookShop.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        IUsersService UsersService { get; set; }
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ShowValidateCode() 
        {
            ValidateCode validateCode=new ValidateCode();
            string code = validateCode.CreateValidateCode(5);
            Session["validateCode"] = code;
            byte[] buffer = validateCode.CreateValidateGraphic(code);
            return File(buffer, "image/jpeg");
        }

        public ActionResult UserRegister() 
        {
            string userName=Request["UserName"];
            string password = Request["Password"];
            string confirmPwd=Request["ConfirmPwd"];
            string telphone = Request["Telphone"];
            string email=Request["Email"];
            string validateCode=Request["ValidateCode"];
            string telCode = Request["TelCode"];
            string vcode = Session["validateCode"].ToString();
            
            if (!validateCode.Equals(vcode))
            {
                return Content("no:validateCode:验证码错误");
            }
            Users user = UsersService.LoadEntities(u => u.LoginId == userName).FirstOrDefault();
            if(user!=null)
            {
                return Content("no:userName:此用户已存在");
            }
            if(!password.Equals(confirmPwd))
            {
                return Content("no:password:两次密码输入不一致");
            }
            user = UsersService.LoadEntities(u=>u.Phone==telphone).FirstOrDefault();
            if (user!=null)
            {
                return Content("no:telphone:此手机号已注册");
            }
            user = UsersService.LoadEntities(u=>u.Mail==email).FirstOrDefault();
            if (user != null)
            {
                return Content("no:email:此邮箱已注册");
            }
            Users user1 = new Users();
            user1.LoginId = userName;
            user1.LoginPwd = Md5Helper.EncryptString(Md5Helper.EncryptString(password));
            user1.Phone = telphone;
            user1.Mail = email;
            user1.Address = telCode;
            user1.UserStateId = Convert.ToInt32(UserState.Add);
            user1.Name = "jd_user";
            Users user2 = UsersService.AddEntity(user1);
            if(user2==null)
            {
                return Content("no:all:注册失败，请稍后再试");
            }
            return Content("ok:all:注册成功");
        }

    }
}
