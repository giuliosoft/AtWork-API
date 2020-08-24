namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_News_Employees_Custom
    {
        public int id { get; set; }

        [StringLength(150)]
        public string coUniqueID { get; set; }

        [StringLength(150)]
        public string proUniqueID { get; set; }

        [StringLength(150)]
        public string volUniqueID { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }
    }
}
