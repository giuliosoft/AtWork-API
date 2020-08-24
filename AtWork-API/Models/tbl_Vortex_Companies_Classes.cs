namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Vortex_Companies_Classes
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string classUniqueID { get; set; }

        [StringLength(10)]
        public string showAcctMgnt { get; set; }

        [StringLength(10)]
        public string grpFilter { get; set; }

        [StringLength(10)]
        public string ShowEmpProfileEdit { get; set; }
    }
}
