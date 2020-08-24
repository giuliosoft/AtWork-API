namespace AtWork_API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_MobilePreferences
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserEmail { get; set; }

        [StringLength(50)]
        public string DeviceId { get; set; }

        [StringLength(50)]
        public string LastSessionDate { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(250)]
        public string SessionToken { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string SetupComplete { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AllowNotifications { get; set; }

        public byte[] ProfileImage { get; set; }

        [StringLength(25)]
        public string DisclosureDate { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AcceptDisclosure { get; set; }

        [StringLength(250)]
        public string Interests { get; set; }
    }
}
