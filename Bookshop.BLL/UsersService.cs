using Bookshop.IBLL;
using Bookshop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using Bookshop.Common;

namespace Bookshop.BLL
{
    public partial class UsersService:BaseService<Users>,IUsersService
    {

        public override void SetCurrentDal()
        {
            this.CurrentDal = this.CurrentDBSession.UsersDal;
        }



        public void SendFindPwdEmail(Users user)
        {
            string SmtpAddress=ConfigurationManager.AppSettings["SmtpAddress"];
            string FromAddress = ConfigurationManager.AppSettings["FromAddress"];
            string DomainIp = ConfigurationManager.AppSettings["DomainIp"];
            SmtpClient smtpClient = new SmtpClient(SmtpAddress);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromAddress);
            mailMessage.To.Add(user.Mail);
            mailMessage.Subject = "更换密码";
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body><a href='"+DomainIp+"/Users/UpdatePwdPage?id="+user.Id+"&validate="+Md5Helper.EncryptString(user.LoginId)+"'>请点击连接跳转到修改密码界面</a><body></html>");
            mailMessage.Body = sb.ToString();
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);//发送大量邮件时，容易发生阻塞，所以最好的办法是先把要发送的邮件发送到队列中
        }
    }
}
