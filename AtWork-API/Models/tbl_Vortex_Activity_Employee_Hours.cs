namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Vortex_Activity_Employee_Hours
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string proUniqueID { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        public decimal? volHours { get; set; }

        public DateTime? proVolHourDates { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }
    }
}
