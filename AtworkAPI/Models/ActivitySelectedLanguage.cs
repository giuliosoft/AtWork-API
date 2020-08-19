namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivitySelectedLanguage
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string proUniqueID { get; set; }

        [StringLength(50)]
        public string proLanguageID { get; set; }

        [StringLength(50)]
        public string proLanguage { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }
    }
}
