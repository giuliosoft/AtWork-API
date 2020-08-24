namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_EducationLevels
    {
        public int id { get; set; }

        [StringLength(150)]
        public string eduName { get; set; }

        [StringLength(150)]
        public string eduValue { get; set; }
    }
}
