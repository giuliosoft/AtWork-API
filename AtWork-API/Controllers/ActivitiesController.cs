using API_Placement_record_management.Models;
using AtWork_API.Filters;
using AtWork_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                           join pg in db.tbl_Activity_Pictures on a.proUniqueID equals pg.proUniqueID into pgs
                           from m in pgs.DefaultIfEmpty()
                           select new
                           {
                               a.id,
                               a.proUniqueID,
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
                               ///proBackgroundImage= string.Join(",", m.picFileName.ToArray()),
                               proBackgroundImage = m.picFileName,
                               a.proDeliveryMethod,
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

        [Route("joinActitvity")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult JoinActivity(tbl_Join_Activities objActivities)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_InsertJoinActivity", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@coUniqueID", objActivities.coUniqueID);
                sqlCmd.Parameters.AddWithValue("@proUniqueID", objActivities.proUniqueID);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", objActivities.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@ActivityID", objActivities.ActivityID);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.InsertSuccessMessage;
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
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("deletejoinActitvity/{id}")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DeleteJoinActivity(int Id)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("DeleteJoinActitvity", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@Id", Id);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.DeleteSuccessMessage;
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
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

    }
}
