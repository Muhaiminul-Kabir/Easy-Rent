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
    
    public partial class Payment
    {
        public int id { get; set; }
        public Nullable<int> infoid { get; set; }
        public Nullable<int> rentid { get; set; }
    
        public virtual Payinfo Payinfo { get; set; }
        public virtual Rent Rent { get; set; }
    }
}
