using AtWork_API.Filters;
using AtWork_API.Helpers;
using AtWork_API.Models;
using AtWork_API.ViewModels;
using Microsoft.Ajax.Utilities;
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

        [Route("getlist/{ComUniqueID}/{pageNumber}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetNewsList(string ComUniqueID, int pageNumber)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                NewsList objNews = new NewsList();

                int numberOfObjectsPerPage = 5;

                var model = (from a in db.tbl_News
                             where a.coUniqueID == ComUniqueID
                             join pg in db.tbl_News_Comments on a.newsUniqueID equals pg.newsUniqueID into pgs
                             from m in pgs.DefaultIfEmpty()
                             join pr in db.tbl_Volunteers on a.volUniqueID equals pr.volUniqueID into prs
                             from p in prs.DefaultIfEmpty()
                             join ds in db.tbl_News_Comments_Likes on m.Id equals ds.newsCommentId into docs
                             from docst in docs.DefaultIfEmpty()
                             select new NewsList()
                             {
                                 news = a,
                                 Volunteers = p,
                                 CommentsCount = (from q in db.tbl_News_Comments
                                                  where q.newsUniqueID.Equals(a.newsUniqueID)
                                                  select q).Count(),
                                 LikeCount = (from a in db.tbl_News_Comments_Likes
                                              where a.Id.Equals(docst.newsCommentId)
                                              select a).Count(),
                                 Day = a.newsDateTime.ToString()

                             }).DistinctBy(s=>s.news.id).OrderByDescending(a => a.news.id).ToList().Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage).ToList();


                model.ForEach(x => x.Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(x.Day)));
               
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = model;
                objResponse.Data1 = objNews;

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
            CommonResponse objResponse = new CommonResponse();
            string fileImagePath = "images/News/Images/";
            string fileFilePath = "images/News/Files/";
            try
            {
                var httpRequest = HttpContext.Current.Request;
                tbl_News item = JsonConvert.DeserializeObject<tbl_News>(httpRequest.Params["Data"].ToString());

                var newId = "newscorp" + DateTime.UtcNow.Ticks + item.volUniqueID.Substring(item.volUniqueID.Length - 3);
                int id = db.tbl_News.Count(a => a.newsUniqueID == newId);
                if (id == 0)
                {
                    var AttachedFiles = httpRequest.Files;
                    string fileName = string.Empty;
                    int index = 0;
                    string ImageFile = string.Empty;
                    string File = string.Empty;
                    foreach (string file in httpRequest.Files)
                    {
                        index++;
                        var postedFile = httpRequest.Files[file];
                        string extension = System.IO.Path.GetExtension(postedFile.FileName);
                        if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                        {
                            if (fileName != string.Empty && fileName != "")
                            {
                                ImageFile += "," + fileImagePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension;
                            }
                            else
                            {
                                ImageFile += fileImagePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                            }
                            var filePath = HttpContext.Current.Server.MapPath("~/images/News/Images/" + fileName);
                            postedFile.SaveAs(filePath);
                        }
                        else
                        {
                            if (File != string.Empty && File != "")
                            {
                                //item.newsFile += string.Join(",", postedFile.FileName);
                                File += "," + fileFilePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                            }
                            else
                            {
                                File += fileFilePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                            }
                            var filePath = HttpContext.Current.Server.MapPath("~/images/News/Files/" + fileName);
                            postedFile.SaveAs(filePath);
                        }
                    }
                    item.newsImage = ImageFile;
                    item.newsFile = File;
                    item.newsUniqueID = newId;
                    db.tbl_News.Add(item);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.Flag = true;
                        objResponse.Message = Message.InsertSuccessMessage;
                        objResponse.Data = null;
                    }
                }
                else
                {
                    int index = 0;
                    newId = "newscorp1" + DateTime.UtcNow.Ticks + item.volUniqueID.Substring(item.volUniqueID.Length - 3);
                    //var httpRequest = HttpContext.Current.Request;
                    //var Datainput = JsonConvert.DeserializeObject<tbl_News>(httpRequest.Params["Data"].ToString());
                    var AttachedFiles = httpRequest.Files;
                    string fileName = string.Empty;
                    string ImageFile = string.Empty;
                    string File = string.Empty;

                    foreach (string file in httpRequest.Files)
                    {
                        index++;
                        var postedFile = httpRequest.Files[file];
                        string extension = System.IO.Path.GetExtension(postedFile.FileName);
                        if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                        {
                            if (ImageFile != string.Empty && ImageFile != "")
                            {
                                ImageFile += "," + fileImagePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension;
                            }
                            else
                            {
                                ImageFile += fileImagePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension;
                            }
                            var filePath = HttpContext.Current.Server.MapPath("~/images/News/Images/" + fileName);
                            postedFile.SaveAs(filePath);
                        }
                        else
                        {
                            if (item.newsFile != null && item.newsFile != "")
                            {
                                File += "," + fileFilePath + newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + "." + extension;
                            }
                            else
                            {
                                File += fileFilePath + newId.Substring(newId.Length - 5) + "_" + index + extension;
                                fileName = newId.Substring(newId.Length - 5) + "_" + index + extension;
                            }
                            var filePath = HttpContext.Current.Server.MapPath("~/images/News/Files/" + fileName);
                            postedFile.SaveAs(filePath);
                        }
                    }

                    item.newsUniqueID = newId;
                    item.newsImage = ImageFile;
                    item.newsFile = File;
                    db.tbl_News.Add(item);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        objResponse.Flag = true;
                        objResponse.Message = Message.InsertSuccessMessage;
                        objResponse.Data = null;
                    }
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

        [Route("editrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult EditRow()
        {
            CommonResponse objResponse = new CommonResponse();
            string fileImagePath = "images/News/Images/";
            string fileFilePath = "images/News/Files/";
            string ImageFile = string.Empty;
            string File = string.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                tbl_News item = JsonConvert.DeserializeObject<tbl_News>(httpRequest.Params["Data"].ToString());

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
                    //newsItem.newsImage = item.newsImage;
                    //newsItem.newsFile = item.newsFile;
                    newsItem.newsFileOriginal = item.newsFileOriginal;
                }
                string fileName = string.Empty;
                int index = 0;
                foreach (string file in httpRequest.Files)
                {
                    index++;
                    var postedFile = httpRequest.Files[file];
                    string extension = System.IO.Path.GetExtension(postedFile.FileName);
                    if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                    {
                        if (item.newsImage != null && item.newsImage != "")
                        {
                            ImageFile += "," + fileImagePath + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 6) + "_" + index + extension; ;
                            fileName = newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 6) + "_" + index + extension; ;
                        }
                        else
                        {
                            ImageFile += fileImagePath + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 6) + "_" + index + extension; ;
                            fileName = postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 6) + "_" + index + extension; ;
                        }
                        var filePath = HttpContext.Current.Server.MapPath("~/images/News/Images/" + fileName);
                        postedFile.SaveAs(filePath);
                    }
                    else
                    {
                        if (item.newsFile != null && item.newsFile != "")
                        {
                            File += fileFilePath + item.newsImage + "," + postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 4) + "_" + index + extension; ;
                            fileName = postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 4) + "_" + index + extension; ;
                        }
                        else
                        {
                            File += fileFilePath + postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 4) + "_" + index + extension; ;
                            fileName = postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 4) + "_" + index + extension; ;
                        }
                        var filePath = HttpContext.Current.Server.MapPath("~/images/News/Files/" + fileName);
                        postedFile.SaveAs(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ImageFile))
                {
                    item.newsImage += ImageFile;
                }
                else
                {
                    item.newsImage = item.newsImage;
                }

                if (!string.IsNullOrEmpty(File))
                {
                    item.newsFile += File;
                }
                else
                {
                    item.newsFile = item.newsFile;
                }
                newsItem.newsImage = item.newsImage;
                newsItem.newsFile = item.newsFile;
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
