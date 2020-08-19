using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using AtworkAPI.Helpers;
using AtworkAPI.Models;
using AtworkAPI.Token;

namespace AtworkAPI.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MobilePreferenceController : ApiController
    {
        ModelContext db = new ModelContext();

        // GET: api/MobilePreference/5
        [HttpGet]
        [Route("get/{id}")]
        public string Get(string id)
        {
            return "value";
        }

        
        [Route("api/mobilepreference/updatelanguage")]
        [HttpPost]
        public IHttpActionResult UpdateLanguage([FromBody] MobilePreference m)
        {
            try
            {
                string username = String.Empty;
                string password = String.Empty;
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
                        
                        //validate
                        var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                        if (c == 1)
                        {
                            //logic
                            var pref = db.MobilePreferences.FirstOrDefault(x => x.UserName == username);
                            pref.ProfileImage = m.ProfileImage;
                            db.SaveChanges();
                            return Ok();
                        }
                        else
                        {
                            return Unauthorized();
                        }

                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error message from Header Auth decode - Inner Exception: {ex.InnerException.ToString()} Exception Message: {ex.Message} Source: {ex.Source}");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [Route("api/mobilepreference/updateimage")]
        [HttpPost]
        public IHttpActionResult UpdateImage([FromBody] MobilePreference m)
        {
            
            string username = String.Empty;
            string password = String.Empty;
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

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        var pref = db.MobilePreferences.FirstOrDefault(x => x.UserName == username);
                        pref.ProfileImage = m.ProfileImage;
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("api/mobilepreference/updateinterest")]
        [HttpPost]
        public IHttpActionResult UpdateInterest([FromBody] MobilePreference m)
        {
            
            string username = String.Empty;
            string password = String.Empty;
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

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        var pref = db.MobilePreferences.FirstOrDefault(x => x.UserName == username);
                        pref.Interests = m.Interests;
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
