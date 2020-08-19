using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AtworkAPI.Models
{
    public class MobileInterest
    {
        [Key]
        public string Interest { get; set; }
    }
}