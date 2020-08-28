namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Companies
    {
        public int id { get; set; }

        [StringLength(100)]
        public string coUniqueID { get; set; }

        [StringLength(100)]
        public string parentCoUniqueID { get; set; }

        [StringLength(150)]
        public string coName { get; set; }

        [StringLength(350)]
        public string coContactName { get; set; }

        [StringLength(250)]
        public string coEmail { get; set; }

        [StringLength(150)]
        public string coUserName { get; set; }

        [StringLength(250)]
        public string coPassword { get; set; }

        [StringLength(250)]
        public string coInvAddress1 { get; set; }

        [StringLength(250)]
        public string coInvAddress2 { get; set; }

        [StringLength(150)]
        public string coInvCity { get; set; }

        [StringLength(50)]
        public string coInvPostalCode { get; set; }

        [StringLength(50)]
        public string coInvCountry { get; set; }

        [StringLength(250)]
        public string coLocation { get; set; }

        [StringLength(250)]
        public string coAddress1 { get; set; }

        [StringLength(250)]
        public string coAddress2 { get; set; }

        [StringLength(150)]
        public string coCity { get; set; }

        [StringLength(50)]
        public string coPostalCode { get; set; }

        [StringLength(50)]
        public string coCountry { get; set; }

        [StringLength(50)]
        public string coContinent { get; set; }

        [StringLength(50)]
        public string coPhone { get; set; }

        [StringLength(250)]
        public string coLogo { get; set; }

        [StringLength(450)]
        public string coURL { get; set; }

        [Column(TypeName = "text")]
        public string coDescription { get; set; }

        [StringLength(250)]
        public string coIndustry { get; set; }

        public int? coMaxEmpl { get; set; }

        [StringLength(150)]
        public string coPackage { get; set; }

        [StringLength(50)]
        public string coTermsAccepted { get; set; }

        public DateTime? dateNewCompanySent { get; set; }

        [StringLength(50)]
        public string onBoardStatus { get; set; }

        public int? coEmplOnBoardCompleted { get; set; }

        public int? coEmplOnboardSent { get; set; }

        [StringLength(50)]
        public string coCompetenciesStatus { get; set; }

        [StringLength(100)]
        public string coWhiteLabel { get; set; }

        [StringLength(10)]
        public string coAllowPosts { get; set; }

        [StringLength(50)]
        public string coWhiteLabelPicStatus { get; set; }

        [StringLength(50)]
        public string coWhiteLabelGTPicStatus { get; set; }

        [StringLength(50)]
        public string Accent { get; set; }
        [StringLength(50)]
        public string Dark { get; set; }
        [StringLength(50)]
        public string Secondary_Dark { get; set; }
        [StringLength(50)]
        public string Light { get; set; }
        [StringLength(50)]
        public string Secondary_Light { get; set; }
        ///public string volUniqueID { get; set; }
    }
}
