using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using AtWork_API.Helpers;
using AtWork_API.Models;
using AtWork_API.Token;
using AtWork_API.ViewModels;

namespace AtWork_API.Controllers
{
    public class LoginController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("api/login")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] LoginViewModel login)
        {
            var c = 0;
            string token = string.Empty;
            string username = string.Empty;
            string password = string.Empty;
            try
            {
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error message from Header Auth decode - Inner Exception: {ex.InnerException.ToString()} Exception Message: {ex.Message} Source: {ex.Source}");
            }


            try
            {
                c = db.tbl_Volunteers.Count(u => u.volUserName == username && u.VolUserPassword == password);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, $"Error message from database operation - Inner Exception: {ex.InnerException.ToString()} Exception Message: {ex.Message} Source: {ex.Source}");
            }

            if (c == 1)
            {
                //FormsAuthentication.SetAuthCookie(login.useremail, false);
                var v = db.Volunteers.FirstOrDefault(u => u.volUserName == username && u.volUserPassword == password);
                c = db.MobilePreferences.Count(x => x.UserName == username);
                if (c == 0 && v!=null)
                {
                    //need to create a preferences row if does not exist.
                    MobilePreference mobilePreference = new MobilePreference();
                    mobilePreference.ID = System.Guid.NewGuid().ToString();
                    mobilePreference.UserId = v.volUniqueID;
                    mobilePreference.UserEmail = v.volEmail;
                    mobilePreference.UserName = v.volUserName;
                    mobilePreference.SessionToken = token;
                    mobilePreference.LastSessionDate = DateTime.Now.ToString();
                    //mobilePreference.ProfileImage = null;
                    db.MobilePreferences.Add(mobilePreference);
                    db.SaveChanges();
                    login.Token = "na";
                    login.SetupComplete = v.volOnBoardStatus;
                    login.UserEmail = v.volEmail;
                    login.Password = password;
                    login.CompanyId = v.coUniqueID;
                    return Content(HttpStatusCode.OK, login);
                }
                else if (c == 1 && v!=null)
                {
                    var m = db.MobilePreferences.FirstOrDefault(x => x.UserName == username);
                    login.Token = "na";
                    login.SetupComplete = string.Empty;
                    login.UserEmail = v.volEmail;
                    login.Password = password;
                    login.CompanyId = v.coUniqueID;
                    return Content(HttpStatusCode.OK, login);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Duplicate profile records.");
                }

            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "There is no matching profile record.");
            }
        }

        [Route("api/login/validate")]
        [HttpPost]
        public IHttpActionResult Validate([FromBody] LoginViewModel login)
        {
            if (login != null)
            {
                var c = 0;
                string token = string.Empty;
                string email = login.UserEmail;

                Jwt j = new Jwt();
                try
                {
                    token = j.CreateToken(email);
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error message from create token - Inner Exception: {ex.InnerException} Exception Message: {ex.Message} Source: {ex.Source}");
                }

                try
                {
                    c = db.Volunteers.Count(u => u.volEmail == email);

                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.BadRequest, $"Error message from database operation - Inner Exception: {ex.InnerException.ToString()} Exception Message: {ex.Message} Source: {ex.Source}");
                }

                if (c == 1)
                {
                    login.Token = token;
                    return Content(HttpStatusCode.OK, login);

                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "There is no matching profile record.");
                }
            }
            else {
                return Content(HttpStatusCode.BadRequest, "There is no matching profile record.");
            }
            

        }
    }
}
