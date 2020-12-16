using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_Placement_record_management.Models;
using AtWork_API.Filters;
using AtWork_API.Models;
//using Humanizer;

namespace AtWork_API.Controllers
{
    [RoutePrefix("api/notification")]
    public class NotificationController : ApiController
    {

        #region Save Settings

        #region Save Notification Settings
        [Route("SaveNotificationSetting")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult SaveNotification(Notification Model)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;

            var volUniqueID = Model.volUniqueId;
            try
            {
                //string token = string.Empty;
                //var re = Request;
                //var headers = re.Headers;
                //token = headers.GetValues("Authorization").First();

                //CommonMethods objCommonMethods = new CommonMethods();
                //var Volunteers = objCommonMethods.getCurentUser(token);
                ////volUniqueID = Volunteers.volUniqueID;

                //Connect_Notification_Setting CNS = new Connect_Notification_Setting();

                //  var ConvEndDateTime = Model.PauseNotificationStarttime.AddMinutes(Model.PauseTimeMinute);

                if (!Model.IsPaused)
                {
                    Model.PauseNotificationStarttime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    Model.PauseNotificationEndtime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    Model.PauseTime = "";
                }
                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_SaveNotificationSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Model.volUniqueId);
                sqlCmd.Parameters.AddWithValue("@IsPaused", Model.IsPaused);
                sqlCmd.Parameters.AddWithValue("@PauseTime", Model.PauseTime);
                sqlCmd.Parameters.AddWithValue("@IsForever", Model.IsForever);
                sqlCmd.Parameters.AddWithValue("@PauseNotificationStarttime", Model.PauseNotificationStarttime);
                sqlCmd.Parameters.AddWithValue("@PauseNotificationEndtime", Model.PauseNotificationEndtime);

                
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                DataObjectFactory.OpenConnection(sqlCon);
                da.SelectCommand = sqlCmd;
                //sqlCmd.ExecuteNonQuery();
                da.Fill(ds);
                sqlCmd.Parameters.Clear();
                DataObjectFactory.CloseConnection(sqlCon);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var currentRow = ds.Tables[0].Rows[0];
                    var res_Success = Convert.ToBoolean(currentRow["IsSuccess"].ToString());
                    var res_msg = currentRow["Message"].ToString();

                    if (res_Success)
                    {
                        Model.FormattedDate = FormateDate(Convert.ToDateTime(currentRow["PauseNotificationEndtime"]));

                        objResponse.Flag = true;
                        objResponse.Message = res_msg;
                        objResponse.Data = Model;

                    }
                    else
                    {
                        objResponse.Flag = false;
                        objResponse.Message = res_msg;
                        objResponse.Data = null;
                    }
                }




                //objResponse.Flag = true;
                //objResponse.Message = Message.UpdateSuccessMessage;
                //objResponse.Data = null;

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

        #endregion Save Notification Settings

        #region Save Connect Settings

