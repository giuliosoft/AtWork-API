namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NewsReview
    {
        public int id { get; set; }

        [StringLength(70)]
        public string coUniqueID { get; set; }

        [StringLength(70)]
        public string newsUniqueID { get; set; }

        [StringLength(70)]
        public string reviewerID { get; set; }

        public DateTime? reviewDate { get; set; }
    }
}
