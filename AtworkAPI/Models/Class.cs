namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class
    {
        public int id { get; set; }

        [StringLength(50)]
        public string classUniqueID { get; set; }

        [StringLength(100)]
        public string classDescription { get; set; }
    }
}
