using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
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

            var ts = new TimeSpan(TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now).Ticks  - objDateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
                return "just now";

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one sec. ago" : ts.Seconds + " sec. ago";

            if (delta < 2 * MINUTE)
                return "a min. ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " min. ago";

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

        public static int SendMail(string volEmail, string body, string Subject, string EmailCC, bool isForgetPassword = false)
        {
            try
            {
                string SMTP_LOGIN = string.Empty;
                string SMTP_Password = string.Empty;
                string SMTP_FROM_EMAIL = string.Empty;

                if (isForgetPassword)
                {
                    SMTP_LOGIN = ConfigurationManager.AppSettings["Forget_SMTP_LOGIN"].ToString();
                    SMTP_Password = ConfigurationManager.AppSettings["Forget_SMTP_Password"].ToString();
                    SMTP_FROM_EMAIL = ConfigurationManager.AppSettings["Forget_SMTP_FROM_EMAIL"].ToString();
                }
                else
                {
                    SMTP_LOGIN = ConfigurationManager.AppSettings["SMTP_LOGIN"].ToString();
                    SMTP_Password = ConfigurationManager.AppSettings["SMTP_Password"].ToString();
                    SMTP_FROM_EMAIL = ConfigurationManager.AppSettings["SMTP_FROM_EMAIL"].ToString();
                }
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTP_SERVER"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"].ToString()));
                NetworkCredential LoginInfo = new NetworkCredential(SMTP_LOGIN, SMTP_Password);



                client.UseDefaultCredentials = false;
                client.Credentials = LoginInfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SMTP_FROM_EMAIL, SMTP_FROM_EMAIL);
                mail.Subject = Subject;

                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.To.Add(volEmail);

                if (!string.IsNullOrEmpty(EmailCC))
                {
                    mail.CC.Add(EmailCC);//sender mail id

                    //mail.CC.Add(new MailAddress(EmailCC));
                    string[] CCId = EmailCC.Split(',');

                    foreach (string CCEmail in CCId)
                    {
                        if (!String.IsNullOrEmpty(CCEmail))
                        {
                            mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                        }
                    }
                }


                ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                mail.AlternateViews.Add(alternate);

                client.Send(mail);
                return 1;
            }
            catch (SmtpException ex)
            {
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static void SaveError(Exception ex, string volUniqueID)
        {
            try
            {
                ErrorLog objErrorLog = new ErrorLog();

                objErrorLog.Message = ex.Message;
                objErrorLog.StackTrace = "Method - " + ex.TargetSite.Name + " -> " + ex.StackTrace;
                objErrorLog.volUniqueID = volUniqueID;
                objErrorLog.InsertError();
            }
            catch
            {
            }
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
    public class GetUsersByGroupWise
    {
        public string coUniqueID { get; set; }
        public string classUniqueID { get; set; }
        public string classValue { get; set; }
        public int pageNumber { get; set; }
    }


    public class SubmitCorrection
    {
        public string volUniqueID { get; set; }
        public string newName { get; set; }
        public string newSurName { get; set; }
        public string newEmail { get; set; }

    }

}