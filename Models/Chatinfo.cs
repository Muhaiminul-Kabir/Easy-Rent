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
    
    public partial class Chatinfo
    {
        public Chatinfo()
        {
            this.Chats = new HashSet<Chat>();
        }
    
        public int id { get; set; }
        public string text { get; set; }
        public Nullable<System.DateTime> time { get; set; }
    
        public virtual ICollection<Chat> Chats { get; set; }
    }
}
