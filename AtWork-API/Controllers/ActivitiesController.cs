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
using System.Web;
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
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                tbl_Activities obj = null;
                List<tbl_Activities> lstActivities = new List<tbl_Activities>();

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("SelectAllActivity", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@coUniqueID", id);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new tbl_Activities();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.proTitle = Convert.ToString(sqlRed["proTitle"]);
                    obj.proUniqueID = Convert.ToString(sqlRed["proUniqueID"]);
                    obj.coUniqueID = Convert.ToString(sqlRed["coUniqueID"]);
                    obj.proDescription = Convert.ToString(sqlRed["proDescription"]);
                    obj.proLocation = Convert.ToString(sqlRed["proLocation"]);
                    obj.proAddActivityDate = Convert.ToDateTime(sqlRed["proAddActivityDate"]);
                    obj.proAddActivity_EndTime = Convert.ToString(sqlRed["proAddActivity_StartTime"]);
                    obj.proAddress1 = Convert.ToString(sqlRed["proAddress1"]);
                    obj.proAddress2 = Convert.ToString(sqlRed["proAddress2"]);
                    obj.proPostalCode = Convert.ToString(sqlRed["proPostalCode"]);
                    obj.proCity = Convert.ToString(sqlRed["proCity"]);
                    obj.proCategoryName = Convert.ToString(sqlRed["proCategoryName"]);
                    obj.proSubCategoryName = Convert.ToString(sqlRed["proSubCategoryName"]);
                    obj.proStatus = Convert.ToString(sqlRed["proStatus"]);
                    obj.proPartner = Convert.ToString(sqlRed["proPartner"]);
                    obj.proAddActivity_HoursCommitted = Convert.ToString(sqlRed["proAddActivity_HoursCommitted"]);
                    obj.proAddActivity_ParticipantsMinNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMinNumber"]);
                    obj.proAddActivity_ParticipantsMaxNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMaxNumber"]);
                    obj.proBackgroundImage = Convert.ToString(sqlRed["picFileName"]);

                    lstActivities.Add(obj);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lstActivities;

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
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
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
        public IHttpActionResult InsertRow([FromBody] tbl_Activities objActivities)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            objActivities.proUniqueID = $"vol{DateTime.UtcNow.ToString()}";
            string activitiesPath = "~/activities/";
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_Add_Activities", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@proUniqueID", objActivities.proUniqueID);
                sqlCmd.Parameters.AddWithValue("@proTitle", objActivities.proTitle);
                sqlCmd.Parameters.AddWithValue("@coUniqueID", objActivities.coUniqueID);
                sqlCmd.Parameters.AddWithValue("@proCompany", objActivities.proCompany);
                sqlCmd.Parameters.AddWithValue("@proDescription", objActivities.proDescription);
                sqlCmd.Parameters.AddWithValue("@proLocation", objActivities.proLocation);
                sqlCmd.Parameters.AddWithValue("@proAddress1", objActivities.proAddress1);
                sqlCmd.Parameters.AddWithValue("@proAddress2", objActivities.proAddress2);
                sqlCmd.Parameters.AddWithValue("@proPostalCode", objActivities.proPostalCode);
                sqlCmd.Parameters.AddWithValue("@proCity", objActivities.proCity);
                sqlCmd.Parameters.AddWithValue("@proCountry", objActivities.proCountry);
                sqlCmd.Parameters.AddWithValue("@proContinent", objActivities.proContinent);
                sqlCmd.Parameters.AddWithValue("@proTimeCommitment", objActivities.proTimeCommitment);
                sqlCmd.Parameters.AddWithValue("@proTimeCommitmentDecimal", objActivities.proTimeCommitmentDecimal);
                sqlCmd.Parameters.AddWithValue("@proDatesConfirmed", objActivities.proDatesConfirmed);
                sqlCmd.Parameters.AddWithValue("@proType", objActivities.proType);
                sqlCmd.Parameters.AddWithValue("@proCategory", objActivities.proCategory);
                sqlCmd.Parameters.AddWithValue("@proCategoryName", objActivities.proCategoryName);
                sqlCmd.Parameters.AddWithValue("@proSubCategory", objActivities.proSubCategory);
                sqlCmd.Parameters.AddWithValue("@proSubCategoryName", objActivities.proSubCategoryName);
                sqlCmd.Parameters.AddWithValue("@proStatus", objActivities.proStatus);
                sqlCmd.Parameters.AddWithValue("@proPartner", objActivities.proPartner);
                sqlCmd.Parameters.AddWithValue("@proPartnerEmail", objActivities.proPartnerEmail);
                sqlCmd.Parameters.AddWithValue("@proActivityLanguage", objActivities.proActivityLanguage);
                sqlCmd.Parameters.AddWithValue("@proActivityLanguageID", objActivities.proActivityLanguageID);
                sqlCmd.Parameters.AddWithValue("@proAudience", objActivities.proAudience);
                sqlCmd.Parameters.AddWithValue("@proSpecialRequirements", objActivities.proSpecialRequirements);
                sqlCmd.Parameters.AddWithValue("@proCostCoveredEmployee", objActivities.proCostCoveredEmployee);
                sqlCmd.Parameters.AddWithValue("@proCostCoveredCompany", objActivities.proCostCoveredCompany);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_HoursCommitted", objActivities.proAddActivity_HoursCommitted);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_StartTime", objActivities.proAddActivity_StartTime);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_EndTime", objActivities.proAddActivity_EndTime);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_ParticipantsMinNumber", objActivities.proAddActivity_ParticipantsMinNumber);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_ParticipantsMaxNumber", objActivities.proAddActivity_ParticipantsMaxNumber);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_OrgName", objActivities.proAddActivity_OrgName);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_Website", objActivities.proAddActivity_Website);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_AdditionalInfo", objActivities.proAddActivity_AdditionalInfo);
                sqlCmd.Parameters.AddWithValue("@proAddActivity_CoordinatorEmail", objActivities.proAddActivity_CoordinatorEmail);
                sqlCmd.Parameters.AddWithValue("@proPublishedDate", objActivities.proPublishedDate);
                sqlCmd.Parameters.AddWithValue("@proAddActivityDate", objActivities.proAddActivityDate);
                sqlCmd.Parameters.AddWithValue("@proBackgroundImage", objActivities.proBackgroundImage);
                sqlCmd.Parameters.AddWithValue("@proDeliveryMethod", objActivities.proDeliveryMethod);
                //sqlCmd.Parameters.AddWithValue("@VolUniqueID", objActivities.v);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                if (i > 0)
                {
                    string ImageFile = string.Empty;
                    int index = 0;
                    var httpRequest = HttpContext.Current.Request;
                    tbl_Activity_Pictures objActivity_Pictures = null;

                    foreach (string file in httpRequest.Files)
                    {
                        index++;
                        var postedFile = httpRequest.Files[file];
                        string extension = System.IO.Path.GetExtension(postedFile.FileName);
                        if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                        {
                            objActivity_Pictures = new tbl_Activity_Pictures();
                            objActivity_Pictures.coUniqueID = objActivities.coUniqueID;
                            objActivity_Pictures.proUniqueID = objActivities.proUniqueID;
                            objActivity_Pictures.picUniqueID = "procorp" + DateTime.UtcNow.Ticks + index;
                            objActivity_Pictures.picFileName = DateTime.UtcNow.Ticks + "_" + index + extension;

                            sqlCon = DataObjectFactory.CreateNewConnection();
                            sqlCmd = new SqlCommand("sp_Add_Activities", sqlCon);
                            sqlCmd.CommandType = CommandType.StoredProcedure;

                            sqlCmd.Parameters.AddWithValue("@coUniqueID", objActivity_Pictures.coUniqueID);
                            sqlCmd.Parameters.AddWithValue("@proUniqueID", objActivity_Pictures.proUniqueID);
                            sqlCmd.Parameters.AddWithValue("@picUniqueID", objActivity_Pictures.picUniqueID);
                            sqlCmd.Parameters.AddWithValue("@picFileName", objActivity_Pictures.picFileName);
                            sqlCmd.Parameters.AddWithValue("@proStatus", objActivities.proStatus);

                            DataObjectFactory.OpenConnection(sqlCon);
                            int j = sqlCmd.ExecuteNonQuery();
                            DataObjectFactory.CloseConnection(sqlCon);

                            var filePath = HttpContext.Current.Server.MapPath(activitiesPath + objActivity_Pictures.picFileName);
                            postedFile.SaveAs(filePath);
                        }
                    }
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

        [Route("MyActivity/{volUniqueID}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult MyActivityFeed(string volUniqueID)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                tbl_Activities obj = null;
                List<tbl_Activities> lstActivities = new List<tbl_Activities>();
                List<tbl_Activities> PastlstActivities = new List<tbl_Activities>();

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_SelectMyActitvity", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueID", volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new tbl_Activities();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.proTitle = Convert.ToString(sqlRed["proTitle"]);
                    obj.proUniqueID = Convert.ToString(sqlRed["proUniqueID"]);
                    obj.coUniqueID = Convert.ToString(sqlRed["coUniqueID"]);
                    obj.proDescription = Convert.ToString(sqlRed["proDescription"]);
                    obj.proLocation = Convert.ToString(sqlRed["proLocation"]);
                    obj.proAddActivityDate = Convert.ToDateTime(sqlRed["proAddActivityDate"]);
                    obj.proAddActivity_EndTime = Convert.ToString(sqlRed["proAddActivity_StartTime"]);
                    obj.proAddress1 = Convert.ToString(sqlRed["proAddress1"]);
                    obj.proAddress2 = Convert.ToString(sqlRed["proAddress2"]);
                    obj.proPostalCode = Convert.ToString(sqlRed["proPostalCode"]);
                    obj.proCity = Convert.ToString(sqlRed["proCity"]);
                    obj.proCategoryName = Convert.ToString(sqlRed["proCategoryName"]);
                    obj.proSubCategoryName = Convert.ToString(sqlRed["proSubCategoryName"]);
                    obj.proStatus = Convert.ToString(sqlRed["proStatus"]);
                    obj.proPartner = Convert.ToString(sqlRed["proPartner"]);
                    obj.proAddActivity_HoursCommitted = Convert.ToString(sqlRed["proAddActivity_HoursCommitted"]);
                    obj.proAddActivity_ParticipantsMinNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMinNumber"]);
                    obj.proAddActivity_ParticipantsMaxNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMaxNumber"]);
                    obj.proBackgroundImage = Convert.ToString(sqlRed["picFileName"]);

                    lstActivities.Add(obj);
                }
                sqlRed.NextResult();
                while (sqlRed.Read())
                {
                    obj = new tbl_Activities();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.proTitle = Convert.ToString(sqlRed["proTitle"]);
                    obj.proUniqueID = Convert.ToString(sqlRed["proUniqueID"]);
                    obj.coUniqueID = Convert.ToString(sqlRed["coUniqueID"]);
                    obj.proDescription = Convert.ToString(sqlRed["proDescription"]);
                    obj.proLocation = Convert.ToString(sqlRed["proLocation"]);
                    obj.proAddActivityDate = Convert.ToDateTime(sqlRed["proAddActivityDate"]);
                    obj.proAddActivity_EndTime = Convert.ToString(sqlRed["proAddActivity_StartTime"]);
                    obj.proAddress1 = Convert.ToString(sqlRed["proAddress1"]);
                    obj.proAddress2 = Convert.ToString(sqlRed["proAddress2"]);
                    obj.proPostalCode = Convert.ToString(sqlRed["proPostalCode"]);
                    obj.proCity = Convert.ToString(sqlRed["proCity"]);
                    obj.proCategoryName = Convert.ToString(sqlRed["proCategoryName"]);
                    obj.proSubCategoryName = Convert.ToString(sqlRed["proSubCategoryName"]);
                    obj.proStatus = Convert.ToString(sqlRed["proStatus"]);
                    obj.proPartner = Convert.ToString(sqlRed["proPartner"]);
                    obj.proAddActivity_HoursCommitted = Convert.ToString(sqlRed["proAddActivity_HoursCommitted"]);
                    obj.proAddActivity_ParticipantsMinNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMinNumber"]);
                    obj.proAddActivity_ParticipantsMaxNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMaxNumber"]);
                    obj.proBackgroundImage = Convert.ToString(sqlRed["picFileName"]);

                    PastlstActivities.Add(obj);
                }

                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lstActivities;
                objResponse.Data1 = PastlstActivities;

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                return BadRequest();
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
