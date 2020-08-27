using AtWork_API.Filters;
using AtWork_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtWork_API.Controllers
{
    [RoutePrefix("api/commentslikes")]
    public class CommentsLikesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getcommentlist/{ComUniqueID}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetCommentList(string ComUniqueID)
        {
            try
            {
                var list = from d in db.tbl_News_Comments.Where(a => a.coUniqueID == ComUniqueID)
                           select new { d.Id ,d.newsUniqueID, d.coUniqueID, d.comDate, d.comByID, d.comContent };
                list = list.OrderBy(ord => ord.comDate);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }
        [Route("addComment")]
        [HttpPost]
        public IHttpActionResult InsertCommentRow([FromBody] tbl_News_Comments objComment)
        {
            try
            {
                db.tbl_News_Comments.Add(objComment);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("EditComment")]
        [HttpPost]
        public IHttpActionResult EditCommentRow([FromBody] tbl_News_Comments objComment)
        {
            try
            {
                tbl_News_Comments newsItem = db.tbl_News_Comments.FirstOrDefault(x => x.Id == objComment.Id);
                if (newsItem != null)
                {
                    newsItem.coUniqueID = objComment.coUniqueID;
                    newsItem.newsUniqueID = objComment.newsUniqueID;
                    newsItem.comContent = objComment.comContent;
                }
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("deleteComment/{newsUniqueID}")]
        [HttpPost]
        public IHttpActionResult DeleteCommentRow(string newsUniqueID)
        {
            try
            {
                var item = db.tbl_News_Comments.FirstOrDefault(x => x.newsUniqueID == newsUniqueID);
                db.tbl_News_Comments.Remove(item);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }
    }
}
