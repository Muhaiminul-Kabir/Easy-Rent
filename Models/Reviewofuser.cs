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
    
    public partial class Reviewofuser
    {
        public int id { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<int> infoid { get; set; }
        public Nullable<int> reviewerid { get; set; }
    
        public virtual reviewinfo reviewinfo { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
