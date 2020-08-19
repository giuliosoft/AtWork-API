using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AtworkAPI.Token;
using AtworkAPI.Models;
using AtworkAPI.Helpers;
using System.Text;

namespace AtworkAPI.Controllers
{
    [RoutePrefix("api/commentslikes")]
    public class CommentsLikesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getcommentlist")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCommentList()
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
                        var list = from d in db.NewsComments
                                   select new { d.newsUniqueID, d.coUniqueID, d.comDate, d.comByID, d.comContent };
                        list = list.OrderBy(ord => ord.comDate);
                        return Ok(list);
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

        [Route("getlikelist/{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetLikeList(int id)
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
                        var list = from d in db.NewsCommentLikes
                                   select new { d.newsCommentId, d.likeByID, d.likeDate };
                        list = list.Where(x => x.newsCommentId == id);
                        list = list.OrderBy(ord => ord.likeDate);
                        return Ok(list);
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

        [Route("getcommentrow/{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCommentRow(string id)
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
                        var list = from d in db.NewsComments
                                   select new { d.newsUniqueID, d.comByID, d.comContent, d.comDate };
                        list = list.Where(x => x.newsUniqueID == id);
                        list = list.OrderBy(ord => ord.comDate);
                        return Ok(list);
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

        [Route("getlikerow/{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetLikeRow(string id)
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

        [Route("insertcommentrow")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult InsertCommentRow([FromBody] NewsComment n)
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
                        db.NewsComments.Add(n);
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

        [Route("insertlikerow")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult InsertLikeRow([FromBody] NewsCommentLike n)
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
                        db.NewsCommentLikes.Add(n);
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