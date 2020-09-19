using API_Placement_record_management.Models;
using AtWork_API.Filters;
using AtWork_API.Helpers;
using AtWork_API.Models;
using AtWork_API.ViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
                                 //LikeCount = (from a in db.tbl_News_Comments_Likes
                                 //             where a.newsCommentId.Equals(m.Id)
                                 //             select a).Count(),
                                 LikeCount = 0,
                                 Day = a.newsDateTime.ToString()

                             }).DistinctBy(s => s.news.id).OrderByDescending(a => a.news.id).ToList().Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage).ToList();


                model.ForEach(x => x.Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(x.Day)));
                model.ForEach(x => x.LikeCount = GetNewLike(Convert.ToInt32(x.news.id)));

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
                CommonMethods.SaveError(ex, "ComUniqueID :"+ ComUniqueID);
                return Ok(objResponse);
            }
        }

        [Route("getlist_v1/{ComUniqueID}/{pageNumber}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetNewsList_v1(string ComUniqueID, int pageNumber)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            NewsList objNews = null;
            List<NewsList> lstNewsList = new List<NewsList>();
            string volUniqueID = string.Empty;
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);
                volUniqueID = Volunteers.volUniqueID;
                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_SelectAllNews", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@coUniqueID", ComUniqueID);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@pageNumber", pageNumber);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    objNews = new NewsList();
                    objNews.news = new tbl_News();
                    objNews.Volunteers = new tbl_Volunteers();

                    objNews.CommentsCount = (Convert.ToInt32(sqlRed["CommentsCount"]));
                    objNews.LikeCount = (Convert.ToInt32(sqlRed["LikeCount"]));
                    objNews.news.id = (Convert.ToInt32(sqlRed["id"]));
                    objNews.news.coUniqueID = (Convert.ToString(sqlRed["coUniqueID"]));
                    objNews.news.newsUniqueID = (Convert.ToString(sqlRed["newsUniqueID"]));
                    objNews.news.volUniqueID = (Convert.ToString(sqlRed["volUniqueID"]));
                    objNews.news.newsTitle = (Convert.ToString(sqlRed["newsTitle"]));
                    objNews.news.newsContent = (Convert.ToString(sqlRed["newsContent"]));
                    objNews.news.newsDateTime = (Convert.ToDateTime(sqlRed["newsDateTime"]));
                    objNews.news.newsPostedTime = (Convert.ToDateTime(sqlRed["newsPostedTime"]));
                    objNews.news.newsPrivacy = (Convert.ToString(sqlRed["newsPrivacy"]));
                    objNews.news.newsStatus = (Convert.ToString(sqlRed["newsStatus"]));
                    objNews.news.newsOrigin = (Convert.ToString(sqlRed["newsOrigin"]));
                    objNews.news.newsImage = (Convert.ToString(sqlRed["newsImage"]));
                    objNews.news.newsFile = (Convert.ToString(sqlRed["newsFile"]));
                    objNews.news.newsFileOriginal = (Convert.ToString(sqlRed["newsFileOriginal"]));

                    objNews.Volunteers.coUniqueID = (Convert.ToString(sqlRed["coUniqueID"]));
                    objNews.Volunteers.volUniqueID = (Convert.ToString(sqlRed["volUniqueID"]));
                    objNews.Volunteers.volFirstName = (Convert.ToString(sqlRed["volFirstName"]));
                    objNews.Volunteers.volLastName = (Convert.ToString(sqlRed["volLastName"]));
                    objNews.Volunteers.volGender = (Convert.ToString(sqlRed["volGender"]));
                    objNews.Volunteers.volUserName = (Convert.ToString(sqlRed["volUserName"]));

                    objNews.Volunteers.VolUserPassword = (Convert.ToString(sqlRed["VolUserPassword"]));
                    objNews.Volunteers.volEmail = (Convert.ToString(sqlRed["volEmail"]));
                    if (sqlRed["volOnBoardStatus"] != DBNull.Value)
                    {
                        objNews.Volunteers.volOnBoardStatus = (Convert.ToString(sqlRed["volOnBoardStatus"]));
                    }
                    if (sqlRed["volOnBoardDateSent"] != DBNull.Value)
                    {
                        objNews.Volunteers.volOnBoardDateSent = (Convert.ToDateTime(sqlRed["volOnBoardDateSent"]));
                    }
                    
                    objNews.Volunteers.volPicture = (Convert.ToString(sqlRed["volPicture"]));
                    objNews.Volunteers.volEducation = (Convert.ToString(sqlRed["volEducation"]));
                    objNews.Volunteers.volCompetencies = (Convert.ToString(sqlRed["volCompetencies"]));
                    objNews.Volunteers.volCategories = (Convert.ToString(sqlRed["volCategories"]));
                    objNews.Volunteers.volPhone = (Convert.ToString(sqlRed["volPhone"]));
                    objNews.Volunteers.volStatus = (Convert.ToString(sqlRed["volStatus"]));
                    objNews.Volunteers.restricted = (Convert.ToString(sqlRed["restricted"]));
                    objNews.Volunteers.reviewStatus = (Convert.ToString(sqlRed["reviewStatus"]));
                    objNews.CommentsCount = (Convert.ToInt32(sqlRed["CommentsCount"]));
                    objNews.LikeCount = (Convert.ToInt32(sqlRed["LikeCount"]));
                    objNews.Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(sqlRed["newsDateTime"]));

                    lstNewsList.Add(objNews);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lstNewsList;

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                CommonMethods.SaveError(ex, volUniqueID);
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("getrow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetRow(int id)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataReader sqlRed = null;
            string volUniqueID = string.Empty;
            try
            {
                tbl_News obj = db.tbl_News.FirstOrDefault(x => x.id == id);
                int i = 0;
                int data = db.tbl_News_Comments_Likes.Count(a => a.newsCommentId == obj.id);
                var user = db.tbl_Volunteers.FirstOrDefault(a => a.volUniqueID == obj.volUniqueID);
                volUniqueID = user.volUniqueID;
                NewsCommets objComments = new NewsCommets();

                objComments.News = obj;
                objComments.Comments_Likes = data;
                objComments.Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(obj.newsDateTime));
                objComments.Volunteers = user;

                //if (obj.volUniqueID == volUniqueID)
                //{
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("Count_news_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@id", id);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", obj.volUniqueID);

                //sqlCmd.Parameters.Add("@CountData", SqlDbType.Int).Direction = ParameterDirection.Output;
                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    objComments.LikeCount = Convert.ToInt32(sqlRed["newsLikeCount"]);
                }
                sqlRed.NextResult();
                while (sqlRed.Read())
                {
                    objComments.LikeId = Convert.ToInt32(sqlRed["Likeid"]);
                }

                DataObjectFactory.CloseConnection(sqlCon);
                if (objComments.LikeId > 0)
                {
                    objComments.LikeByLoginUser = true;
                }
                //int CountData = (int)sqlCmd.Parameters["@CountData"].Value;
                //}
                //if (CountData > 0)
                //{
                //    objComments.LikeByLoginUser = true;
                //}
                //}

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
                CommonMethods.SaveError(ex, volUniqueID);
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("getrow_v1/{id}/{volUniqueID}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetRow_v1(int id, string volUniqueID)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataReader sqlRed = null;
            try
            {
                tbl_News obj = db.tbl_News.FirstOrDefault(x => x.id == id);
                int i = 0;
                int data = db.tbl_News_Comments_Likes.Count(a => a.newsCommentId == obj.id);
                var user = db.tbl_Volunteers.FirstOrDefault(a => a.volUniqueID == obj.volUniqueID);

                NewsCommets objComments = new NewsCommets();

                objComments.News = obj;
                objComments.Comments_Likes = data;
                objComments.Day = CommonMethods.getRelativeDateTime(Convert.ToDateTime(obj.newsDateTime));
                objComments.Volunteers = user;

                //if (obj.volUniqueID == volUniqueID)
                //{
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("Count_news_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@id", id);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", volUniqueID);

                //sqlCmd.Parameters.Add("@CountData", SqlDbType.Int).Direction = ParameterDirection.Output;
                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    objComments.LikeCount = Convert.ToInt32(sqlRed["newsLikeCount"]);
                }
                sqlRed.NextResult();
                while (sqlRed.Read())
                {
                    objComments.LikeId = Convert.ToInt32(sqlRed["Likeid"]);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                if (objComments.LikeId > 0)
                {
                    objComments.LikeByLoginUser = true;
                }
                //if (obj.volUniqueID == volUniqueID)
                //{
                //    objComments.LikeByLoginUser = true;
                //}


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
                CommonMethods.SaveError(ex, volUniqueID);
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("addrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult AddRow()
        {
            CommonResponse objResponse = new CommonResponse();
            string imagesPath = "~/newsposts/";
            string filesPath = "~/newspostsfiles/";
            string userUniqueID = string.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                tbl_News item = JsonConvert.DeserializeObject<tbl_News>(httpRequest.Params["Data"].ToString());
                string volUniqueID = item.volUniqueID.Substring(item.volUniqueID.Length - 3);
                userUniqueID = item.volUniqueID;
                int counter = 1;
                var newId = "newscorp" + DateTime.UtcNow.Ticks + volUniqueID;
                while (db.tbl_News.Any(a => a.newsUniqueID == newId))
                {
                    newId = "newscorp" + counter + DateTime.UtcNow.Ticks + volUniqueID;
                    counter++;
                }


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
                        if (ImageFile != string.Empty && ImageFile != "")
                        {
                            ImageFile += "," + newId.Substring(newId.Length - 5) + "_" + index + extension;
                            fileName = newId.Substring(newId.Length - 5) + "_" + index + extension;
                        }
                        else
                        {
                            ImageFile = newId.Substring(newId.Length - 5) + "_" + index + extension;
                            fileName = newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                        }
                        var filePath = HttpContext.Current.Server.MapPath(imagesPath + fileName);
                        postedFile.SaveAs(filePath);
                    }
                    else
                    {
                        if (File != string.Empty && File != "")
                        {
                            File += "," + newId.Substring(newId.Length - 5) + "_" + index + extension;
                            fileName = newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                        }
                        else
                        {
                            File += newId.Substring(newId.Length - 5) + "_" + index + extension;
                            fileName = newId.Substring(newId.Length - 5) + "_" + index + extension; ;
                        }
                        var filePath = HttpContext.Current.Server.MapPath(filesPath + fileName);
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
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                CommonMethods.SaveError(ex, userUniqueID);
                return Ok(objResponse);
            }
        }

        [Route("editrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult EditRow()
        {
            CommonResponse objResponse = new CommonResponse();
            string imagesPath = "~/newsposts/";
            string filesPath = "~/newspostsfiles/";
            string ImageFile = string.Empty;
            string File = string.Empty;
            string volUniqueID = string.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                tbl_News item = JsonConvert.DeserializeObject<tbl_News>(httpRequest.Params["Data"].ToString());
                volUniqueID = item.volUniqueID;
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
                string ImageName = string.Empty;
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
                            ImageFile += "," + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                            ImageName = newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                        }
                        else
                        {
                            ImageFile += newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                            ImageName = newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                        }
                        var filePath = HttpContext.Current.Server.MapPath(imagesPath + ImageName);
                        postedFile.SaveAs(filePath);
                    }
                    else
                    {
                        if (item.newsFile != null && item.newsFile != "")
                        {
                            File += item.newsImage + "," + postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                            fileName = postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                        }
                        else
                        {
                            File += postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                            fileName = postedFile.FileName + newsItem.newsUniqueID.Substring(newsItem.newsUniqueID.Length - 5) + DateTime.UtcNow.Ticks + "_" + index + extension; ;
                        }
                        var filePath = HttpContext.Current.Server.MapPath(filesPath + fileName);
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
                CommonMethods.SaveError(ex, volUniqueID);
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
                CommonMethods.SaveError(ex, "news id" + id);
                return Ok(objResponse);
            }
        }

        public int GetNewLike(int id)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            int CountData = 0;

            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_InsertNews_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("Count_news_Like", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@id", id);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", DBNull.Value);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();


                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    CountData = Convert.ToInt32(sqlRed["newsLikeCount"]);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                return CountData;
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, string.Empty);
                return 0;
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }
    }
}
