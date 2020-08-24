using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;


namespace AtWork_API.Models
{
    public class AuthenticateModel : AuthorizeAttribute //System.Web.Mvc.IAuthorizationFilter
    {
        //private ModelContext db = new ModelContext();
        //public void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    if (actionContext.Request.Headers.Authorization != null)
        //    {
        //        // get the Authorization header value from the request and base64 decode it
        //        //string userInfo = Encoding.Default.GetString(Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter));/

        //        string header = actionContext.Request.Headers.Authorization.Parameter;
        //        string encodedHeader = header.Substring("Basic ".Length).Trim();
        //        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        //        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));
        //        // custom authentication logic
        //        //if (string.Equals(header, string.Format("{0}:{1}", "Parry", "123456")))
        //        //{
        //        //    //IsAuthorized(actionContext);
        //        //}
        //        //else
        //        //{
        //        //    HandleUnauthorizedRequest(actionContext);
        //        //}
        //    }
        //    else
        //    {
        //        HandleUnauthorizedRequest(actionContext);
        //    }
        //}
        //protected void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        ReasonPhrase = "Unauthorized"
        //    };
        //}
        //public void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //string header = filterContext.GetHeader(Request, "Authorization");
        //    string username = string.Empty;
        //    string password = string.Empty;

        //    var header = filterContext.HttpContext.Request.Headers["Authorization"];

        //    if (header != null && header.StartsWith("Basic"))
        //    {
        //        string encodedHeader = header.Substring("Basic ".Length).Trim();
        //        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        //        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

        //        int separatorIndex = usernamePassword.IndexOf(":");
        //        username = usernamePassword.Substring(0, separatorIndex);
        //        password = usernamePassword.Substring(separatorIndex + 1);
        //    }
        //    int c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
        //    if (c == 1)
        //    {
        //        return;
        //    }
        //}
    }
}