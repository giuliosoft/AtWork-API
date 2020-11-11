﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API.Models
{
    public class Volunteers
    {
        public int id { get; set; }

        public string coUniqueID { get; set; }

        public string volUniqueID { get; set; }

        public string volFirstName { get; set; }

        public string volLastName { get; set; }

        public string volGender { get; set; }

        public string volUserName { get; set; }

        public string VolUserPassword { get; set; }

        public string volEmail { get; set; }

        public string volOnBoardStatus { get; set; }

        public DateTime? volOnBoardDateSent { get; set; }

        public string volPicture { get; set; }

        public string volEducation { get; set; }

        public string volCompetencies { get; set; }

        public string volCategories { get; set; }

        public string volPhone { get; set; }

        public string volStatus { get; set; }

        public string restricted { get; set; }

        public string reviewStatus { get; set; }

        public DateTime? reviewDate { get; set; }

        public string volDepartment { get; set; }

        public string volLanguage { get; set; }

        public string volAbout { get; set; }

        public string volInterests { get; set; }

        public string volHours { get; set; }

        public int Vortex_Activity_Count { get; set; }
        public List<VolunteerClasses> VolunteerClasses { get; set; }
        public string classes { get; set; }
        public string oldPassword { get; set; }
        public string volDefaultPicture { get; set; }
        public string CategoryActivityCount { get; set; }
        public string CategorywiseHourCount { get; set; }
        public string VolBirthday { get; set; }
        public bool volShowBirthday { get; set; }
        public VolunteerBirthday VolunteerBirthday { get; set; }
        public string coLocation { get; set; }
        public string StartDate { get; set; }
        public string EmployeeID { get; set; }
        public string CustomField { get; set; }
    }
    public class VolunteerClasses
    {
        public string classUniqueID { get; set; }
        public string classDescription { get; set; }
        public string classValue { get; set; }
        public string grpFilter { get; set; }
    }
    public class VolunteerBirthday
    {
        public int volBirthMonth { get; set; }
        public int volBirthDay { get; set; }
        public int volBirthYear { get; set; }
        public bool volShowBirthday { get; set; }
    }


}