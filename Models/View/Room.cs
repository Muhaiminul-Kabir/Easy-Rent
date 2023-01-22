using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectsd.Models.View
{
    public class Rooms: Models.View.Rent
    {
        public List<Models.View.Facilities> facilities = new List<Models.View.Facilities>();
        public int? noofrooms { get; set; }
        public int? maxmembers { get; set; }
        public int? needmembers { get; set; }
        public int? size { get; set; }
    }
}