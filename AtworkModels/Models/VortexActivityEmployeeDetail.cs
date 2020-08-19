namespace AtworkModels.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VortexActivityEmployeeDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(100)]
        public string proUniqueID { get; set; }

        [StringLength(450)]
        public string proTitle { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(450)]
        public string proCompany { get; set; }

        [Column(TypeName = "text")]
        public string proDescription { get; set; }

        [StringLength(350)]
        public string proLocation { get; set; }

        [StringLength(350)]
        public string proAddress1 { get; set; }

        [StringLength(350)]
        public string proAddress2 { get; set; }

        [StringLength(50)]
        public string proPostalCode { get; set; }

        [StringLength(150)]
        public string proCity { get; set; }

        [StringLength(150)]
        public string proCountry { get; set; }

        [StringLength(150)]
        public string proContinent { get; set; }

        [StringLength(50)]
        public string proTimeCommitment { get; set; }

        [StringLength(100)]
        public string proDatesConfirmed { get; set; }

        [StringLength(250)]
        public string proType { get; set; }

        [StringLength(250)]
        public string proCategory { get; set; }

        [StringLength(250)]
        public string proCategoryName { get; set; }

        [StringLength(80)]
        public string proSubCategory { get; set; }

        [StringLength(100)]
        public string proSubCategoryName { get; set; }

        [StringLength(50)]
        public string proStatus { get; set; }

        [StringLength(250)]
        public string proPartner { get; set; }

        [StringLength(250)]
        public string proPartnerEmail { get; set; }

        [StringLength(50)]
        public string proActivityLanguage { get; set; }

        [StringLength(50)]
        public string proActivityLanguageID { get; set; }

        [StringLength(50)]
        public string proAudience { get; set; }

        [Column(TypeName = "text")]
        public string proSpecialRequirements { get; set; }

        [StringLength(50)]
        public string proCostCoveredEmployee { get; set; }

        [StringLength(50)]
        public string proCostCoveredCompany { get; set; }

        [StringLength(50)]
        public string proAddActivity_HoursCommitted { get; set; }

        [StringLength(50)]
        public string proAddActivity_StartTime { get; set; }

        [StringLength(50)]
        public string proAddActivity_EndTime { get; set; }

        [StringLength(50)]
        public string proAddActivity_ParticipantsMinNumber { get; set; }

        [StringLength(50)]
        public string proAddActivity_ParticipantsMaxNumber { get; set; }

        [StringLength(50)]
        public string proAddActivity_OrgName { get; set; }

        [StringLength(200)]
        public string proAddActivity_Website { get; set; }

        [Column(TypeName = "text")]
        public string proAddActivity_AdditionalInfo { get; set; }

        [StringLength(50)]
        public string proAddActivity_CoordinatorEmail { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proPublishedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proAddActivityDate { get; set; }

        [StringLength(100)]
        public string proBackgroundImage { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(50)]
        public string VortexStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? proVolHourDates { get; set; }

        [StringLength(50)]
        public string dateType { get; set; }
    }
}
