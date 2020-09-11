using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class Activity_Feedback
    {
        public int id { get; set; }
        public string coUniqueID { get; set; }
        public string proUniqueID { get; set; }
        public string volUniqueID { get; set; }
        public DateTime? ActivityDate { get; set; }
        public int? selectedStarRating { get; set; }
        public string ActivityFeedback_Like { get; set; }
        public int? SliderValue { get; set; }
        public string ActivityFeedbackFeeling { get; set; }
        public string ActivityFeedbackImprove { get; set; }
        public string ActivityFeedbackComments { get; set; }
        public string ActivityFeedbackAdditional { get; set; }
        public int? SliderValue2 { get; set; }
    }
}