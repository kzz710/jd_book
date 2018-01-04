using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookshop.Model;

namespace jd_BookShop.Models
{
    public class OrderViewModel
    {
        public string orderId { get; set; }
        public List<OrderBook> orderBookList = new List<OrderBook>();
        public decimal totalPrice { get; set; }
        public string postAddress { get; set; }
    }
}