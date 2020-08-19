namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityDate
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string proUniqueID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dates { get; set; }

        [Column(TypeName = "date")]
        public DateTime? startDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? endDate { get; set; }

        [StringLength(50)]
        public string startTime { get; set; }

        [StringLength(50)]
        public string endTime { get; set; }

        [StringLength(50)]
        public string dateType { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }

        public string dateComment { get; set; }

        [StringLength(50)]
        public string timeCommitment { get; set; }

        [StringLength(50)]
        public string timeCommitmentDecimal { get; set; }
    }
}
