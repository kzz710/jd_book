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
    public class OrdersController : BaseController
    {
        //
        // GET: /Orders/
        IOrdersService ordersService { get; set; }
        IOrderBookService orderBookService { get; set; }
        ICartService cartService { get; set; }
        IAddressInfoService addressInfoService { get; set; }
        IBooksService booksService { get; set; }
        public ActionResult Index()
        {
            Users user = LoginUser;
            ViewBag.User = user;
            //查找当前用户的所有订单
            List<Orders> orderList = ordersService.LoadEntities(o=>o.UserId==user.Id).ToList();
            //创建一个viewmodel为了存储当前订单编号的具体内容
            List<OrderViewModel> ovmList = new List<OrderViewModel>();
            //根据订单编号查出当前订单的具体内容
            foreach (Orders order in orderList)
            {
                OrderViewModel ovm = new OrderViewModel();
                ovm.orderId = order.OrderId;
                ovm.totalPrice = order.TotalPrice;
                ovm.postAddress = order.PostAddress;
                List<OrderBook> obList = orderBookService.LoadEntities(o=>o.OrderID==order.OrderId).ToList();
                foreach (OrderBook ob in obList)
                {                 
                    ovm.orderBookList.Add(ob);
                }
                ovmList.Add(ovm);
            }
            if (ovmList.Count!=0)
            {
                ViewBag.OrderList = ovmList;
            }
            else
            {
                ViewBag.OrderList = null;
            }
            
            
            return View();
        }


        public ActionResult CreateOrder() 
        {
            if (Request["addressId"]!=null&&Request["booksId"]!=null)
            {
                Users user = LoginUser;
                int userId=user.Id;
                int addressId = Convert.ToInt32(Request["addressId"]);
                string booksId = Request["booksId"];
                List<int> list = new List<int>();
                //如果只选择一件商品，就只有一个ID，所以找不到，
                if (booksId.IndexOf(',')!=-1)
                {
                    string[] bIds = booksId.Split(',');
                    foreach (string bookId in bIds)
                    {
                        list.Add(Convert.ToInt32(bookId));
                    }
                }
                else
                {
                    list.Add(Convert.ToInt32(booksId));
                }
                List<Cart> cartList = new List<Cart>();
                foreach (int bookId in list)
                {
                    Cart cart = cartService.LoadEntities(c => c.BookId == bookId && c.UserId == userId).FirstOrDefault();
                    cartList.Add(cart);
                }
                AddressInfo address = addressInfoService.LoadEntities(a=>a.Id==addressId).FirstOrDefault();
                string postAddress = string.Format("收件人:{0},联系电话:{1},地址:{2},邮编:{3}",address.Name,address.Telphone,address.Address,address.ZipCode);
                string orderId=DateTime.Now.ToString("yyyyMMddHHmmssffff");
                decimal totalPrice = 0;
                foreach (Cart cart in cartList)
                {
                    Books book = booksService.LoadEntities(b => b.Id == cart.BookId).FirstOrDefault();
                    decimal unitPrice = book.UnitPrice;
                    int count = cart.Count;
                    totalPrice += unitPrice * count;
                }
                Orders order = new Orders() 
                {
                    OrderId=orderId,
                    OrderDate=DateTime.Now,
                    UserId=userId,
                    TotalPrice=totalPrice,
                    PostAddress=postAddress,
                    state=0
                };
                if (ordersService.AddEntity(order)!=null)
                {
                    foreach (Cart cart in cartList)
                    {
                        Books book = booksService.LoadEntities(b => b.Id == cart.BookId).FirstOrDefault();
                        decimal unitPrice = book.UnitPrice;
                        int count = cart.Count;                      
                        OrderBook orderBook = new OrderBook()
                        {
                            OrderID = orderId,
                            UnitPrice = unitPrice,
                            Quantity = count,
                            BookID = book.Id
                        };
                        orderBookService.AddEntity(orderBook);
                    }
                    foreach (Cart cart in cartList)
                    {
                        cartService.DeleteEntity(cart);
                    }
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }

                

            }
            else
            {
                return Content("noCheck");
            }
            

           
        }
    }
}
