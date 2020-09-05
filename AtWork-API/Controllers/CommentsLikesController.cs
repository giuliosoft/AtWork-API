using API_Placement_record_management.Models;
using AtWork_API.Filters;
using AtWork_API.Models;
using AtWork_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


namespace AtWork_API.Controllers
{
    [RoutePrefix("api/commentslikes")]
    public class CommentsLikesController : ApiController
    {
        private ModelContext db = new ModelContext();

        //[Route("getcommentlist/{newsUniqueID}/{volUniqueID}")]
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

                    //var dt = db.tbl_News_Comments_Likes.Where(a => a.newsCommentId == item.Id).ToList();
                    //if (dt.Count > 0)
                    //{
                    //    var dd = db.tbl_News_Comments_Likes.Where(a => a.likeByID == volUniqueID).ToList();
                    //    if (dd.Count > 0)
                    //    {
                    //        int id = dd.FirstOrDefault().Id;
                    //    }
                    //}
                    //var Like = db.tbl_News_Comments_Likes.Where(a => a.newsCommentId == item.Id).ToList();

                    //if (Like.Count > 0)
                    //{
                    //    obj.LikeCount = Like.Count();
                    //    obj.LikeId = db.tbl_News_Comments_Likes.Where(a => a.likeByID == volUniqueID).FirstOrDefault().Id;
                    //}
                    //obj.LikeId = db.tbl_News_Comments_Likes.Where(a => a.newsCommentId == obj.Id).SingleOrDefault().Id;
                    //if (item.comByID == volUniqueID)
                    //{
                    //    obj.LikeByLoginUser = db.tbl_News_Comments_Likes.Any(a => a.newsCommentId == obj.Id);
                    //}
                    lst.Add(obj);
                }

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
        [BasicAuthentication]
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
        [BasicAuthentication]
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
        [BasicAuthentication]
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

        [Route("AddNewsCommentLike")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult insertNewsCommentLike([FromBody] News_Comments_Likes ObjnewsCommentLike)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_InsertNews_Comments_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@newsCommentId", ObjnewsCommentLike.newsCommentId);
                sqlCmd.Parameters.AddWithValue("@likeByID", ObjnewsCommentLike.likeByID);
                sqlCmd.Parameters.AddWithValue("@likeDate", ObjnewsCommentLike.likeDate);

                sqlCmd.Parameters.Add("@CountData", SqlDbType.Int).Direction = ParameterDirection.Output;

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                int CountData = (int)sqlCmd.Parameters["@CountData"].Value;

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.InsertSuccessMessage;
                    objResponse.Data = CountData;
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }
        [Route("DeleteNewsCommentLike")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DeletetNewsCommentLike([FromBody] News_Comments_Likes ObjnewsCommentLike)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_delete_News_Comments_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@Id", ObjnewsCommentLike.Id);
                sqlCmd.Parameters.AddWithValue("@newsCommentId", ObjnewsCommentLike.newsCommentId);

                sqlCmd.Parameters.Add("@CountData", SqlDbType.Int).Direction = ParameterDirection.Output;

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                int CountData = (int)sqlCmd.Parameters["@CountData"].Value;

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.DeleteSuccessMessage;
                    objResponse.Data = CountData;
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }
        [Route("AddNewsLike")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult insertNewsLike([FromBody] News_Likes ObjnewsLike)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_InsertNews_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@newsId", ObjnewsLike.newsId);
                sqlCmd.Parameters.AddWithValue("@likeByID", ObjnewsLike.likeByID);
                sqlCmd.Parameters.AddWithValue("@likeDate", ObjnewsLike.likeDate);

                sqlCmd.Parameters.Add("@CountData", SqlDbType.Int).Direction = ParameterDirection.Output;

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                int CountData = (int)sqlCmd.Parameters["@CountData"].Value;

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.InsertSuccessMessage;
                    objResponse.Data = CountData;
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("DeleteNewsLike")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DelettNewsLike([FromBody] News_Likes ObjnewsLike)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_delete_News_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@Id", ObjnewsLike.Id);
                sqlCmd.Parameters.AddWithValue("@newsId", ObjnewsLike.newsId);

                sqlCmd.Parameters.Add("@CountData", SqlDbType.Int).Direction = ParameterDirection.Output;

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                int CountData = (int)sqlCmd.Parameters["@CountData"].Value;

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.DeleteSuccessMessage;
                    objResponse.Data = CountData;
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.ErrorMessage;
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }
    }
}
