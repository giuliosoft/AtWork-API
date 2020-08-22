using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public string SetupComplete { get; set; }
        public string CompanyId { get; set; }
    }
}