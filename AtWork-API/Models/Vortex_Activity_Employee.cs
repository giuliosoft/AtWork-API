using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class Vortex_Activity_Employee
    {
        public int id { get; set; }

        public string coUniqueID { get; set; }

        public string proUniqueID { get; set; }

        public string volUniqueID { get; set; }

        public string volTransport { get; set; }

        public string volDiet { get; set; }

        public string proStatus { get; set; }

        public DateTime? proChosenDate { get; set; }

        public DateTime? proVolHourDates { get; set; }
    }
}