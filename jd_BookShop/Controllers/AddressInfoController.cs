using Bookshop.IBLL;
using Bookshop.Model;
using jd_BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jd_BookShop.Controllers
{
    public class AddressInfoController : BaseController
    {
        //
        // GET: /AddressInfo/
        IAddressInfoService addressInfoService { get; set; }
        ICartService cartService { get; set; }
        public ActionResult Index()
        {
            Users user = LoginUser;
            List<AddressInfo> list = addressInfoService.LoadEntities(a=>a.UserId==user.Id).ToList();
            if (list.Count != 0)
            {
                ViewBag.AddressList = list;

            }
            else
            {
                ViewBag.AddressList = null;
                
            }
            
            ViewBag.User = user;
            return View();
        }
        /// <summary>
        /// 添加修改收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult AddAddressInfo() 
        {
            int userId = Convert.ToInt32(Request["UserId"]);
            string name = Request["Name"];
            string zipCode = Request["ZipCode"];
            string telphone = Request["Telphone"];
            string address = Request["Address"];
            
            //根据接收的Id是否为空，判断是添加还是修改
            if (Request["Id"]!="")
            {
                int id = Convert.ToInt32(Request["Id"]);
                AddressInfo addressInfo = addressInfoService.LoadEntities(a=>a.Id==id).FirstOrDefault();
                if (addressInfo!=null)
                {
                    addressInfo.Name = name;
                    addressInfo.Telphone = telphone;
                    addressInfo.Address = address;
                    addressInfo.ZipCode = zipCode;
                    if (addressInfoService.EditEntity(addressInfo))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    return Content("no");
                }

            }
            else
            {
                AddressInfo addressInfo = new AddressInfo() 
                {
                    UserId=userId,
                    Name=name,
                    ZipCode=zipCode,
                    Address=address,
                    Telphone=telphone
                };
                if (addressInfoService.AddEntity(addressInfo)!=null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteAddressInfo() 
        {
            int id =Convert.ToInt32(Request["Id"]);
            AddressInfo address = addressInfoService.LoadEntities(a=>a.Id==id).FirstOrDefault();
            if (addressInfoService.DeleteEntity(address))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        /// <summary>
        /// 老方法
        /// </summary>
        /// <returns></returns>
        //public ActionResult Carts() 
        //{
        //    if (Request["UserId"]!="")
        //    {
        //        int userId = Convert.ToInt32(Request["UserId"]);
        //        string[] allKeys = Request.Form.AllKeys;
        //        List<int> bookIdList = new List<int>();
        //        foreach (string key in allKeys)
        //        {
        //            if (key.StartsWith("ck_"))
        //            {
        //                string k = key.Replace("ck_", "");
        //                bookIdList.Add(Convert.ToInt32(k));
        //            }
        //        }
        //        if (bookIdList.Count == 0)
        //        {
        //            return Content("noCheck");
        //        }
        //        else 
        //        {
                    
        //            foreach (int bookId in bookIdList)
        //            {
        //                Cart cart = cartService.LoadEntities(c=>c.UserId==userId&&c.BookId==bookId).FirstOrDefault();
        //                cartList.Add(cart);
        //            }
        //            return Content("ok");
        //        }
        //    }
        //    else
        //    {
        //        return Content("noLogin");
        //    }
        //}

    }
}
