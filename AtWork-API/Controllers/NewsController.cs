using AtWork_API.Filters;
using AtWork_API.Helpers;
using AtWork_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            try
            {
                var list = db.tbl_News.Where(x => x.coUniqueID == ComUniqueID);
                list = list.OrderBy(x => x.newsDateTime >= DateTime.Today);
                if (list != null)
                {
                    return Content(HttpStatusCode.OK, list);
                }
                return Content(HttpStatusCode.OK, "No record found");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("getrow/{NewsUniqueID}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetRow(string NewsUniqueID)
        {
            try
            {
                tbl_News obj = db.tbl_News.FirstOrDefault(x => x.newsUniqueID == NewsUniqueID);
                return Content(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("addrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult AddRow([FromBody] tbl_News item)
        {
            try
            {
                db.tbl_News.Add(item);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("editrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult EditRow([FromBody] tbl_News item)
        {
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
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("deleterow/{newsUniqueID}")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DeleteRow(string newsUniqueID)
        {
            try
            {
                var item = db.tbl_News.FirstOrDefault(x => x.newsUniqueID == newsUniqueID);
                db.tbl_News.Remove(item);
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
