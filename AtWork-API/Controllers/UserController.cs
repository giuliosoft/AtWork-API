using API_Placement_record_management.Models;
using AtWork_API.Filters;
using AtWork_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace AtWork_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getprofile/{volUniqueID}")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetProfile(string volUniqueID)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                Volunteers obj = null;

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("GetProfile", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new Volunteers();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.coUniqueID = Convert.ToString(sqlRed["coUniqueID"]);
                    obj.volUniqueID = Convert.ToString(sqlRed["volUniqueID"]);
                    obj.volFirstName = Convert.ToString(sqlRed["volFirstName"]);
                    obj.volLastName = Convert.ToString(sqlRed["volLastName"]);
                    obj.volDepartment = Convert.ToString(sqlRed["volDepartment"]);
                    obj.volAbout = Convert.ToString(sqlRed["volAbout"]);
                    obj.volInterests = Convert.ToString(sqlRed["volInterests"]);
                    obj.volHours = Convert.ToString(sqlRed["volHours"]);
                    if (string.IsNullOrEmpty(obj.volHours))
                    {
                        obj.volHours = "0";
                    }
                    obj.Vortex_Activity_Count = Convert.ToInt32(sqlRed["Vortex_Activity_Count"]);
                    obj.volPicture = Convert.ToString(sqlRed["volPicture"]);
                }
                sqlRed.NextResult();
                int i = 0;
                while (sqlRed.Read())
                {
                    if (i == 0)
                    {
                        obj.classes = Convert.ToString(sqlRed["classUniqueID"]) + ":" + Convert.ToString(sqlRed["classValue"]);
                    }
                    else
                    {
                        obj.classes = obj.classes + "," + Convert.ToString(sqlRed["classUniqueID"]) + ":" + Convert.ToString(sqlRed["classValue"]);
                    }
                    i++;
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = obj;//.DistinctBy(a => a.id);

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

        [Route("GetProfilePicture")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetProfilePicture()
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string ProfileName = string.Empty;
            string DefaultImage = string.Empty;
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
                var company = db.tbl_Companies.Where(a => a.coUniqueID == Volunteers.coUniqueID).ToList();
                if (company != null)
                {
                    string coWhiteLabelPicStatus = company.FirstOrDefault().coWhiteLabelPicStatus;
                    string coWhiteLabel = company.FirstOrDefault().coWhiteLabel;

                    if (coWhiteLabelPicStatus.ToLower() == "no")
                    {
                        DefaultImage = "avatar1.png,avatar2.png,avatar3.png,avatar4.png,avatar5.png,avatar6.png";
                    }
                    else if (coWhiteLabelPicStatus.ToLower() == "yes")
                    {
                        DefaultImage = coWhiteLabel + "avatar1.png" + "," + coWhiteLabel + "avatar2.png" + "," + coWhiteLabel + "avatar3.png" + "," + coWhiteLabel + "avatar4.png" + "," + coWhiteLabel + "avatar5.png" + "," + coWhiteLabel + "avatar6.png";
                    }
                }
                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_GetProfilePicture", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    ProfileName = Convert.ToString(sqlRed["volPicture"]);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = ProfileName;
                objResponse.Data1 = DefaultImage;

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

        [Route("UpdateProfilePicture")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult UpdateProfilePicture()
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string ProfilePath = "~/volunteers/";
            string ProfileImageName = string.Empty;
            string volUniqueID = string.Empty;

            try
            {
                var httpRequest = HttpContext.Current.Request;
                Volunteers item = JsonConvert.DeserializeObject<Volunteers>(httpRequest.Params["Data"].ToString());
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();
                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);
                volUniqueID = Volunteers.volUniqueID;

                if (!string.IsNullOrEmpty(item.volDefaultPicture))
                {
                    ProfileImageName = item.volDefaultPicture;
                }
                else
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        string extension = System.IO.Path.GetExtension(postedFile.FileName);
                        if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png") || extension.ToLower().Contains("heic"))
                        {
                            //ProfileImageName = postedFile.FileName;
                            ProfileImageName = volUniqueID + extension;
                            //var filePath = HttpContext.Current.Server.MapPath(ProfilePath + postedFile.FileName);
                            var filePath = HttpContext.Current.Server.MapPath(ProfilePath + ProfileImageName);
                            postedFile.SaveAs(filePath);
                        }
                    }
                }

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_UpdateProfilePicture", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@volPicture", ProfileImageName);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);
                if (i > 0)
                {
                    objResponse.Flag = true;
                    objResponse.Message = Message.UpdateSuccessMessage;
                    objResponse.Data = ProfileImageName;

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
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("GetInterests")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetInterests()
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string Interests = string.Empty;
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

                sqlCmd = new SqlCommand("sp_GetUserInterests", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                int count = 0;
                while (sqlRed.Read())
                {
                    Interests = Convert.ToString(sqlRed["volInterests"]);
                    //if (count == 0)
                    //{
                    //    Interests = Convert.ToString(sqlRed["volInterest"]);
                    //}
                    //else
                    //{
                    //    Interests += "," + Convert.ToString(sqlRed["volInterest"]);
                    //}
                    //count++;

                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = Interests;

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

        [Route("UpdateInterests")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult UpdateInterests([FromBody] tbl_Volunteers tblVolunteers)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            List<string> Interests = new List<string>();
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

                sqlCmd = new SqlCommand("sp_UpdateUserInterests", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@volInterests", tblVolunteers.volInterests);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                if (!string.IsNullOrEmpty(tblVolunteers.volInterests))
                {
                    if (tblVolunteers.volInterests.Contains(","))
                    {
                        Interests = tblVolunteers.volInterests.Split(',').ToList();
                    }
                    else
                    {
                        Interests.Add(tblVolunteers.volInterests);
                    }
                }

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_Delete_Volunteer_Interests", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                foreach (var item in Interests)
                {
                    sqlCon = DataObjectFactory.CreateNewConnection();

                    sqlCmd = new SqlCommand("sp_AddVolunteer_Interests", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                    sqlCmd.Parameters.AddWithValue("@volInterests", item);

                    DataObjectFactory.OpenConnection(sqlCon);
                    sqlCmd.ExecuteNonQuery();
                    DataObjectFactory.CloseConnection(sqlCon);
                }

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_UpdateBoardStatus", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.UpdateSuccessMessage;
                objResponse.Data = null;

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

        [Route("Getabout")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult Getabout()
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string volAbout = string.Empty;
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

                sqlCmd = new SqlCommand("sp_GetUserabout", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    volAbout = Convert.ToString(sqlRed["volAbout"]);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = volAbout;

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

        [Route("Updateabout")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult Updateabout([FromBody] tbl_Volunteers tblVolunteers)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
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

                sqlCmd = new SqlCommand("sp_UpdateUserabout", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@volAbout", tblVolunteers.volAbout);

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

        [Route("GetLanguage")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult GetLanguage()
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string volLanguage = string.Empty;
            List<string> lstLanguage = new List<string>();
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

                sqlCmd = new SqlCommand("sp_GetUserLanguage", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    volLanguage = Convert.ToString(sqlRed["volLanguage"]);
                }
                sqlRed.NextResult();
                while (sqlRed.Read())
                {
                    lstLanguage.Add(Convert.ToString(sqlRed["language"]));
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = lstLanguage;
                objResponse.Data1 = volLanguage;

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

        [Route("UpdateLanguage")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult UpdateLanguage([FromBody] tbl_Volunteers tblVolunteers)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
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

                sqlCmd = new SqlCommand("sp_UpdateUserLanguage", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@volLanguage", tblVolunteers.volLanguage);

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

        [Route("getPassword")]
        [HttpGet]
        [BasicAuthentication]
        public IHttpActionResult getPassword()
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string VolUserPassword = string.Empty;
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

                sqlCmd = new SqlCommand("sp_GetUserPassword", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    VolUserPassword = Convert.ToString(sqlRed["VolUserPassword"]);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = VolUserPassword;

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

        [Route("UpdatePassword")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult UpdatePassword([FromBody] Volunteers Volunteer)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
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
                if (!string.IsNullOrEmpty(Volunteer.oldPassword) && Volunteers.VolUserPassword != Volunteer.oldPassword)
                {
                    objResponse.Flag = false;
                    objResponse.Message = "Password not match";
                    objResponse.Data = null;
                    return Ok(objResponse);
                }
                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_UpdateUserPassword", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@VolUserPassword", Volunteer.VolUserPassword);

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

        [Route("claimprofile/{volUserName}")]
        [HttpGet]
        //[BasicAuthentication]
        public IHttpActionResult ClaimProfile(string volUserName)
        {
            CommonResponse objResponse = new CommonResponse();
            string volUniqueID = string.Empty;
            try
            {
                var Volunteers = db.tbl_Volunteers.FirstOrDefault(u => u.volUserName == volUserName);
                if (Volunteers != null)
                {
                    volUniqueID = Volunteers.volUniqueID;
                    var CompanyInfo = db.tbl_Companies.FirstOrDefault(a => a.coUniqueID == Volunteers.coUniqueID);

                    objResponse.Flag = true;
                    objResponse.Message = Message.GetData;
                    objResponse.Data = CompanyInfo;
                    objResponse.Data1 = Volunteers;
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

        [Route("forgotpassword/{volUserName}")]
        [HttpGet]
        public IHttpActionResult ForgotPassword(string volUserName)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataReader sqlRed = null;
            string FullName = string.Empty;
            string volEmail = string.Empty;
            string volUniqueID = string.Empty;
            try
            {
                var user = db.tbl_Volunteers.FirstOrDefault(a => a.volUserName == volUserName);
                if (user == null)
                {
                    user = db.tbl_Volunteers.FirstOrDefault(a => a.volEmail == volUserName);
                }
                if (user != null)
                {
                    volUniqueID = user.volUniqueID;
                    string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                    string numbers = "1234567890";

                    string characters = numbers;
                    characters += Convert.ToString(alphabets + small_alphabets) + numbers;

                    int length = int.Parse("9");
                    string otp = string.Empty;
                    for (int i = 0; i < length - 1; i++)
                    {
                        string character = string.Empty;
                        do
                        {
                            int index = new Random().Next(0, characters.Length);
                            character = characters.ToCharArray()[index].ToString();
                        } while (otp.IndexOf(character) != -1);
                        otp += character;
                    }

                    var RandomPwd = otp;

                    sqlCon = DataObjectFactory.CreateNewConnection();
                    sqlCmd = new SqlCommand("UpdatePassword", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("@volUniqueID", user.volUniqueID);
                    sqlCmd.Parameters.AddWithValue("@volUserPassword", RandomPwd);

                    DataObjectFactory.OpenConnection(sqlCon);
                    sqlRed = sqlCmd.ExecuteReader();

                    while (sqlRed.Read())
                    {
                        FullName = Convert.ToString(sqlRed["FullName"]);
                        volEmail = Convert.ToString(sqlRed["volEmail"]);
                    }
                    sqlRed.Close();
                    DataObjectFactory.CloseConnection(sqlCon);

                    objResponse.Flag = true;
                    objResponse.Message = Message.UpdateSuccessMessage;
                    objResponse.Data = null;

                    if (!string.IsNullOrEmpty(volEmail))
                    {
                        int isSent = SendPasswordResetRequestMail(FullName, volEmail, RandomPwd);
                        if (isSent == 0)
                        {
                            objResponse.Message = objResponse.Message + " But failed to send mail of password reset request";
                        }
                        else
                        {
                            objResponse.Message = objResponse.Message + " Mail sent sucssesfully for password reset request";
                        }
                    }
                }
                else
                {
                    objResponse.Flag = false;
                    objResponse.Message = "User not found";
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
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        //[Route("submitcorrection/{volUniqueID}/{newName}/{newSurName}/{newEmail}")]
        [Route("submitcorrection")]
        [HttpPost]
        [BasicAuthentication]
        //public IHttpActionResult SubmitCorrection(string volUniqueID, string newName, string newSurName, string newEmail)
        public IHttpActionResult SubmitCorrection([FromBody] SubmitCorrection obj)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataReader sqlRed = null;
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("GetCoEmailByVolUniqueID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueID", obj.volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();

                string volFullName = string.Empty;
                string coEmail = string.Empty;
                while (sqlRed.Read())
                {
                    volFullName = Convert.ToString(sqlRed["volFullName"]);
                    coEmail = Convert.ToString(sqlRed["coEmail"]);
                }
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);
                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = null;

                if (!string.IsNullOrEmpty(coEmail))
                {
                    int isSent = SendSubmitCorrectionMail(obj.newName, obj.newSurName, obj.newEmail, volFullName, coEmail);

                    if (isSent == 0)
                    {
                        objResponse.Message = objResponse.Message + " But failed to send mail of correction request";
                    }
                    else
                    {
                        objResponse.Message = objResponse.Message + " Mail sent sucssesfully for correction request";
                    }
                }

                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                objResponse.Flag = false;
                objResponse.Message = Message.ErrorMessage;
                objResponse.Data = null;
                CommonMethods.SaveError(ex, obj.volUniqueID);
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        public int SendPasswordResetRequestMail(string FullName, string volEmail, string RandomPwd)
        {
            try
            {
                string strBody = string.Empty;
                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/EmailTemplates/PasswordResetRequestEmail.html")))
                {
                    strBody = reader.ReadToEnd();
                }

                strBody = strBody.Replace("##FullName##", FullName);
                strBody = strBody.Replace("##Password##", RandomPwd);
                var Subject = "AtWork - Password Reset Request";
                var EmailCC = "";
                var result = CommonMethods.SendMail(volEmail, strBody, Subject, EmailCC, true);

                if (result == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, string.Empty);
                return 0;
            }
        }
        public int SendSubmitCorrectionMail(string newName, string newSurName, string newEmail, string volFullName, string coEmail)
        {
            try
            {
                string strBody = string.Empty;
                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/EmailTemplates/SubmitCorrectionEmail.html")))
                {
                    strBody = reader.ReadToEnd();
                }

                strBody = strBody.Replace("##volFullName##", volFullName);
                strBody = strBody.Replace("##Name##", newName);
                strBody = strBody.Replace("##SurName##", newSurName);
                strBody = strBody.Replace("##Email##", newEmail);
                var Subject = "AtWork - Correction Request from " + volFullName;
                var EmailCC = "";
                var result = CommonMethods.SendMail(coEmail, strBody, Subject, EmailCC, false);

                if (result == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                CommonMethods.SaveError(ex, string.Empty);
                return 0;
            }
        }
    }
}
