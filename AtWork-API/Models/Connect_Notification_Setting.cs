using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class Connect_Notification_Setting
    {
        public string volUniqueId { get; set; }
        public bool Connect_IsPostFromCompany { get; set; }
        public bool Connect_IsPostFromGroup { get; set; }
        public bool Connect_IsPostFromEveryone { get; set; }
        public bool Connect_IsLikesOnPosts { get; set; }
        public bool Connect_IsLikesOnComments { get; set; }
        public bool Connect_IsCommentsOnPosts { get; set; }
        public bool Connect_IsPostsYouComment { get; set; }

    }
}