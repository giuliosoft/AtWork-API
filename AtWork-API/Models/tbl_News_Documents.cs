namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_News_Documents
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(100)]
        public string newsUniqueID { get; set; }

        [StringLength(100)]
        public string docFileName { get; set; }

        [StringLength(100)]
        public string docFileNameOriginal { get; set; }

        [StringLength(50)]
        public string status { get; set; }
    }
}
