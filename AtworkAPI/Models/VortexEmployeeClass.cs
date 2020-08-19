namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VortexEmployeeClass
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(50)]
        public string classUniqueID { get; set; }

        [StringLength(150)]
        public string classValue { get; set; }
    }
}
