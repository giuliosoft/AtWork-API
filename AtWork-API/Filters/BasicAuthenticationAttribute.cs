using AtWork_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AtWork_API.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private ModelContext db = new ModelContext();

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (actionContext.Request.Headers.Authorization == null)
            {
                var host = actionContext.Request.RequestUri.DnsSafeHost;
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                //actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
                actionContext.Response.Content = new StringContent("Invalid request.Authorization token required", Encoding.UTF8, "text/html");
                return;
            }
            else
            {
                try
                {
                    string header = Convert.ToString(actionContext.Request.Headers.Authorization);
                    if (header != null && header.StartsWith("Basic"))
                    {
                        string username = string.Empty;
                        string password = string.Empty;

                        string encodedHeader = header.Substring("Basic ".Length).Trim();
                        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                        int separatorIndex = usernamePassword.IndexOf(":");
                        username = usernamePassword.Substring(0, separatorIndex);
                        password = usernamePassword.Substring(separatorIndex + 1);

                        int c = db.tbl_Volunteers.Count(u => u.volUserName == username && u.VolUserPassword == password);
                        if (c == 0)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                            actionContext.Response.Content = new StringContent("Invalid authorization token!", Encoding.UTF8, "text/html");
                        }

                    }
                }
                catch (Exception ex)
                {
                    var host = actionContext.Request.RequestUri.DnsSafeHost;
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    //actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
                    actionContext.Response.Content = new StringContent("Invalid authorization token!", Encoding.UTF8, "text/html");
                    return;
                }
            }
            base.OnAuthorization(actionContext);
        }
    }
}
