namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NewsCommentLike
    {
        public int Id { get; set; }

        public int newsCommentId { get; set; }

        [StringLength(50)]
        public string likeByID { get; set; }

        public DateTime? likeDate { get; set; }

        public virtual NewsComment tbl_News_Comments { get; set; }
    }
}
