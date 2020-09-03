using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class News_Likes
    {
        public int Id { get; set; }
        public int newsId { get; set; }
        public string likeByID { get; set; }
        public DateTime? likeDate { get; set; }
    }
}