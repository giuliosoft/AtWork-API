using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace AtWork_API.Models
{
    public class CommonMethods
    {
        public ModelContext db = new ModelContext();
        public static string getRelativeDateTime(DateTime objDateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - objDateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
        public tbl_Volunteers getCurentUser(string token)
        {
            string encodedHeader = token.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

            int separatorIndex = usernamePassword.IndexOf(":");
            string username = usernamePassword.Substring(0, separatorIndex);
            string password = usernamePassword.Substring(separatorIndex + 1);

            var Volunteers = db.tbl_Volunteers.FirstOrDefault(u => u.volUserName == username && u.VolUserPassword == password);
            return Volunteers;
        }
    }

    public class CommonResponse
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Data1 { get; set; }
    }
    public class Message
    {
        public const string AlreadyExistsMessage = "Record already exists.";
        public const string InsertSuccessMessage = "Record saved successfully.";
        public const string UpdateSuccessMessage = "Record updated successfully.";
        public const string DeleteSuccessMessage = "Record deleted successfully.";
        public const string NoRecordMessage = "No record found.";
        public const string ErrorMessage = "Something want worng.";
        public const string GetData = "Success";
    }
}