namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityFeedback
    {
        public int id { get; set; }

        [StringLength(70)]
        public string coUniqueID { get; set; }

        [StringLength(70)]
        public string proUniqueID { get; set; }

        [StringLength(70)]
        public string volUniqueID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ActivityDate { get; set; }

        public int? selectedStarRating { get; set; }

        [Column(TypeName = "text")]
        public string ActivityFeedback_Like { get; set; }

        public int? SliderValue { get; set; }

        [StringLength(50)]
        public string ActivityFeedbackFeeling { get; set; }

        [StringLength(60)]
        public string ActivityFeedbackImprove { get; set; }

        [Column(TypeName = "text")]
        public string ActivityFeedbackComments { get; set; }

        [Column(TypeName = "text")]
        public string ActivityFeedbackAdditional { get; set; }

        public int? SliderValue2 { get; set; }
    }
}
