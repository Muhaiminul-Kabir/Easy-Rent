//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace projectsd.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserContact
    {
        public UserContact()
        {
            this.Users = new HashSet<User>();
        }
    
        public int id { get; set; }
        public string email { get; set; }
        public string cell { get; set; }
        public string address { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}
