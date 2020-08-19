using AtworkAPI.Token;
using AtworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AtworkAPI.Helpers;
using System.Web.Http.Cors;
using System.Text;
using System.Net;

namespace AtworkAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET, POST")]
    public class ProfilesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("api/profiles/getlist")]
        [HttpGet]
        public IHttpActionResult GetList()
        {
            
            string email = String.Empty;
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
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        List<Volunteer> vol = db.Volunteers.ToList();
                        return Ok(vol);
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

        [Route("api/profiles/getrow")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetRow(string id)
        {
            var c = 0;
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

                
                else
                {
                    return Content(System.Net.HttpStatusCode.BadRequest, "Bad credentials");
                }

                try
                {
                    c = db.Volunteers.Count(u => u.volUserName == username);

                    if (c != 1)
                    {
                        return Unauthorized();
                    }
                    else
                    {
                        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volUserName == username);                    
                        return Ok(vol);
                    }

                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.BadRequest, $"Error message from database operation - Inner Exception: {ex.InnerException.ToString()} Exception Message: {ex.Message} Source: {ex.Source}");
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("api/profiles/updaterow")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult UpdateRow([FromBody] Volunteer volunteer)
        {
            //try
            //{
            //    string header = HttpHelper.GetHeader(Request, "Authorization");
            //    string token = header.Substring("Bearer ".Length).Trim();
            //    Jwt j = new Jwt();
            //    bool b = j.ValidateToken(token);
            //    if (b == true)
            //    {
            //        //logic
            //        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volUniqueID == volunteer.volUniqueID);
            //        vol.volFirstName = volunteer.volFirstName;
            //        vol.volLastName = volunteer.volLastName;
            //        vol.volGender = volunteer.volGender;
            //        vol.volUserName = volunteer.volUserName;
            //        vol.volUserPassword = volunteer.volUserPassword;
            //        vol.volEmail = volunteer.volEmail;
            //        vol.volOnBoardStatus = volunteer.volOnBoardStatus;
            //        vol.volOnBoardDateSent = volunteer.volOnBoardDateSent;
            //        vol.volPicture = volunteer.volPicture;
            //        vol.volEducation = volunteer.volEducation;
            //        vol.volCompetencies = volunteer.volCompetencies;
            //        vol.volCategories = volunteer.volCategories;
            //        vol.volPhone = volunteer.volPhone;
            //        vol.volStatus = volunteer.volStatus;
            //        vol.restricted = volunteer.restricted;
            //        vol.volDepartment = volunteer.volDepartment;
            //        vol.volLanguage = volunteer.volLanguage;
            //        vol.volAbout = volunteer.volAbout;
            //        vol.volInterests = volunteer.volInterests;
            //        db.SaveChanges();
            //        return Ok();
            //    }
            //    else
            //    {
            //        return Unauthorized();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest();
            //}

            string email = String.Empty;
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
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volEmail == volunteer.volEmail);
                        vol.volFirstName = volunteer.volFirstName;
                        vol.volLastName = volunteer.volLastName;
                        vol.volGender = volunteer.volGender;
                        vol.volUserName = volunteer.volUserName;
                        vol.volUserPassword = volunteer.volUserPassword;
                        vol.volEmail = volunteer.volEmail;
                        vol.volOnBoardStatus = volunteer.volOnBoardStatus;
                        vol.volOnBoardDateSent = volunteer.volOnBoardDateSent;
                        vol.volPicture = volunteer.volPicture;
                        vol.volEducation = volunteer.volEducation;
                        vol.volCompetencies = volunteer.volCompetencies;
                        vol.volCategories = volunteer.volCategories;
                        vol.volPhone = volunteer.volPhone;
                        vol.volStatus = volunteer.volStatus;
                        vol.restricted = volunteer.restricted;
                        vol.volDepartment = volunteer.volDepartment;
                        vol.volLanguage = volunteer.volLanguage;
                        vol.volAbout = volunteer.volAbout;
                        vol.volInterests = volunteer.volInterests;
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

        //sets language in volunteer file and mobile preferences
        [Route("api/profiles/updatelanguage")]
        [HttpPost]
        public IHttpActionResult UpdateLanguage([FromBody] Volunteer volunteer)
        {
            try
            {
                
                string email = String.Empty;
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
                        email = usernamePassword.Substring(0, separatorIndex);
                        password = usernamePassword.Substring(separatorIndex + 1);

                        var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                        if (c == 1)
                        {
                            var vol = db.Volunteers.FirstOrDefault(x => x.volEmail == volunteer.volEmail);
                            vol.volLanguage = volunteer.volLanguage;
                            var pref = db.MobilePreferences.FirstOrDefault(x => x.UserEmail == volunteer.volEmail);
                            pref.Language = volunteer.volLanguage;
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

        [Route("api/profiles/updatepassword")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult UpdatePassword([FromBody] Volunteer volunteer)
        {
            string email = string.Empty; // login.UserEmail;
            string password = string.Empty; // login.Password;
            try
            {
                //string header = HttpHelper.GetHeader(Request, "Authorization");
                //string token = header.Substring("Bearer ".Length).Trim();
                //Jwt j = new Jwt();
                //bool b = j.ValidateToken(token);
                //if (b == true)
                //{
                //    //logic
                //    Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volEmail == volunteer.volEmail);
                //    vol.volUserPassword = volunteer.volUserPassword;
                //    db.SaveChanges();
                //    return Ok();
                //}
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volEmail == volunteer.volEmail);
                        vol.volUserPassword = volunteer.volUserPassword;
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


        [Route("api/profiles/updatepicture")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult UpdatePicture([FromBody] Volunteer volunteer)
        {
            //try
            //{
            //    string header = HttpHelper.GetHeader(Request, "Authorization");
            //    string token = header.Substring("Bearer ".Length).Trim();
            //    Jwt j = new Jwt();
            //    bool b = j.ValidateToken(token);
            //    if (b == true)
            //    {
            //        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volUniqueID == volunteer.volUniqueID);
            //        vol.volPicture = volunteer.volPicture;
            //        db.SaveChanges();
            //        return Ok();
            //    }
            //    else
            //    {
            //        return Unauthorized();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest();
            //}

            string email = String.Empty;
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
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volUniqueID == volunteer.volUniqueID);
                        vol.volPicture = volunteer.volPicture;
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

        [Route("api/profiles/updateinterests")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult UpdateInterests([FromBody] Volunteer volunteer)
        {
            //try
            //{
            //    string header = HttpHelper.GetHeader(Request, "Authorization");
            //    string token = header.Substring("Bearer ".Length).Trim();
            //    Jwt j = new Jwt();
            //    bool b = j.ValidateToken(token);
            //    if (b == true)
            //    {
            //        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volUniqueID == volunteer.volUniqueID);
            //        vol.volInterests = volunteer.volInterests;
            //        db.SaveChanges();
            //        return Ok();
            //    }
            //    else
            //    {
            //        return Unauthorized();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest();
            //}

            string email = String.Empty;
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
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        Volunteer vol = db.Volunteers.FirstOrDefault(x => x.volUniqueID == volunteer.volUniqueID);
                        vol.volInterests = volunteer.volInterests;
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

        [Route("api/profiles/insertinterests")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult InsertInterests([FromBody] List<VolunteerInterest> list)
        {
            //try
            //{
            //    string header = HttpHelper.GetHeader(Request, "Authorization");
            //    string token = header.Substring("Bearer ".Length).Trim();
            //    Jwt j = new Jwt();
            //    bool b = j.ValidateToken(token);
            //    if (b == true)
            //    {
            //        db.VolunteerInterests.AddRange(list);
            //        db.SaveChanges();
            //        return Ok();
            //    }
            //    else
            //    {
            //        return Unauthorized();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest();
            //}

            string email = String.Empty;
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
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        db.VolunteerInterests.AddRange(list);
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

        [Route("api/profiles/aboutme")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult UpdateAboutMe([FromBody] Volunteer vol)
        {
            string email = String.Empty;
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
                    email = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volEmail == email && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        var v = db.Volunteers.FirstOrDefault(x => x.volEmail == vol.volEmail);
                        v.volAbout = vol.volAbout;
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