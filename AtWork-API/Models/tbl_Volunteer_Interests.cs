namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Volunteer_Interests
    {
        public int id { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(500)]
        public string volInterest { get; set; }
    }
}
