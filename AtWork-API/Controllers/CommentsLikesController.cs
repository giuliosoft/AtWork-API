using AtWork_API.Filters;
using AtWork_API.Models;
using AtWork_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AtWork_API.Controllers
{
    [RoutePrefix("api/commentslikes")]
    public class CommentsLikesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getcommentlist/{newsUniqueID}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetCommentList(string newsUniqueID)
        {
            CommonResponse objResponse = new CommonResponse();
            List<NewsCommets> lst = new List<NewsCommets>();
            NewsCommets obj = null;
            try
            {

                var list = from d in db.tbl_News_Comments
                           where d.newsUniqueID == newsUniqueID
                           select new { d.Id, d.newsUniqueID, d.coUniqueID, d.comDate, d.comByID, d.comContent };
                list = list.OrderBy(ord => ord.comDate);

                foreach (var item in list)
                {
                    obj = new NewsCommets();
                    obj.Id = item.Id;
                    obj.comByID = item.comByID;
                    obj.comContent = item.comContent;
                    obj.comDate = item.comDate;
                    obj.coUniqueID = item.coUniqueID;
                    obj.Volunteers = db.tbl_Volunteers.FirstOrDefault(a => a.volUniqueID == item.comByID);
                    obj.LikeCount = db.tbl_News_Comments_Likes.Count(a => a.newsCommentId == item.Id);
                    lst.Add(obj);
                }
                //var list = from d in db.tbl_News_Comments
                //           join c in db.tbl_News_Comments_Likes
                //           on d.Id equals c.newsCommentId
                //           join v in db.tbl_Volunteers
                //           on d.comByID equals v.volUniqueID
                //           select new {
                //               d.newsUniqueID
                //           };
                //var a = list.Where(s=>s.newsUniqueID== newsUniqueID).ToList();
                //list = list.OrderBy(ord => ord.comDate);



                //if (list == null)
                //{
                //    objResponse.Flag = true;
                //    objResponse.Message = Message.NoRecordMessage;
                //    objResponse.Data = null;
                //    return Ok(objResponse);
                //}
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lst;
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
        [Route("addComment")]
        [HttpPost]
        public IHttpActionResult InsertCommentRow([FromBody] tbl_News_Comments objComment)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                db.tbl_News_Comments.Add(objComment);
                int i = db.SaveChanges();
                if (i > 0)
                {
                   
                    objResponse.Flag = true;
                    objResponse.Message = Message.InsertSuccessMessage;
                    objResponse.Data = null;

                    return Ok(objResponse);
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;

                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
        }

        [Route("EditComment")]
        [HttpPost]
        public IHttpActionResult EditCommentRow([FromBody] tbl_News_Comments objComment)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                tbl_News_Comments newsItem = db.tbl_News_Comments.FirstOrDefault(x => x.Id == objComment.Id);
                if (newsItem != null)
                {
                    newsItem.coUniqueID = objComment.coUniqueID;
                    newsItem.newsUniqueID = objComment.newsUniqueID;
                    newsItem.comContent = objComment.comContent;
                    newsItem.comDate = objComment.comDate;
                    newsItem.comByID = objComment.comByID;
                }
                int i = db.SaveChanges();
                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.UpdateSuccessMessage;
                    objResponse.Data = null;

                    return Ok(objResponse);
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;

                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
        }

        [Route("deleteComment/{Id}")]
        [HttpPost]
        public IHttpActionResult DeleteCommentRow(int Id)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                var item = db.tbl_News_Comments.FirstOrDefault(x => x.Id == Id);
                db.tbl_News_Comments.Remove(item);
                int i = db.SaveChanges();

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.DeleteSuccessMessage;
                    objResponse.Data = null;

                    return Ok(objResponse);
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;

                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
        }
    }
}
