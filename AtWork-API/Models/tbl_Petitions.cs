namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Petitions
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string petUniqueID { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(50)]
        public string catName { get; set; }

        [StringLength(50)]
        public string catUniqueID { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column(TypeName = "text")]
        public string motivation { get; set; }

        [StringLength(150)]
        public string ngoName { get; set; }

        [StringLength(150)]
        public string ngoContact { get; set; }

        [StringLength(50)]
        public string petStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? petDatePublished { get; set; }
    }
}
