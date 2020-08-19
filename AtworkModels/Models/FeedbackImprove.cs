namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeedbackImprove
    {
        public int id { get; set; }

        [StringLength(50)]
        public string ActivityFeedbackImprove { get; set; }

        [StringLength(50)]
        public string status { get; set; }
    }
}
