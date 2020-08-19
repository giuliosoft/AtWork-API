namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeedbackFeeling
    {
        public int id { get; set; }

        [StringLength(50)]
        public string ActivityFeedbackFeeling { get; set; }

        [StringLength(50)]
        public string status { get; set; }
    }
}
