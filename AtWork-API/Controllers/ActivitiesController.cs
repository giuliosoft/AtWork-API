using API_Placement_record_management.Models;
using AtWork_API.Filters;
using AtWork_API.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
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
    [RoutePrefix("api/activities")]
    public class ActivitiesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getlist/{id}/{Catid?}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetActivityFeed(string id, string catId = null)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string token = string.Empty;
            var re = Request;
            var headers = re.Headers;

            token = headers.GetValues("Authorization").First();

            string encodedHeader = token.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

            int separatorIndex = usernamePassword.IndexOf(":");
            string username = usernamePassword.Substring(0, separatorIndex);
            string password = usernamePassword.Substring(separatorIndex + 1);

            var Volunteers = db.tbl_Volunteers.FirstOrDefault(u => u.volUserName == username && u.VolUserPassword == password);


            try
            {
                Activities obj = null;
                List<Activities> lstActivities = new List<Activities>();
                List<Activities> lstDefultImage = new List<Activities>();

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("SelectAllActivity_v1", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@coUniqueID", id);
                sqlCmd.Parameters.AddWithValue("@catId", catId);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new Activities();
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
                    obj.Member = Convert.ToString(sqlRed["Member"]) + " " + "Joined";
                    lstActivities.Add(obj);
                }
                sqlRed.NextResult();
                while (sqlRed.Read())
                {
                    obj = new Activities();
                    if (sqlRed["coWhiteLabelGTPicStatus"].ToString().ToLower() == "no")
                    {
                        obj.proBackgroundImage = "picture1.png,picture2.png,picture3.png";
                    }
                    else if (sqlRed["coWhiteLabelGTPicStatus"].ToString().ToLower() == "yes")
                    {
                        obj.proBackgroundImage = "picture1" + obj.coUniqueID + ".png" + ",picture2" + obj.coUniqueID + ".png" + ",picture3" + obj.coUniqueID + ".png";
                    }
                    lstDefultImage.Add(obj);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lstActivities.DistinctBy(a => a.id);
                objResponse.Data1 = lstDefultImage;

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
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataReader sqlRed = null;
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_SelectDetailsActivities", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@proUniqueID", id);
                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                Activities obj = null;
                if (sqlRed.Read())
                {
                    obj = new Activities();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.proUniqueID = Convert.ToString(sqlRed["proUniqueID"]);
                    obj.proTitle = Convert.ToString(sqlRed["proTitle"]);
                    obj.coUniqueID = Convert.ToString(sqlRed["coUniqueID"]);
                    obj.proDescription = Convert.ToString(sqlRed["proDescription"]);
                    obj.proAddress1 = Convert.ToString(sqlRed["proAddress1"]);
                    obj.proAddress2 = Convert.ToString(sqlRed["proAddress2"]);
                    obj.proLocation = Convert.ToString(sqlRed["proLocation"]);
                    obj.proPostalCode = Convert.ToString(sqlRed["proPostalCode"]);
                    obj.proCity = Convert.ToString(sqlRed["proCity"]);
                    obj.proCountry = Convert.ToString(sqlRed["proCountry"]);
                    obj.proContinent = Convert.ToString(sqlRed["proContinent"]);
                    obj.proTimeCommitment = Convert.ToString(sqlRed["proTimeCommitment"]);
                    obj.proTimeCommitmentDecimal = Convert.ToString(sqlRed["proTimeCommitmentDecimal"]);
                    obj.proDatesConfirmed = Convert.ToString(sqlRed["proDatesConfirmed"]);
                    obj.proType = Convert.ToString(sqlRed["proType"]);
                    obj.proCategory = Convert.ToString(sqlRed["proCategory"]);
                    obj.proCategoryName = Convert.ToString(sqlRed["proCategoryName"]);
                    obj.proSubCategory = Convert.ToString(sqlRed["proSubCategory"]);
                    obj.proSubCategoryName = Convert.ToString(sqlRed["proSubCategoryName"]);
                    obj.proStatus = Convert.ToString(sqlRed["proStatus"]);
                    obj.proPartner = Convert.ToString(sqlRed["proPartner"]);
                    obj.proPartnerEmail = Convert.ToString(sqlRed["proPartnerEmail"]);
                    obj.proActivityLanguage = Convert.ToString(sqlRed["proActivityLanguage"]);
                    obj.proActivityLanguageID = Convert.ToString(sqlRed["proActivityLanguageID"]);
                    obj.proAudience = Convert.ToString(sqlRed["proAudience"]);
                    obj.proSpecialRequirements = Convert.ToString(sqlRed["proSpecialRequirements"]);
                    obj.proCostCoveredEmployee = Convert.ToString(sqlRed["proCostCoveredEmployee"]);
                    obj.proCostCoveredCompany = Convert.ToString(sqlRed["proCostCoveredCompany"]);
                    obj.proAddActivity_HoursCommitted = Convert.ToString(sqlRed["proAddActivity_HoursCommitted"]);
                    obj.proAddActivity_StartTime = Convert.ToString(sqlRed["proAddActivity_StartTime"]);
                    obj.proAddActivity_EndTime = Convert.ToString(sqlRed["proAddActivity_EndTime"]);
                    obj.proAddActivity_ParticipantsMinNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMinNumber"]);
                    obj.proAddActivity_ParticipantsMaxNumber = Convert.ToString(sqlRed["proAddActivity_ParticipantsMaxNumber"]);
                    obj.proAddActivity_OrgName = Convert.ToString(sqlRed["proAddActivity_OrgName"]);
                    obj.proAddActivity_Website = Convert.ToString(sqlRed["proAddActivity_Website"]);
                    obj.proAddActivity_AdditionalInfo = Convert.ToString(sqlRed["proAddActivity_AdditionalInfo"]);
                    obj.proAddActivity_CoordinatorEmail = Convert.ToString(sqlRed["proAddActivity_CoordinatorEmail"]);
                    obj.proPublishedDate = Convert.ToDateTime(sqlRed["proPublishedDate"]);
                    obj.proAddActivityDate = Convert.ToDateTime(sqlRed["proAddActivityDate"]);
                    obj.proDeliveryMethod = Convert.ToString(sqlRed["proDeliveryMethod"]);
                    obj.proBackgroundImage = Convert.ToString(sqlRed["picFileName"]);
                    obj.StartDate = Convert.ToString(sqlRed["StartDate"]);
                    obj.EndDate = Convert.ToString(sqlRed["StartDate"]);
                    obj.DataType = Convert.ToString(sqlRed["dataType"]);
                    obj.proVolHourDates = Convert.ToString(sqlRed["proVolHourDates"]);
                    obj.Member = Convert.ToString(sqlRed["Member"]) + " " + "Joined";
                }
                DataObjectFactory.CloseConnection(sqlCon);
                sqlRed.Close();
                return Ok(obj);
            }
            //try
            //{
            //    var row = db.tbl_Activities.FirstOrDefault(a => a.proUniqueID == id);
            //    return Ok(row);
            //}
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("insertrow")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult InsertRow()
        {
            var httpRequest = HttpContext.Current.Request;
            Activities objActivities = JsonConvert.DeserializeObject<Activities>(httpRequest.Params["Data"].ToString());

            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            objActivities.proUniqueID = "procorp" + DateTime.UtcNow.Ticks;
            string activitiesPath = "~/activities/";
            //string activitiesPath = "~/images/";
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
                sqlCmd.Parameters.AddWithValue("@proCategory", "cat006");
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
                sqlCmd.Parameters.AddWithValue("@VolUniqueID", objActivities.volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                if (i > 0)
                {
                    #region Activity_Date
                    sqlCon = DataObjectFactory.CreateNewConnection();
                    sqlCmd = new SqlCommand("sp_InsertActivityDate", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@coUniqueID", objActivities.coUniqueID);
                    sqlCmd.Parameters.AddWithValue("@proUniqueID", objActivities.proUniqueID);
                    sqlCmd.Parameters.AddWithValue("@dates", objActivities.proAddActivityDate);
                    sqlCmd.Parameters.AddWithValue("@dateType", "Regular");

                    DataObjectFactory.OpenConnection(sqlCon);
                    int d = sqlCmd.ExecuteNonQuery();
                    DataObjectFactory.CloseConnection(sqlCon);

                    #endregion
                    #region ImageUpload
                    string ImageFile = string.Empty;
                    int index = 0;

                    tbl_Activity_Pictures objActivity_Pictures = null;
                    if (!string.IsNullOrEmpty(objActivities.proBackgroundImage))
                    {
                        objActivity_Pictures = new tbl_Activity_Pictures();
                        objActivity_Pictures.coUniqueID = objActivities.coUniqueID;
                        objActivity_Pictures.proUniqueID = objActivities.proUniqueID;
                        objActivity_Pictures.picUniqueID = "procorp" + DateTime.UtcNow.Ticks + index;
                        objActivity_Pictures.picFileName = objActivities.proBackgroundImage;

                        sqlCon = DataObjectFactory.CreateNewConnection();
                        sqlCmd = new SqlCommand("sp_Insert_Activity_Pictures", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.AddWithValue("@coUniqueID", objActivity_Pictures.coUniqueID);
                        sqlCmd.Parameters.AddWithValue("@proUniqueID", objActivity_Pictures.proUniqueID);
                        sqlCmd.Parameters.AddWithValue("@picUniqueID", objActivity_Pictures.picUniqueID);
                        sqlCmd.Parameters.AddWithValue("@picFileName", objActivity_Pictures.picFileName);
                        sqlCmd.Parameters.AddWithValue("@proStatus", objActivities.proStatus);

                        DataObjectFactory.OpenConnection(sqlCon);
                        int j = sqlCmd.ExecuteNonQuery();
                        DataObjectFactory.CloseConnection(sqlCon);
                    }
                    else
                    {
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
                                sqlCmd = new SqlCommand("sp_Insert_Activity_Pictures", sqlCon);
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
                    }

                    #endregion
                    #region Emoji
                    tbl_Activity_GetTogether_Emoticons objActivity_GetTogether_Emoticons = null;
                    List<tbl_Activity_GetTogether_Emoticons> lst = new List<tbl_Activity_GetTogether_Emoticons>();
                    foreach (var item in lst)
                    {
                        objActivity_GetTogether_Emoticons.coUniqueID = objActivities.coUniqueID;
                        objActivity_GetTogether_Emoticons.proUniqueID = objActivities.proCompany;
                        objActivity_GetTogether_Emoticons.emoticonUniqueID = "em " + item.emoticonUniqueID;
                        objActivity_GetTogether_Emoticons.proStatus = item.proStatus;

                        sqlCon = DataObjectFactory.CreateNewConnection();
                        sqlCmd = new SqlCommand("sp_Insert_Activity_GetTogether_Emoticons", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.AddWithValue("@coUniqueID", objActivity_GetTogether_Emoticons.coUniqueID);
                        sqlCmd.Parameters.AddWithValue("@proUniqueID", objActivity_GetTogether_Emoticons.proUniqueID);
                        sqlCmd.Parameters.AddWithValue("@emoticonUniqueID", objActivity_GetTogether_Emoticons.emoticonUniqueID);
                        sqlCmd.Parameters.AddWithValue("@proStatus", objActivity_GetTogether_Emoticons.proStatus);


                        DataObjectFactory.OpenConnection(sqlCon);
                        int E = sqlCmd.ExecuteNonQuery();
                        DataObjectFactory.CloseConnection(sqlCon);
                    }

                    #endregion

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
        public IHttpActionResult JoinActivity(Vortex_Activity_Employee objVortexActivity)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            List<string> date = new List<string>();
            int i = 0;
            try
            {
                if (!string.IsNullOrEmpty(objVortexActivity.RecurringDates))
                {
                    if (objVortexActivity.RecurringDates.Contains(","))
                    {
                        date = objVortexActivity.RecurringDates.Split(',').ToList();

                        foreach (var item in date)
                        {
                            sqlCon = DataObjectFactory.CreateNewConnection();
                            sqlCmd = new SqlCommand("sp_InsertVortex_Activity_Employee", sqlCon);
                            sqlCmd.CommandType = CommandType.StoredProcedure;

                            sqlCmd.Parameters.AddWithValue("@coUniqueID", objVortexActivity.coUniqueID);
                            sqlCmd.Parameters.AddWithValue("@proUniqueID", objVortexActivity.proUniqueID);
                            sqlCmd.Parameters.AddWithValue("@volUniqueID", objVortexActivity.volUniqueID);
                            sqlCmd.Parameters.AddWithValue("@volTransport", objVortexActivity.volTransport);
                            sqlCmd.Parameters.AddWithValue("@volDiet", objVortexActivity.volDiet);
                            sqlCmd.Parameters.AddWithValue("@proStatus", objVortexActivity.proStatus);
                            sqlCmd.Parameters.AddWithValue("@proChosenDate", objVortexActivity.proChosenDate);
                            sqlCmd.Parameters.AddWithValue("@proVolHourDates", Convert.ToDateTime(item).ToString("yyyy-MM-dd"));

                            DataObjectFactory.OpenConnection(sqlCon);
                            i = sqlCmd.ExecuteNonQuery();
                            DataObjectFactory.CloseConnection(sqlCon);
                        }

                    }
                }
                else
                {
                    sqlCon = DataObjectFactory.CreateNewConnection();
                    sqlCmd = new SqlCommand("sp_InsertVortex_Activity_Employee", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@coUniqueID", objVortexActivity.coUniqueID);
                    sqlCmd.Parameters.AddWithValue("@proUniqueID", objVortexActivity.proUniqueID);
                    sqlCmd.Parameters.AddWithValue("@volUniqueID", objVortexActivity.volUniqueID);
                    sqlCmd.Parameters.AddWithValue("@volTransport", objVortexActivity.volTransport);
                    sqlCmd.Parameters.AddWithValue("@volDiet", objVortexActivity.volDiet);
                    sqlCmd.Parameters.AddWithValue("@proStatus", objVortexActivity.proStatus);
                    sqlCmd.Parameters.AddWithValue("@proChosenDate", objVortexActivity.proChosenDate);
                    sqlCmd.Parameters.AddWithValue("@proVolHourDates", objVortexActivity.proVolHourDates);

                    DataObjectFactory.OpenConnection(sqlCon);
                    i = sqlCmd.ExecuteNonQuery();
                    DataObjectFactory.CloseConnection(sqlCon);
                }


                if (i > 0)
                {
                    //sqlCmd = new SqlCommand("Insert_Vortex_Activity_Employee_Hours", sqlCon);
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

        [Route("deletejoinActitvity")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult DeleteJoinActivity(Vortex_Activity_Employee objVortexActivity)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("sp_Update_Vortex_Activity_Employee", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@coUniqueID", objVortexActivity.coUniqueID);
                sqlCmd.Parameters.AddWithValue("@proUniqueID", objVortexActivity.proUniqueID);
                sqlCmd.Parameters.AddWithValue("@volUniqueID", objVortexActivity.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@proStatus", objVortexActivity.proStatus);
                sqlCmd.Parameters.AddWithValue("@proVolHourDates", objVortexActivity.proVolHourDates);


                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

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
                Activities obj = null;
                List<Activities> lstActivities = new List<Activities>();
                List<Activities> PastlstActivities = new List<Activities>();

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_SelectMyActitvity", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueID", volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new Activities();
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
                    obj.Member = Convert.ToString(sqlRed["Member"]) + " " + "Joined";

                    lstActivities.Add(obj);
                }
                sqlRed.NextResult();
                while (sqlRed.Read())
                {
                    obj = new Activities();
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
                    obj.Member = Convert.ToString(sqlRed["Member"]) + " " + "Joined";

                    PastlstActivities.Add(obj);
                }

                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                lstActivities = lstActivities.DistinctBy(a => a.volUniqueID).ToList();
                PastlstActivities = PastlstActivities.DistinctBy(a => a.volUniqueID).ToList();
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

        [Route("getActivitiesEmp/{proUniqueID}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetMemberInActitvity(string proUniqueID)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                tbl_Volunteers obj = null;
                List<tbl_Volunteers> lstVolunteers = new List<tbl_Volunteers>();

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_ListActitviiyEmployee", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@proUniqueID", proUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new tbl_Volunteers();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.volFirstName = Convert.ToString(sqlRed["volFirstName"]);
                    obj.volLastName = Convert.ToString(sqlRed["volLastName"]);
                    obj.volEmail = Convert.ToString(sqlRed["volEmail"]);
                    obj.volPicture = Convert.ToString(sqlRed["volPicture"]);
                    lstVolunteers.Add(obj);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lstVolunteers;

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
