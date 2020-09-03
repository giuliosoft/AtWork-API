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
    [RoutePrefix("api/activities")]
    public class ActivitiesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getlist/{id}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetActivityFeed(string id)
        {
            CommonResponse objResponse = new CommonResponse();
            try
            {
                var list = from a in db.tbl_Activities
                           select new
                           {
                               a.id,
                               a.proTitle,
                               a.coUniqueID,
                               a.proDescription,
                               a.proLocation,
                               a.proAddActivityDate,
                               a.proAddActivity_StartTime,
                               a.proAddActivity_EndTime,
                               a.proAddress1,
                               a.proAddress2,
                               a.proPostalCode,
                               a.proCity,
                               a.proCategoryName,
                               a.proSubCategoryName,
                               a.proStatus,
                               a.proPartner,
                               a.proAddActivity_HoursCommitted,
                               a.proAddActivity_ParticipantsMinNumber,
                               a.proAddActivity_ParticipantsMaxNumber,
                               a.proBackgroundImage,
                               a.proDeliveryMethod
                           };
                if (list != null)
                {
                    list = list.Where(x => x.coUniqueID == id);
                }
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = list;
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("getrow/{id}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetRow(string id)
        {
            try
            {
                var row = db.tbl_Activities.FirstOrDefault(a => a.proUniqueID == id);
                return Ok(row);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("insertrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult InsertRow([FromBody] tbl_Activities a)
        {
            try
            {
                a.proUniqueID = $"vol{DateTime.UtcNow.ToString()}";
                db.tbl_Activities.Add(a);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("updaterow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult UpdateRow([FromBody] tbl_Activities a)
        {
            try
            {
                tbl_Activities toDB = db.tbl_Activities.FirstOrDefault(x => x.proUniqueID == a.proUniqueID);
                toDB.proTitle = a.proTitle;
                toDB.proDescription = a.proDescription;
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
                tbl_Activities a = db.tbl_Activities.FirstOrDefault(x => x.proUniqueID == id);
                db.tbl_Activities.Remove(a);
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
