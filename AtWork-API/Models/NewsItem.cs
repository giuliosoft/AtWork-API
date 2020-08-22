using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public partial class NewsItem
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string newsUniqueID { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(450)]
        public string newsTitle { get; set; }

        [Column(TypeName = "text")]
        public string newsContent { get; set; }

        public DateTime? newsDateTime { get; set; }

        public DateTime? newsPostedTime { get; set; }

        [StringLength(50)]
        public string newsPrivacy { get; set; }

        [StringLength(50)]
        public string newsStatus { get; set; }

        [StringLength(50)]
        public string newsOrigin { get; set; }

        [StringLength(100)]
        public string newsImage { get; set; }

        [StringLength(100)]
        public string newsFile { get; set; }

        [StringLength(100)]
        public string newsFileOriginal { get; set; }
    }
}