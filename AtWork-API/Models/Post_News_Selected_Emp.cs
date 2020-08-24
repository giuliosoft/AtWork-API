namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post_News_Selected_Emp
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string newsUniqueID { get; set; }

        [StringLength(50)]
        public string VolUniqueID { get; set; }

        [StringLength(50)]
        public string Attribute { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string NewsPrivacy { get; set; }
    }
}
