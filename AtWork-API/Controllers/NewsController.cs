using AtWork_API.Filters;
using AtWork_API.Helpers;
using AtWork_API.Models;
using AtWork_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AtWork_API.Controllers
{
    [RoutePrefix("api/news")]
    public class NewsController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getlist/{ComUniqueID}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetNewsList(string ComUniqueID)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                NewsList objNews = new NewsList();
                //int CommentCount = 0;int LikeCount = 0;
                string[] imag=null;
                var list = db.tbl_News.Where(x => x.coUniqueID == ComUniqueID);
                list = list.OrderBy(x => x.newsDateTime >= DateTime.Today);

                if (list == null)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.NoRecordMessage;
                    objResponse.Data = null;
                    return Ok(objResponse);
                }
                else
                {
                    //foreach (var item in list)
                    //{
                    //    if (item.coUniqueID != null)
                    //    {
                    //        var data = db.tbl_News_Comments.Where(a => a.coUniqueID == item.coUniqueID);
                    //        CommentCount = data.Count();
                    //        foreach (var i in data)
                    //        {
                    //            LikeCount = db.tbl_News_Comments_Likes.Count(a => a.newsCommentId == i.Id);
                    //        }
                    //    }
                    //}

                    

                }

                //var xyz = from t in db.tbl_News
                //          join c in db.tbl_News_Comments
                //          on t.newsUniqueID equals c.newsUniqueID
                //          join s in db.tbl_News_Comments_Likes
                //          on c.Id equals s.newsCommentId
                //          join v in db.tbl_Volunteers
                //          on t.volUniqueID equals v.volUniqueID
                //          select new NewsList()
                //          {
                //              CommentsCount = c,
                //              LikeCount=s,
                //              news=t,
                //              Volunteers=v,
                //          };


                ////objNews.CommentsCount = ;

                //var i = xyz.Count(a=>a.CommentsCount.Id!=null);
                //var j = xyz.Count(a => a.LikeCount.Id != null);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = list;
                //objResponse.Data1 = objNews;

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

        [Route("getrow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetRow(int id)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                tbl_News obj = db.tbl_News.FirstOrDefault(x => x.id == id);
                
                int data = db.tbl_News_Comments_Likes.Count(a => a.newsCommentId == obj.id);
                var user = db.tbl_Volunteers.FirstOrDefault(a => a.volUniqueID == obj.volUniqueID);
               
                NewsCommets objComments = new NewsCommets();

                objComments.News = obj;
                objComments.Comments_Likes = data;
                objComments.Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(obj.newsDateTime));
                objComments.Volunteers = user;


                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = objComments;
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

        [Route("addrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult AddRow([FromBody] tbl_News item)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                db.tbl_News.Add(item);
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

        [Route("editrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult EditRow([FromBody] tbl_News item)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                tbl_News newsItem = db.tbl_News.FirstOrDefault(x => x.newsUniqueID == item.newsUniqueID);
                if (newsItem != null)
                {
                    newsItem.coUniqueID = item.coUniqueID;
                    newsItem.newsUniqueID = item.newsUniqueID;
                    newsItem.volUniqueID = item.volUniqueID;
                    newsItem.newsTitle = item.newsTitle;
                    newsItem.newsContent = item.newsContent;
                    newsItem.newsDateTime = item.newsDateTime;
                    newsItem.newsPostedTime = item.newsPostedTime;
                    newsItem.newsPrivacy = item.newsPrivacy;
                    newsItem.newsStatus = item.newsStatus;
                    newsItem.newsOrigin = item.newsOrigin;
                    newsItem.newsImage = item.newsImage;
                    newsItem.newsFile = item.newsFile;
                    newsItem.newsFileOriginal = item.newsFileOriginal;
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

        [Route("deleterow/{id}")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DeleteRow(int id)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                var item = db.tbl_News.FirstOrDefault(x => x.id == id);
                db.tbl_News.Remove(item);
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

        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Documents/NewsImage/");
            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            List<string> files = new List<string>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(Path.GetFileName(file.LocalFileName));
                }

                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
