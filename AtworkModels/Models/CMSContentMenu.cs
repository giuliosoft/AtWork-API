namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMSContentMenu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(150)]
        public string menuGroup { get; set; }

        [StringLength(150)]
        public string menuTitle { get; set; }

        [StringLength(150)]
        public string menuValue { get; set; }

        [StringLength(100)]
        public string status { get; set; }
    }
}
