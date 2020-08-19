namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Audit
    {
        public int id { get; set; }

        [StringLength(50)]
        public string action { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        public DateTime? date { get; set; }
    }
}
