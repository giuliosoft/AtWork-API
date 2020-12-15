using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class Notification
    {

        
        public string volUniqueId { get; set; }
        public bool IsPaused { get; set; }
        public string PauseTime { get; set; }
        public bool IsForever { get; set; }
        public int PauseTimeMinute { get; set; }
        public DateTime PauseNotificationStarttime { get; set; }
        public DateTime PauseNotificationEndtime { get; set; }
        public string FormattedDate { get; set; }
        public bool? IsDisplayMsg { get; set; }

        //public Connect_Notification_Setting connect_Notification_Setting { get; set; }

        //public Activity_Notification_Setting activity_Notification_Setting { get; set; }



    }
}