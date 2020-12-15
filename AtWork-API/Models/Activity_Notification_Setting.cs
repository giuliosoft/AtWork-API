using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class Activity_Notification_Setting
    {
        public string volUniqueId { get; set; }
        public bool Active_IsYGTSomeoneRegister { get; set; }
        public bool Active_IsYGTSomeoneCancelled { get; set; }
        public bool Active_IsAllActActivityCancelled { get; set; }
        public bool Active_IsAllActActivityReminder { get; set; }
        public bool Active_IsAllActFeedbackReminder { get; set; }
        public bool Active_IsPetitionsStatusUpdate { get; set; }

    }
}