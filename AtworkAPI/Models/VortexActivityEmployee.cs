namespace AtworkAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VortexActivityEmployee
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string proUniqueID { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(50)]
        public string volTransport { get; set; }

        [StringLength(50)]
        public string volDiet { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proChosenDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proVolHourDates { get; set; }
    }
}
