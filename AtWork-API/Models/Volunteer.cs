namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Volunteer
    {
        public int id { get; set; }

        [StringLength(50)]
        public string coUniqueID { get; set; }

        [StringLength(50)]
        public string volUniqueID { get; set; }

        [StringLength(150)]
        public string volFirstName { get; set; }

        [StringLength(150)]
        public string volLastName { get; set; }

        [StringLength(150)]
        public string volGender { get; set; }

        [StringLength(150)]
        public string volUserName { get; set; }

        [StringLength(50)]
        public string volUserPassword { get; set; }

        [StringLength(250)]
        public string volEmail { get; set; }

        [StringLength(50)]
        public string volOnBoardStatus { get; set; }

        public DateTime? volOnBoardDateSent { get; set; }

        [StringLength(150)]
        public string volPicture { get; set; }

        [StringLength(150)]
        public string volEducation { get; set; }

        [StringLength(150)]
        public string volCompetencies { get; set; }

        [StringLength(150)]
        public string volCategories { get; set; }

        [StringLength(50)]
        public string volPhone { get; set; }

        [StringLength(50)]
        public string volStatus { get; set; }

        [StringLength(500)]
        public string restricted { get; set; }

        [StringLength(100)]
        public string reviewStatus { get; set; }

        public DateTime? reviewDate { get; set; }

        [StringLength(50)]
        public string volDepartment { get; set; }

        [StringLength(50)]
        public string volLanguage { get; set; }

        [Column(TypeName = "text")]
        public string volAbout { get; set; }

        [StringLength(500)]
        public string volInterests { get; set; }
    }
}
