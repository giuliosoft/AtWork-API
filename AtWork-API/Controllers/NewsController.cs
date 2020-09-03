using AtWork_API.Filters;
using AtWork_API.Helpers;
using AtWork_API.Models;
using AtWork_API.ViewModels;
using Newtonsoft.Json;
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
                //List<string> imag = null;
                //var list = db.tbl_News.Where(x => x.coUniqueID == ComUniqueID);
                //list = list.OrderBy(x => x.newsDateTime >= DateTime.Today);

                //if (list == null)
                //{
                //    objResponse.Flag = true;
                //    objResponse.Message = Message.NoRecordMessage;
                //    objResponse.Data = null;
                //    return Ok(objResponse);
                //}



                var model = (from t in db.tbl_News
                             join c in db.tbl_News_Comments
                             on t.newsUniqueID equals c.newsUniqueID
                             join s in db.tbl_News_Comments_Likes
                             on c.Id equals s.newsCommentId
                             join v in db.tbl_Volunteers
                             on t.volUniqueID equals v.volUniqueID
                             select new NewsList()
                             {
                                 news = t,
                                 Volunteers = v,
                                 CommentsCount = (from q in db.tbl_News_Comments
                                                  where q.newsUniqueID.Equals(t.newsUniqueID)
                                                  select q).Count(),
                                 LikeCount = (from a in db.tbl_News_Comments_Likes
                                              where a.Id.Equals(s.newsCommentId)
                                              select a).Count(),
                                 //Image = t.newsImage.Where(x => x..Any(tag => list.Contains(tag));,
                                 //Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(t.newsDateTime)).ToString()
                             });
                // }).AsEnumerable()
                //.Select(x => new { T = x.news.newsImage.Join(",", new List<string>(imag).ToArray()) });


                //var model = (from a in db.tbl_News
                //                    join pg in db.tbl_News_Comments on a.newsUniqueID equals pg.newsUniqueID into pgs
                //                    from m in pgs.DefaultIfEmpty()
                //                    join pr in db.tbl_Volunteers on m.comByID equals pr.volUniqueID into prs
                //                    from p in prs.DefaultIfEmpty()
                //                    join ds in db.tbl_News_Comments_Likes on m.Id equals ds.newsCommentId into docs
                //                    from docst in docs.DefaultIfEmpty()
                //                        //where a.FullPath.Contains(txtSearchText.Text)
                //                    select new NewsList()
                //                    {
                //                        news = a,
                //                        Volunteers = p,
                //                        CommentsCount = (from q in db.tbl_News_Comments
                //                                         where q.newsUniqueID.Equals(a.newsUniqueID)
                //                                         select q).Count(),
                //                        LikeCount = (from a in db.tbl_News_Comments_Likes
                //                                     where a.Id.Equals(docst.newsCommentId)
                //                                     select a).Count(),

                //                    }).OrderByDescending(a => a.news.id).ToList();

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = model;
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
        public IHttpActionResult AddRow()
        {
            CommonResponse response = new CommonResponse();
            string imagesPath = "~/newspots/";
            string filesPath = "~/newspostsfiles/";

            try
            {
                var httpRequest = HttpContext.Current.Request;
                tbl_News item = JsonConvert.DeserializeObject<tbl_News>(httpRequest.Params["Data"].ToString());
                string volUniqueID = item.volUniqueID.Substring(item.volUniqueID.Length - 3);

                int counter = 1;
                var newId = "newscorp" + DateTime.UtcNow.Ticks + volUniqueID;
                while (db.tbl_News.Any(a => a.newsUniqueID == newId))
                {
                    newId = "newscorp" + counter + DateTime.UtcNow.Ticks + volUniqueID;
                    counter++;
                }

                foreach (string file in httpRequest.Files)
                {
                    var filePath = string.Empty;
                    var postedFile = httpRequest.Files[file];
                    string extension = Path.GetExtension(postedFile.FileName);
                    if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                    {
                        if (string.IsNullOrEmpty(item.newsImage))
                        {
                            item.newsImage = postedFile.FileName;
                        }
                        else
                        {
                            item.newsImage += string.Join(",", postedFile.FileName);
                        }

                        filePath = HttpContext.Current.Server.MapPath(imagesPath + postedFile.FileName);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.newsFile))
                        {
                            item.newsFile = postedFile.FileName;
                        }
                        else
                        {
                            item.newsFile += string.Join(",", postedFile.FileName);
                        }

                        filePath = HttpContext.Current.Server.MapPath(filesPath + postedFile.FileName);
                    }
                    postedFile.SaveAs(filePath);
                }

                item.newsUniqueID = newId;
                db.tbl_News.Add(item);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    response.Flag = true;
                    response.Message = Message.InsertSuccessMessage;
                    response.Data = null;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Flag = false;
                response.Message = Message.ErrorMessage;
                response.Data = null;
                return Ok(response);
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

                var httpRequest = HttpContext.Current.Request;
                var AttachedFiles = httpRequest.Files;
                string fileName = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string extension = System.IO.Path.GetExtension(postedFile.FileName);
                    if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                    {
                        if (item.newsImage != null && item.newsImage != "")
                        {
                            item.newsImage += string.Join(",", postedFile.FileName);
                        }
                        else
                        {
                            item.newsImage = postedFile.FileName;
                        }
                        var filePath = HttpContext.Current.Server.MapPath("~/images/News/Images/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                    }
                    else
                    {
                        if (item.newsFile != null && item.newsFile != "")
                        {
                            item.newsFile += string.Join(",", postedFile.FileName);
                        }
                        else
                        {
                            item.newsFile = postedFile.FileName;
                        }
                        item.newsFile = string.Join(",", postedFile.FileName);
                        var filePath = HttpContext.Current.Server.MapPath("~/images/News/Files/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                    }

                }
                int i = db.SaveChanges();
                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.UpdateSuccessMessage;
                    objResponse.Data = null;
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
    }
}
