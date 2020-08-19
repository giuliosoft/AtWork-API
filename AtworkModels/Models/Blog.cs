namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Blog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(500)]
        public string blgTitle { get; set; }

        [StringLength(500)]
        public string blgSEOTitle { get; set; }

        [StringLength(200)]
        public string blgAuthor { get; set; }

        [Column(TypeName = "date")]
        public DateTime? blgDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? blgReleaseDate { get; set; }

        public string blgTeaser { get; set; }

        [StringLength(200)]
        public string blgTeaserImage { get; set; }

        [StringLength(50)]
        public string blgIndicator { get; set; }

        [StringLength(50)]
        public string blgLanguage { get; set; }

        public string blgContent { get; set; }

        [StringLength(100)]
        public string blgStatus { get; set; }
    }
}
