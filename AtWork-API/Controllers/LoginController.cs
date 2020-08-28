using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using AtWork_API.Filters;
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
        //[BasicAuthentication]
        public IHttpActionResult Post()
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                string username = string.Empty;
                string password = string.Empty;
                string token = string.Empty;

                var re = Request;
                var headers = re.Headers;

                if (headers.Contains("Authorization"))
                {
                    token = headers.GetValues("Authorization").First();

                    string encodedHeader = token.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    var Volunteers = db.tbl_Volunteers.FirstOrDefault(u => u.volUserName == username && u.VolUserPassword == password);
                    if (Volunteers != null)
                    {
                       
                         var CompanyInfo = db.tbl_Companies.FirstOrDefault(a => a.coUniqueID == Volunteers.coUniqueID);
                        if (CompanyInfo != null)
                        {
                            //ComamiesModel obj = new ComamiesModel();

                            //obj.Volunteers = Volunteers;
                            //obj.Companies = CompanyInfo;

                            objResponse.Flag = true;
                            objResponse.Message = Message.GetData;
                            objResponse.Data = CompanyInfo;
                            objResponse.Data1 = Volunteers;
                            return Ok(objResponse);
                        }
                    }
                    else
                    {
                        objResponse.Flag = true;
                        objResponse.Message = Message.NoRecordMessage;
                        objResponse.Data = null;
                        return Ok(objResponse);
                    }
                }

                objResponse.Flag = true;
                objResponse.Message = "Invalid parameter";
                objResponse.Data = null;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
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
            else
            {
                return Content(HttpStatusCode.BadRequest, "There is no matching profile record.");
            }


        }

        
        

    }
}