        [Route("SaveConnectSetting")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult SaveConnectSetting(Connect_Notification_Setting Model)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;

            var volUniqueID = Model.volUniqueId;
            try
            {
                //string token = string.Empty;
                //var re = Request;
                //var headers = re.Headers;
                //token = headers.GetValues("Authorization").First();

                //  CommonMethods objCommonMethods = new CommonMethods();
                //var Volunteers = objCommonMethods.getCurentUser(token);
                //volUniqueID = Volunteers.volUniqueID;

                //Connect_Notification_Setting CNS = new Connect_Notification_Setting();


                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_SaveConnectSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Model.volUniqueId);

                sqlCmd.Parameters.AddWithValue("@Connect_IsPostFromCompany", Model.Connect_IsPostFromCompany);
                sqlCmd.Parameters.AddWithValue("@Connect_IsPostFromGroup", Model.Connect_IsPostFromGroup);
                sqlCmd.Parameters.AddWithValue("@Connect_IsPostFromEveryone", Model.Connect_IsPostFromEveryone);
                sqlCmd.Parameters.AddWithValue("@Connect_IsLikesOnPosts", Model.Connect_IsLikesOnPosts);
                sqlCmd.Parameters.AddWithValue("@Connect_IsLikesOnComments", Model.Connect_IsLikesOnComments);
                sqlCmd.Parameters.AddWithValue("@Connect_IsCommentsOnPosts", Model.Connect_IsCommentsOnPosts);
                sqlCmd.Parameters.AddWithValue("@Connect_IsPostsYouComment", Model.Connect_IsPostsYouComment);



                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                DataObjectFactory.OpenConnection(sqlCon);
                da.SelectCommand = sqlCmd;
                //sqlCmd.ExecuteNonQuery();
                da.Fill(ds);
                sqlCmd.Parameters.Clear();
                DataObjectFactory.CloseConnection(sqlCon);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var currentRow = ds.Tables[0].Rows[0];
                    var res_Success = Convert.ToBoolean(currentRow["IsSuccess"].ToString());
                    var res_msg = currentRow["Message"].ToString();
                    if (res_Success)
                    {

                        objResponse.Flag = true;
                        objResponse.Message = res_msg;
                        objResponse.Data = null;

                    }
                    else
                    {
                        objResponse.Flag = false;
                        objResponse.Message = res_msg;
                        objResponse.Data = null;
                    }
                }

                //objResponse.Flag = true;
                //objResponse.Message = Message.UpdateSuccessMessage;
                //objResponse.Data = null;

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

        #endregion Save Connect Settings

        #region Save Active Settings

        [Route("SaveActiveSetting")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult SaveActiveSetting(Activity_Notification_Setting Model)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;

            var volUniqueID = Model.volUniqueId;
            try
            {
                //string token = string.Empty;
                //var re = Request;
                //var headers = re.Headers;
                //token = headers.GetValues("Authorization").First();

                //CommonMethods objCommonMethods = new CommonMethods();
                //var Volunteers = objCommonMethods.getCurentUser(token);
                //volUniqueID = Volunteers.volUniqueID;

                //Connect_Notification_Setting CNS = new Connect_Notification_Setting();


                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_SaveActiveSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Model.volUniqueId);

                sqlCmd.Parameters.AddWithValue("@Active_IsYGTSomeoneRegister", Model.Active_IsYGTSomeoneRegister);
                sqlCmd.Parameters.AddWithValue("@Active_IsYGTSomeoneCancelled", Model.Active_IsYGTSomeoneCancelled);
                sqlCmd.Parameters.AddWithValue("@Active_IsAllActActivityCancelled", Model.Active_IsAllActActivityCancelled);
                sqlCmd.Parameters.AddWithValue("@Active_IsAllActActivityReminder", Model.Active_IsAllActActivityReminder);
                sqlCmd.Parameters.AddWithValue("@Active_IsAllActFeedbackReminder", Model.Active_IsAllActFeedbackReminder);
                sqlCmd.Parameters.AddWithValue("@Active_IsPetitionsStatusUpdate", Model.Active_IsPetitionsStatusUpdate);


                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                DataObjectFactory.OpenConnection(sqlCon);
                da.SelectCommand = sqlCmd;
                //sqlCmd.ExecuteNonQuery();
                da.Fill(ds);
                sqlCmd.Parameters.Clear();
                DataObjectFactory.CloseConnection(sqlCon);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var currentRow = ds.Tables[0].Rows[0];
                    var res_Success = Convert.ToBoolean(currentRow["IsSuccess"].ToString());
                    var res_msg = currentRow["Message"].ToString();
                    if (res_Success)
                    {

                        objResponse.Flag = true;
                        objResponse.Message = res_msg;
                        objResponse.Data = null;

                    }
                    else
                    {
                        objResponse.Flag = false;
                        objResponse.Message = res_msg;
                        objResponse.Data = null;
                    }
                }

                //objResponse.Flag = true;
                //objResponse.Message = Message.UpdateSuccessMessage;
                //objResponse.Data = null;

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

        #endregion Save Active Settings

        #endregion Save Settings

        #region Get Settings

        #region Get Notification Settings

