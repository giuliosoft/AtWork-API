using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtWork_API
{
    public class Activities
    {
        public int id { get; set; }


        public string proUniqueID { get; set; }

        public string proTitle { get; set; }

        public string coUniqueID { get; set; }

        public string proCompany { get; set; }

        public string proDescription { get; set; }

        public string proLocation { get; set; }

        public string proAddress1 { get; set; }

        public string proAddress2 { get; set; }

        public string proPostalCode { get; set; }

        public string proCity { get; set; }

        public string proCountry { get; set; }

        public string proContinent { get; set; }

        public string proTimeCommitment { get; set; }

        public string proTimeCommitmentDecimal { get; set; }

        public string proDatesConfirmed { get; set; }

        public string proType { get; set; }

        public string proCategory { get; set; }

        public string proCategoryName { get; set; }

        public string proSubCategory { get; set; }

        public string proSubCategoryName { get; set; }

        public string proPartner { get; set; }

        public string proPartnerEmail { get; set; }

        public string proActivityLanguage { get; set; }

        public string proActivityLanguageID { get; set; }

        public string proAudience { get; set; }

        public string proSpecialRequirements { get; set; }

        public string proCostCoveredEmployee { get; set; }

        public string proCostCoveredCompany { get; set; }

        public string proAddActivity_HoursCommitted { get; set; }

        public string proAddActivity_StartTime { get; set; }

        public string proAddActivity_EndTime { get; set; }

        public string proAddActivity_ParticipantsMinNumber { get; set; }

        public string proAddActivity_ParticipantsMaxNumber { get; set; }

        public string proAddActivity_OrgName { get; set; }

        public string proAddActivity_Website { get; set; }

        public string proAddActivity_AdditionalInfo { get; set; }

        public string proAddActivity_CoordinatorEmail { get; set; }

        public DateTime? proPublishedDate { get; set; }

        public DateTime? proAddActivityDate { get; set; }
        public string proBackgroundImage { get; set; }

        public string proDeliveryMethod { get; set; }
        public int JoinActivityId { get; set; }
        public string proStatus { get; set; }
        public string Member { get; set; }
        public string volUniqueID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DataType { get; set; }
        public string proVolHourDates { get; set; }
        public string Companie_Name { get; set; }
        public string Companie_Address1 { get; set; }
        public string Companie_Address2 { get; set; }
        public string Keyword { get; set; }
    }
}