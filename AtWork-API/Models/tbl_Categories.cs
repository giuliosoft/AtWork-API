namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Categories
    {
        public int id { get; set; }

        [StringLength(100)]
        public string catName { get; set; }

        [StringLength(100)]
        public string catDescription { get; set; }

        [StringLength(100)]
        public string catUniqueID { get; set; }

        [StringLength(100)]
        public string catPicture { get; set; }
    }
}
