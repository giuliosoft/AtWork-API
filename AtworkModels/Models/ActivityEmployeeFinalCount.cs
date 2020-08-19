namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityEmployeeFinalCount
    {
        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(100)]
        public string proUniqueID { get; set; }

        [StringLength(450)]
        public string proTitle { get; set; }

        [StringLength(250)]
        public string proCategoryName { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }

        [StringLength(150)]
        public string proCity { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TotalEmployees { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proPublishedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proAddActivityDate { get; set; }
    }
}
