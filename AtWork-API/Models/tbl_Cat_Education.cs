namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Cat_Education
    {
        public int id { get; set; }

        [StringLength(50)]
        public string catUniqueID { get; set; }

        [StringLength(50)]
        public string subCatUniqueID { get; set; }

        [StringLength(50)]
        public string keywordGroup { get; set; }

        [StringLength(50)]
        public string keywordUniqueID { get; set; }
    }
}
