namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_JobSync
    {
        public int ID { get; set; }

        public int jobType { get; set; }

        [Required]
        [StringLength(150)]
        public string jobKey { get; set; }

        public int jobStatus { get; set; }

        public DateTime jobBeginDateUtc { get; set; }

        public DateTime jobExpireDateUtc { get; set; }

        public DateTime? jobLastCompleteDateUtc { get; set; }

        public DateTime? jobLastErrorDateUtc { get; set; }

        [StringLength(200)]
        public string jobLastErrorMessage { get; set; }
    }
}
