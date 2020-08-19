using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtworkAPI.Models
{
    public class MobilePreference
    {
        public string ID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string DeviceId { get; set; }
        public string LastSessionDate { get; set; }
        public string Language { get; set; }
        public string SessionToken { get; set; }
        public string SetupComplete { get; set; }
        public int AllowNotifications { get; set; }
        public object ProfileImage { get; set; }
        public string DisclosureDate { get; set; }
        public int AcceptDisclosure { get; set; }
        public string  Interests { get; set; }
    }
}
