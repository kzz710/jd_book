using Bookshop.Common;
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
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        IBooksService booksService {get;set; }
        IUsersService usersService { get; set; }
        ICartService cartService { get; set; }
        IAddressInfoService addressInfoService { get; set; }
        /// <summary>
        /// 购物车首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (CheckIsLogin())
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                object obj = MemcacheHelper.Get(sessionId);
                Users user = SerializeHelper.DeserializeToObject<Users>(obj.ToString());
                ViewBag.User = user;
                List<Cart> list = cartService.LoadEntities(c => c.UserId == user.Id).ToList();
                List<AddressInfo> addList = addressInfoService.LoadEntities(a => a.UserId == user.Id).ToList();
                if (addList.Count != 0)
                {
                    ViewBag.AddressList = addList;

                }
                else
                {
                    ViewBag.AddressList = null;

                }
                List<CommodityViewModel> cvmList = new List<CommodityViewModel>();
                if (list.Count!=0)
                {
                    foreach (Cart cart in list)
                    {
                        CommodityViewModel cvm = new CommodityViewModel();
                        cvm.book = cart.Books;
                        cvm.count = cart.Count;
                        cvmList.Add(cvm);
                    }
                    ViewBag.CommodityList = cvmList;
                }
                else
                {
                    ViewBag.CommodityList = null;
                }
                
            }
            else 
            {
                //没有登录直接判断cookie中是否有值，然后根据结果去生成页面
                if (Request.Cookies["commodity"]!=null)
                {
                    List<BookViewModel> list = SerializeHelper.DeserializeToObject<List<BookViewModel>>(Request.Cookies["commodity"].Value);
                    List<CommodityViewModel> cvmList = new List<CommodityViewModel>();
                    foreach (BookViewModel bvm in list)
                    {
                        CommodityViewModel cvm = new CommodityViewModel();
                        cvm.book = booksService.LoadEntities(b=>b.Id==bvm.bookId).FirstOrDefault();
                        cvm.count = bvm.count;
                        cvmList.Add(cvm);
                    }
                    ViewBag.CommodityList = cvmList;
                }
                else
                {
                    
                    ViewBag.CommodityList = null;
                }
            }
            return View();
        }
        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCommodity() 
        {
            string action=Request["action"];
            int bookId=Convert.ToInt32(Request["bookId"]);
            int count = Convert.ToInt32(Request["count"]);
            //检查用户是否登录。如果没有登录，就将商品添加至cookie中
            if (CheckIsLogin())
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                object obj = MemcacheHelper.Get(sessionId);
                Users user = SerializeHelper.DeserializeToObject<Users>(obj.ToString());
                List<Cart> list = cartService.LoadEntities(c=>c.UserId==user.Id).ToList();
                if (list.Count==0)
                {
                    Cart cart = new Cart();
                    cart.UserId = user.Id;
                    cart.BookId = bookId;
                    cart.Count = 1;
                    if (cartService.AddEntity(cart) != null)
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
                    if (action.Equals("oneAdd"))
                    {                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].BookId == bookId)
                            {
                                list[i].Count += 1;
                                if (cartService.EditEntity(list[i]))
                                {
                                    return Content("ok");
                                }
                                else
                                {
                                    return Content("no");
                                }
                                
                            }
                        }
                        Cart cart = new Cart();
                        cart.UserId = user.Id;
                        cart.BookId = bookId;
                        cart.Count = 1;
                        if (cartService.AddEntity(cart) != null)
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
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].BookId == bookId)
                            {
                                list[i].Count =count;
                                if (cartService.EditEntity(list[i]))
                                {
                                    return Content("ok");
                                }
                                else
                                {
                                    return Content("no");
                                }

                            }
                        }
                        return Content("no");
                    }
                    
                }
            }
            else 
            {
                Books book=booksService.LoadEntities(b=>b.Id==bookId).FirstOrDefault();
                HttpCookie ck=Request.Cookies["commodity"];
                //先判断cookie中是否有值，如果没值，就直接添加，有值需先判断是否已经有该商品，有就直接在数量上+1，没有就继续添加
                if (ck==null)
                {
                    ck = new HttpCookie("commodity");
                    List<BookViewModel> list = new List<BookViewModel>();
                    BookViewModel bvm = new BookViewModel();
                    bvm.bookId = bookId;
                    bvm.count = 1;
                    list.Add(bvm);
                    ck.Value = SerializeHelper.SerializeToString(list);
                    ck.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(ck);
                    
                }
                else
                {
                    bool isExist = false;
                    List<BookViewModel> list = SerializeHelper.DeserializeToObject<List<BookViewModel>>(ck.Value);
                    //判断用户是该改变input的值加的还是点击+加的
                    if (action.Equals("oneAdd"))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].bookId == bookId)
                            {
                                list[i].count += 1;
                                isExist = true;
                                break;
                            }
                        }
                        //判断cookie中是否存在该商品，存在就数量+1,不存在就添加该商品
                        if (!isExist)
                        {
                            BookViewModel newBvm = new BookViewModel();
                            newBvm.bookId = bookId;
                            newBvm.count = 1;
                            list.Add(newBvm);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].bookId == bookId)
                            {
                                list[i].count =count;                               
                                break;
                            }
                        }
                    }
                    
                    ck.Value = SerializeHelper.SerializeToString(list);
                    ck.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(ck);
                    
                }
                return Content("ok");
            }
        }
        /// <summary>
        /// 删除购物车中商品
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteCommodity() 
        {
            string action=Request["action"];
            int bookId=Convert.ToInt32(Request["bookId"]);
            //判断用户是否登录，没有登录就去cookie中删除商品
            if (CheckIsLogin())
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                object obj = MemcacheHelper.Get(sessionId);
                Users user = SerializeHelper.DeserializeToObject<Users>(obj.ToString());
                List<Cart> list = cartService.LoadEntities(c => c.UserId == user.Id).ToList();
                if (action.Equals("totalDelete"))
                {
                    foreach (Cart cart in list)
                    {
                        if (cart.BookId==bookId)
                        {
                            if (cartService.DeleteEntity(cart)) 
                            {
                                return Content("ok");
                            }
                            else
                            {
                                return Content("no");
                            } 
                        }
                    }
                    return Content("no");
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].BookId==bookId)
                        {
                            if (list[i].Count>1)
                            {
                                list[i].Count--;
                                if (cartService.EditEntity(list[i]))
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
                                if (cartService.DeleteEntity(list[i]))
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
                    return Content("no");
                }
            }
            else
            {
                
                bool isSuccess = false;
                HttpCookie ck = Request.Cookies["commodity"];
                List<BookViewModel> list = SerializeHelper.DeserializeToObject<List<BookViewModel>>(ck.Value);
                BookViewModel rbvm = new BookViewModel();
                //判断用户是点击删除按钮还是点击-号
                if (action.Equals("totalDelete"))
                {
                    foreach (BookViewModel bvm in list)
                    {
                        if (bvm.bookId == bookId)
                        {
                            list.Remove(bvm);
                            isSuccess = true;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].bookId == bookId)
                        {
                            //点击-号进行删除，需判断商品数量是否大于1，大于数量减一，否则移除该商品
                            if (list[i].count > 1)
                            {
                                list[i].count--;
                            }
                            else
                            {
                                rbvm = list[i];
                                list.Remove(rbvm);

                            }
                            isSuccess = true;
                            break;
                        }
                    }
                }
                
                if (isSuccess)
                {
                    //移除商品后，判断list中是否还有商品，如果没有就删除cookie
                    if (list.Count==0)
                    {
                        ck.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(ck);
                        Request.Cookies.Remove("commodity");
                    }
                    else
                    {
                        ck.Value = SerializeHelper.SerializeToString(list);
                        ck.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(ck);
                    }
                    
                    return Content("ok");
                }
                return Content("no");
                
                
            }
        }

        /// <summary>
        /// 得到购物车中商品的数量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCommodityCount() 
        {
            if (CheckIsLogin())
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                object obj = MemcacheHelper.Get(sessionId);
                Users user = SerializeHelper.DeserializeToObject<Users>(obj.ToString());
                List<Cart> list = cartService.LoadEntities(c => c.UserId == user.Id).ToList();
                return Content((list.Count).ToString());
            }
            else
            {
                if (Request.Cookies["commodity"] != null)
                {
                    List<BookViewModel> list = SerializeHelper.DeserializeToObject<List<BookViewModel>>(Request.Cookies["commodity"].Value);
                    return Content((list.Count).ToString());
                }
                else
                {
                    return Content("0");
                }
            }
        }
        /// <summary>
        /// 检查用户是否登录
        /// </summary>
        /// <returns></returns>
        public bool CheckIsLogin() 
        {
            bool isLogin = false;
            if (Request.Cookies["sessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                object obj = MemcacheHelper.Get(sessionId);
                if (obj != null)
                {
                    isLogin = true;
                }
            }
            return isLogin;
        }

    }
}
