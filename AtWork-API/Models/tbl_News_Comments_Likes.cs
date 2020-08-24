namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_News_Comments_Likes
    {
        public int Id { get; set; }

        public int newsCommentId { get; set; }

        [StringLength(50)]
        public string likeByID { get; set; }

        public DateTime? likeDate { get; set; }

        public virtual tbl_News_Comments tbl_News_Comments { get; set; }
    }
}
