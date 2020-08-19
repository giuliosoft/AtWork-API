namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomKeyword
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string keywordGroup { get; set; }

        [StringLength(50)]
        public string keywordUniqueID { get; set; }

        [StringLength(50)]
        public string keywordName { get; set; }

        [StringLength(50)]
        public string keywordDescription { get; set; }
    }
}