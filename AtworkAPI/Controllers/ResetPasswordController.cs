using AtworkAPI.Helpers;
using AtworkAPI.Token;
using AtworkAPI.ViewModels;
using AtworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

namespace AtworkAPI.Controllers
{
    [RoutePrefix("api/reset")]
    public class ResetPasswordController : ApiController
    {
        ModelContext db = new ModelContext();

        [Route("new")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Post([FromBody] LoginViewModel login)
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
                        Volunteer v = db.Volunteers.FirstOrDefault(u => u.volUserName == login.UserName && u.volUserPassword == login.Password);
                        v.volUserPassword = login.NewPassword;
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
