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

        [Route("getlist/{id}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetCombinedNewsList(string id)
        {
            try
            {
                var list = db.CombinedNewsItems.Where(x => x.coUniqueID == id);
                list = list.OrderBy(x => x.newsDateTime >= DateTime.Today);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("getrow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetRow(string id)
        {
            try
            {
                VolunteerNewsItem obj = db.VolunteerNewsItems.FirstOrDefault(x => x.newsUniqueID == id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("addrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult AddRow([FromBody] NewsItem item)
        {
            try
            {
                db.NewsItems.Add(item);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("editrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult EditRow([FromBody] NewsItem item)
        {
            try
            {
                NewsItem newsItem = db.NewsItems.FirstOrDefault(x => x.newsUniqueID == item.newsUniqueID);
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
                return BadRequest();
            }
        }

        [Route("deleterow/{id}")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DeleteRow(string id)
        {
            try
            {
                var item = db.NewsItems.FirstOrDefault(x => x.newsUniqueID == id);
                db.NewsItems.Remove(item);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
