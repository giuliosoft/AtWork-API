using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AtworkAPI.Token;
using AtworkAPI.ViewModels;
using AtworkAPI.Models;
using AtworkAPI.Helpers;
using System.Web.Http.Cors;
using System.Text;

namespace AtworkAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CommonController : ApiController
    {
        private ModelContext db = new ModelContext();

        
        [Route("api/common/getkeywords")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetKeywords()
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
                        List<Keyword> keywords = db.Keywords.ToList<Keyword>();
                        return Ok(keywords);
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


        [Route("api/common/getinterestlist")]
        [HttpGet]
        public IHttpActionResult GetInterestList()
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
                        List<MobileInterest> interests = db.MobileInterests.ToList();
                        return Ok(interests);
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