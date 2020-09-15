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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);
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
                        DefaultImage = coWhiteLabel + "avatar1.png" +","+ coWhiteLabel + "avatar2.png" + "," + coWhiteLabel + "avatar3.png" + "," + coWhiteLabel + "avatar4.png" + "," + coWhiteLabel + "avatar5.png" + "," + coWhiteLabel + "avatar6.png";
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
            try
            {
                var httpRequest = HttpContext.Current.Request;
                //Volunteers item = JsonConvert.DeserializeObject<Volunteers>(httpRequest.Params["Data"].ToString());
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string extension = System.IO.Path.GetExtension(postedFile.FileName);
                    if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                    {
                        sqlCon = DataObjectFactory.CreateNewConnection();

                        sqlCmd = new SqlCommand("sp_UpdateProfilePicture", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                        sqlCmd.Parameters.AddWithValue("@volPicture", postedFile.FileName);

                        DataObjectFactory.OpenConnection(sqlCon);
                        int i = sqlCmd.ExecuteNonQuery();
                        DataObjectFactory.CloseConnection(sqlCon);
                        if (i > 0)
                        {
                            objResponse.Flag = true;
                            objResponse.Message = Message.UpdateSuccessMessage;
                            objResponse.Data = null;
                            var filePath = HttpContext.Current.Server.MapPath(ProfilePath + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                        }
                        else
                        {
                            objResponse.Flag = true;
                            objResponse.Message = Message.ErrorMessage;
                            objResponse.Data = null;
                        }

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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_GetUserInterests", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                int count = 0;
                while (sqlRed.Read())
                {
                    if (count == 0)
                    {
                        Interests = Convert.ToString(sqlRed["volInterest"]);
                    }
                    else
                    {
                        Interests += "," + Convert.ToString(sqlRed["volInterest"]);
                    }
                    count++;
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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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
                int i = 0;
                int j = 0;
                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_Delete_Volunteer_Interests", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);

                DataObjectFactory.OpenConnection(sqlCon);
                i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);
                if (i > 0)
                {
                    foreach (var item in Interests)
                    {
                        sqlCon = DataObjectFactory.CreateNewConnection();

                        sqlCmd = new SqlCommand("sp_AddVolunteer_Interests", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                        sqlCmd.Parameters.AddWithValue("@volInterests", item);

                        DataObjectFactory.OpenConnection(sqlCon);
                        j = sqlCmd.ExecuteNonQuery();
                        DataObjectFactory.CloseConnection(sqlCon);
                    }
                }


                if (j > 0)
                {
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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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

            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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

            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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

            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);
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
        [BasicAuthentication]
        public IHttpActionResult ClaimProfile(string volUserName)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            try
            {
                tbl_Volunteers obj = null;

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("claimProfile", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUserName", volUserName);

                DataObjectFactory.OpenConnection(sqlCon);
                sqlRed = sqlCmd.ExecuteReader();
                while (sqlRed.Read())
                {
                    obj = new tbl_Volunteers();
                    obj.id = Convert.ToInt32(sqlRed["id"]);
                    obj.coUniqueID = Convert.ToString(sqlRed["coUniqueID"]);
                    obj.volUniqueID = Convert.ToString(sqlRed["volUniqueID"]);
                    obj.volFirstName = Convert.ToString(sqlRed["volFirstName"]);
                    obj.volLastName = Convert.ToString(sqlRed["volLastName"]);
                    obj.volGender = Convert.ToString(sqlRed["volGender"]);
                    obj.volUserName = Convert.ToString(sqlRed["volUserName"]);
                    obj.VolUserPassword = Convert.ToString(sqlRed["VolUserPassword"]);
                    obj.volEmail = Convert.ToString(sqlRed["volEmail"]);
                    obj.volOnBoardStatus = Convert.ToString(sqlRed["volOnBoardStatus"]);
                    if (sqlRed["volOnBoardDateSent"] is DBNull)
                        obj.volOnBoardDateSent = null;
                    else
                        obj.volOnBoardDateSent = Convert.ToDateTime(sqlRed["volOnBoardDateSent"]);
                    obj.volPicture = Convert.ToString(sqlRed["volPicture"]);
                    obj.volEducation = Convert.ToString(sqlRed["volEducation"]);
                    obj.volCompetencies = Convert.ToString(sqlRed["volCompetencies"]);
                    obj.volCategories = Convert.ToString(sqlRed["volCategories"]);
                    obj.volPhone = Convert.ToString(sqlRed["volPhone"]);
                    obj.volStatus = Convert.ToString(sqlRed["volStatus"]);
                    obj.restricted = Convert.ToString(sqlRed["restricted"]);
                    obj.reviewStatus = Convert.ToString(sqlRed["reviewStatus"]);
                    if (sqlRed["reviewDate"] is DBNull)
                        obj.reviewDate = null;
                    else
                        obj.reviewDate = Convert.ToDateTime(sqlRed["reviewDate"]);
                    obj.volDepartment = Convert.ToString(sqlRed["volDepartment"]);
                    obj.volLanguage = Convert.ToString(sqlRed["volLanguage"]);
                    obj.volAbout = Convert.ToString(sqlRed["volAbout"]);
                    obj.volInterests = Convert.ToString(sqlRed["volInterests"]);
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
                return Ok(objResponse);
            }
            finally
            {
                DataObjectFactory.DisposeDataReader(sqlRed);
                DataObjectFactory.DisposeCommand(sqlCmd);
                DataObjectFactory.CloseConnection(sqlCon);
            }
        }

        [Route("ForgetPassword/{volUniqueID}/{volUserPassword}")]
        [HttpPost]
        [BasicAuthentication]
        public IHttpActionResult ForgetPassword(string volUniqueID, string volUserPassword)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataReader sqlRed = null;
            string FullName = string.Empty;
            string volEmail = string.Empty;
            try
            {
                sqlCon = DataObjectFactory.CreateNewConnection();
                sqlCmd = new SqlCommand("UpdatePassword", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@volUniqueID", volUniqueID);
                sqlCmd.Parameters.AddWithValue("@volUserPassword", volUserPassword);

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

                int isSent = SendPasswordResetRequestMail(FullName, volEmail, volUserPassword);

                if (isSent == 0)
                {
                    objResponse.Message = objResponse.Message + " But failed to send mail of password reset request";
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

        public int SendPasswordResetRequestMail(string FullName, string volEmail, string volUserPassword)
        {
            try
            {
                string strBody = string.Empty;
                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/EmailTemplates/PasswordResetRequestEmail.html")))
                {
                    strBody = reader.ReadToEnd();
                }

                strBody = strBody.Replace("##FullName##", FullName);
                strBody = strBody.Replace("##Password##", volUserPassword);
                var Subject = "atWork - Password Reset Request";
                var EmailCC = "";
                var result = CommonMethods.SendMail(volEmail, strBody, Subject, EmailCC);

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
                return 0;
            }
        }


    }
}
