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
    
    public partial class Users
    {
        public Users()
        {
            this.Cart = new HashSet<Cart>();
            this.Orders = new HashSet<Orders>();
        }
    
        public int Id { get; set; }
        public string LoginId { get; set; }
        public string LoginPwd { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public int UserStateId { get; set; }
    
    	[JsonIgnore]
        public virtual ICollection<Cart> Cart { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Orders> Orders { get; set; }
    	[JsonIgnore]
        public virtual UserStates UserStates { get; set; }
    }
}
