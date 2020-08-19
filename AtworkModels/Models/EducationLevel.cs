namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EducationLevel
    {
        public int id { get; set; }

        [StringLength(150)]
        public string eduName { get; set; }

        [StringLength(150)]
        public string eduValue { get; set; }
    }
}
