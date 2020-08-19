using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtworkAPI.Models
{
    public class ProfileRecord
    {
        public int id { get; set; }

        public string coUniqueID { get; set; }

        public string volUniqueID { get; set; }

        public string volFirstName { get; set; }

        public string volLastName { get; set; }

        public string volGender { get; set; }

        public string volUserName { get; set; }

        public string volUserPassword { get; set; }

        public string volEmail { get; set; }

        public string volOnBoardStatus { get; set; }

        public string volOnBoardDateSent { get; set; }

        public string volPicture { get; set; }

        public string volEducation { get; set; }

        public string volCompetencies { get; set; }

        public string volCategories { get; set; }

        public string volPhone { get; set; }

        public string volStatus { get; set; }

        public string restricted { get; set; }

        public string reviewStatus { get; set; }

        public string reviewDate { get; set; }

        public string volDepartment { get; set; }

        public string volLanguage { get; set; }

        public string volAbout { get; set; }

        public string volInterests { get; set; }
    }
}
