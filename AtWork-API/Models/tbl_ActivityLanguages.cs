namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_ActivityLanguages
    {
        public int id { get; set; }

        [StringLength(50)]
        public string languageID { get; set; }

        [StringLength(50)]
        public string language { get; set; }
    }
}
