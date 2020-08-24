namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_CMSContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(150)]
        public string language { get; set; }

        [StringLength(150)]
        public string pagename { get; set; }

        [StringLength(150)]
        public string pagesection { get; set; }

        [StringLength(150)]
        public string pagesectionTitle { get; set; }

        public string pagecontent { get; set; }
    }
}
