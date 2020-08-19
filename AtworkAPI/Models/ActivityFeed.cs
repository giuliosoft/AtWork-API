using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtworkAPI.Models
{
    public class ActivityFeed
    {
        public string UniqueID { get; set; }
        public string Title { get; set; }
        public string coUniqueID { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public string SubCategory { get; set; }
        public string SubCategoryName { get; set; }
        public string Status { get; set; }
        public string AddActivity_HoursCommitted { get; set; }
        public string AddActivity_StartTime { get; set; }
        public string AddActivity_EndTime { get; set; }
        public string AddActivity_ParticipantsMinNumber { get; set; }
        public string AddActivity_ParticipantsMaxNumber { get; set; }
        public string BackgroundImage { get; set; }
    }
}
