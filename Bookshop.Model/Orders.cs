//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bookshop.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        public Orders()
        {
            this.OrderBook = new HashSet<OrderBook>();
        }
    
        public string OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string PostAddress { get; set; }
        public int state { get; set; }
    
    	[JsonIgnore]
        public virtual ICollection<OrderBook> OrderBook { get; set; }
    	[JsonIgnore]
        public virtual Users Users { get; set; }
    }
}