        [Route("GetNotificationSetting/{volUniqueId}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetNotificationSetting(string volUniqueId)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                var IsPausedValue = false;
                sqlCon = DataObjectFactory.CreateNewConnection();

                List<Notification> lstNotifications = new List<Notification>();
                sqlCmd = new SqlCommand("sp_GetNotificationSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueId", volUniqueId);
                sqlCmd.Parameters.AddWithValue("@GetDateTime", DateTime.Now);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();

                while (sqlRed.Read())
                {
                    var obj = new Notification();
                    obj.volUniqueId = Convert.ToString(sqlRed["volUniqueId"]);
                    obj.IsPaused = Convert.ToBoolean(sqlRed["IsPaused"]);
                    obj.IsDisplayMsg = Convert.ToBoolean(sqlRed["IsDisplayMessage"]);
                    if (obj.IsPaused == false && obj.IsDisplayMsg == true)
                    {
                        IsPausedValue = true;
                    }

                    obj.PauseTime = Convert.ToString(sqlRed["PauseTime"]);
                    obj.IsForever = Convert.ToBoolean(sqlRed["IsForever"]);
                    if (obj.IsForever)
                    {
                        obj.PauseNotificationStarttime = DateTime.Now;
                        obj.PauseNotificationEndtime = DateTime.Now;
                        obj.FormattedDate = null;
                    }
                    else if (sqlRed["PauseNotificationStarttime"] != DBNull.Value && sqlRed["PauseNotificationEndtime"] != DBNull.Value)
                    {
                        obj.PauseNotificationStarttime = Convert.ToDateTime(sqlRed["PauseNotificationStarttime"]);
                        var DateFormat = obj.PauseNotificationEndtime = Convert.ToDateTime(sqlRed["PauseNotificationEndtime"]);

                        obj.FormattedDate = FormateDate(DateFormat);

                        //var Date_Day = Convert.ToString(DateFormat.Day);

                        //Int32 rem = DateFormat.Day % 100;
                        //if (rem >= 11 && rem <= 13)
                        //{
                        //    Date_Day += "th";
                        //}
                        //else
                        //{

                        //    switch (DateFormat.Day % 10)
                        //    {
                        //        case 1:
                        //            Date_Day += "st";
                        //            break;
                        //        case 2:
                        //            Date_Day += "nd";
                        //            break;
                        //        case 3:
                        //            Date_Day += "rd";
                        //            break;
                        //        default:
                        //            Date_Day += "th";
                        //            break;
                        //    }
                        //}

                        //var DateFormatMonth = DateFormat.ToString(string.Format("MMM "));
                        //var DateFormatTime = DateFormat.ToString(string.Format("HH:mm"));

                        //var DateFormatStr = string.Format(DateFormatMonth + Date_Day + ", " + DateFormatTime);
                        //obj.FormattedDate = "until " + DateFormatStr;

                    }
                    else
                    {

                        obj.PauseNotificationStarttime = DateTime.Now;
                        obj.PauseNotificationEndtime = DateTime.Now;
                        obj.FormattedDate = null;
                    }

                    lstNotifications.Add(obj);
                }

                sqlCmd.Parameters.Clear();

                if (IsPausedValue)
                {
                    sqlCmd = new SqlCommand("sp_UpdateDisplayNotificationSetting", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@volUniqueId", volUniqueId);

                    sqlCmd.ExecuteNonQuery();

                }

                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                if (lstNotifications.Count > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.GetData;
                    objResponse.Data = lstNotifications;
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = "No Record Found.";
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, "volUniqueId : " + volUniqueId);
                return BadRequest();
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        #endregion Get Notification Settings

        #region Get Notification Settings_V1

        [Route("GetNotificationSetting_V1")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult GetNotificationSetting_V1([FromBody] Notification Model)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                var IsPausedValue = false;
                sqlCon = DataObjectFactory.CreateNewConnection();

                List<Notification> lstNotifications = new List<Notification>();
                sqlCmd = new SqlCommand("sp_GetNotificationSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueId", Model.volUniqueId);
                sqlCmd.Parameters.AddWithValue("@GetDateTime", Model.PauseNotificationCurrentTime);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();

                while (sqlRed.Read())
                {
                    var obj = new Notification();
                    obj.volUniqueId = Convert.ToString(sqlRed["volUniqueId"]);
                    obj.IsPaused = Convert.ToBoolean(sqlRed["IsPaused"]);
                    obj.IsDisplayMsg = Convert.ToBoolean(sqlRed["IsDisplayMessage"]);
                    if (obj.IsPaused == false && obj.IsDisplayMsg == true)
                    {
                        IsPausedValue = true;
                    }

                    obj.PauseTime = Convert.ToString(sqlRed["PauseTime"]);
                    obj.IsForever = Convert.ToBoolean(sqlRed["IsForever"]);
                    if (obj.IsForever)
                    {
                        obj.PauseNotificationStarttime = DateTime.Now;
                        obj.PauseNotificationEndtime = DateTime.Now;
                        obj.FormattedDate = null;
                    }
                    else if (sqlRed["PauseNotificationStarttime"] != DBNull.Value && sqlRed["PauseNotificationEndtime"] != DBNull.Value)
                    {
                        obj.PauseNotificationStarttime = Convert.ToDateTime(sqlRed["PauseNotificationStarttime"]);
                        var DateFormat = obj.PauseNotificationEndtime = Convert.ToDateTime(sqlRed["PauseNotificationEndtime"]);

                        obj.FormattedDate = FormateDate(DateFormat);

                        //var Date_Day = Convert.ToString(DateFormat.Day);

                        //Int32 rem = DateFormat.Day % 100;
                        //if (rem >= 11 && rem <= 13)
                        //{
                        //    Date_Day += "th";
                        //}
                        //else
                        //{

                        //    switch (DateFormat.Day % 10)
                        //    {
                        //        case 1:
                        //            Date_Day += "st";
                        //            break;
                        //        case 2:
                        //            Date_Day += "nd";
                        //            break;
                        //        case 3:
                        //            Date_Day += "rd";
                        //            break;
                        //        default:
                        //            Date_Day += "th";
                        //            break;
                        //    }
                        //}

                        //var DateFormatMonth = DateFormat.ToString(string.Format("MMM "));
                        //var DateFormatTime = DateFormat.ToString(string.Format("HH:mm"));

                        //var DateFormatStr = string.Format(DateFormatMonth + Date_Day + ", " + DateFormatTime);
                        //obj.FormattedDate = "until " + DateFormatStr;

                    }
                    else
                    {

                        obj.PauseNotificationStarttime = DateTime.Now;
                        obj.PauseNotificationEndtime = DateTime.Now;
                        obj.FormattedDate = null;
                    }

                    lstNotifications.Add(obj);
                }

                sqlCmd.Parameters.Clear();

                if (IsPausedValue)
                {
                    sqlCmd = new SqlCommand("sp_UpdateDisplayNotificationSetting", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@volUniqueId", Model.volUniqueId);

                    sqlCmd.ExecuteNonQuery();

                }

                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                if (lstNotifications.Count > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.GetData;
                    objResponse.Data = lstNotifications;
                }
                else
                {
                    objResponse.Flag = true;
                    objResponse.Message = "No Record Found.";
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, "volUniqueId : " + Model.volUniqueId);
                return BadRequest();
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        #endregion Get Notification Settings_V1


        #region Get Connect Settings

        [Route("GetConnectSetting/{volUniqueId}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetConnectSetting(string volUniqueId)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {

                sqlCon = DataObjectFactory.CreateNewConnection();

                List<Connect_Notification_Setting> lstNotifications = new List<Connect_Notification_Setting>();
                sqlCmd = new SqlCommand("sp_GetConnectSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueId", volUniqueId);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    var obj = new Connect_Notification_Setting();
                    obj.volUniqueId = Convert.ToString(sqlRed["volUniqueId"]);
                    obj.Connect_IsPostFromCompany = Convert.ToBoolean(sqlRed["Connect_IsPostFromCompany"]);
                    obj.Connect_IsPostFromGroup = Convert.ToBoolean(sqlRed["Connect_IsPostFromGroup"]);
                    obj.Connect_IsPostFromEveryone = Convert.ToBoolean(sqlRed["Connect_IsPostFromEveryone"]);
                    obj.Connect_IsLikesOnPosts = Convert.ToBoolean(sqlRed["Connect_IsLikesOnPosts"]);
                    obj.Connect_IsLikesOnComments = Convert.ToBoolean(sqlRed["Connect_IsLikesOnComments"]);
                    obj.Connect_IsCommentsOnPosts = Convert.ToBoolean(sqlRed["Connect_IsCommentsOnPosts"]);
                    obj.Connect_IsPostsYouComment = Convert.ToBoolean(sqlRed["Connect_IsPostsYouComment"]);

                    lstNotifications.Add(obj);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                if (lstNotifications.Count > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.GetData;
                    objResponse.Data = lstNotifications;
                }
                else
                {
                    objResponse.Flag = false;
                    objResponse.Message = "No Record Found.";
                    objResponse.Data = null;
                }
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, "volUniqueId : " + volUniqueId);
                return BadRequest();
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        #endregion Get Connect Settings

        #region Get Active Settings

        [Route("GetActiveSetting/{volUniqueId}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetActiveSetting(string volUniqueId)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {

                sqlCon = DataObjectFactory.CreateNewConnection();

                List<Activity_Notification_Setting> lstNotifications = new List<Activity_Notification_Setting>();
                sqlCmd = new SqlCommand("sp_GetActiveSetting", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueId", volUniqueId);

                DataObjectFactory.OpenConnection(sqlCon);

                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    var obj = new Activity_Notification_Setting();
                    obj.volUniqueId = Convert.ToString(sqlRed["volUniqueId"]);
                    obj.Active_IsYGTSomeoneRegister = Convert.ToBoolean(sqlRed["Active_IsYGTSomeoneRegister"]);
                    obj.Active_IsYGTSomeoneCancelled = Convert.ToBoolean(sqlRed["Active_IsYGTSomeoneCancelled"]);
                    obj.Active_IsAllActActivityCancelled = Convert.ToBoolean(sqlRed["Active_IsAllActActivityCancelled"]);
                    obj.Active_IsAllActActivityReminder = Convert.ToBoolean(sqlRed["Active_IsAllActActivityReminder"]);
                    obj.Active_IsAllActFeedbackReminder = Convert.ToBoolean(sqlRed["Active_IsAllActFeedbackReminder"]);
                    obj.Active_IsPetitionsStatusUpdate = Convert.ToBoolean(sqlRed["Active_IsPetitionsStatusUpdate"]);
                    lstNotifications.Add(obj);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                if (lstNotifications.Count > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.GetData;
                    objResponse.Data = lstNotifications;
                }
                else
                {
                    objResponse.Flag = false;
                    objResponse.Message = "No Record Found.";
                    objResponse.Data = null;
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, "volUniqueId : " + volUniqueId);
                return BadRequest();
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        #endregion Get Active Settings

        #endregion Get Settings

        public string FormateDate(DateTime PauseNotificationEndtime)
        {
            string FinalDate;

            var DateFormat = PauseNotificationEndtime;

            var Date_Day = Convert.ToString(DateFormat.Day);

            Int32 rem = DateFormat.Day % 100;
            if (rem >= 11 && rem <= 13)
            {
                Date_Day += "th";
            }
            else
            {

                switch (DateFormat.Day % 10)
                {
                    case 1:
                        Date_Day += "st";
                        break;
                    case 2:
                        Date_Day += "nd";
                        break;
                    case 3:
                        Date_Day += "rd";
                        break;
                    default:
                        Date_Day += "th";
                        break;
                }
            }

            var DateFormatMonth = DateFormat.ToString(string.Format("MMM "));
            var DateFormatTime = DateFormat.ToString(string.Format("HH:mm"));

            var DateFormatStr = string.Format(DateFormatMonth + Date_Day + ", " + DateFormatTime);
            FinalDate = "until " + DateFormatStr;

            return FinalDate;
        }
    }


}
