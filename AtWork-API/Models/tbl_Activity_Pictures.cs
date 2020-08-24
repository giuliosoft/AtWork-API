namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Activity_Pictures
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string proUniqueID { get; set; }

        [StringLength(100)]
        public string picUniqueID { get; set; }

        [StringLength(100)]
        public string picFileName { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }
    }
}
