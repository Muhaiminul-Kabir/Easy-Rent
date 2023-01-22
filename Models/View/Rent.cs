using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectsd.Models.View
{
    public class Rent 
    {
        public int ID { get; set; }
        public int? price { get; set; }
        public string Location { get; set; }
        public DateTime? posted { get; set; }
        public string pic { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? expiryDate { get; set; }
        public int tenantid { get; set; }
        public bool isAvaliable = false;
    }
}