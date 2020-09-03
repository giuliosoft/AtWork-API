using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class tbl_Join_Activities
    {
        public int Id { get; set; }
        public string coUniqueID { get; set; }
        public string proUniqueID { get; set; }
        public string volUniqueID { get; set; }
        public int ActivityID { get; set; }
    }
}