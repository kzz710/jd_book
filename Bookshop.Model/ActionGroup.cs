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
    
    public partial class ActionGroup
    {
        public ActionGroup()
        {
            this.ActionInfo = new HashSet<ActionInfo>();
            this.Role = new HashSet<Role>();
        }
    
        public int ID { get; set; }
        public string GroupName { get; set; }
        public short GroupType { get; set; }
        public string DelFlag { get; set; }
        public int Sort { get; set; }
    
    	[JsonIgnore]
        public virtual ICollection<ActionInfo> ActionInfo { get; set; }
    	[JsonIgnore]
        public virtual ICollection<Role> Role { get; set; }
    }
}