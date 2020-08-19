namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NewsComment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NewsComment()
        {
            tbl_News_Comments_Likes = new HashSet<NewsCommentLike>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string newsUniqueID { get; set; }

        [StringLength(50)]
        public string comByID { get; set; }

        public DateTime? comDate { get; set; }

        [Column(TypeName = "text")]
        public string comContent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsCommentLike> tbl_News_Comments_Likes { get; set; }
    }
}
