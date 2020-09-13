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
using System.Text;
using System.Web;
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
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

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
        public IHttpActionResult UpdateProfilePicture([FromBody] tbl_Volunteers objVolunteers)
        {
            CommonResponse objResponse = new CommonResponse();
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRed = null;
            string ProfilePath = "~/volunteers/";
            try
            {
                string token = string.Empty;
                var re = Request;
                var headers = re.Headers;
                token = headers.GetValues("Authorization").First();

                CommonMethods objCommonMethods = new CommonMethods();
                var Volunteers = objCommonMethods.getCurentUser(token);

                sqlCon = DataObjectFactory.CreateNewConnection();

                sqlCmd = new SqlCommand("sp_UpdateProfilePicture", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@volPicture", objVolunteers.volPicture);

                DataObjectFactory.OpenConnection(sqlCon);
                int i = sqlCmd.ExecuteNonQuery();
                DataObjectFactory.CloseConnection(sqlCon);
                if (i > 0)
                {
                    var httpRequest = HttpContext.Current.Request;
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        string extension = System.IO.Path.GetExtension(postedFile.FileName);
                        if (extension.ToLower().Contains("gif") || extension.ToLower().Contains("jpg") || extension.ToLower().Contains("jpeg") || extension.ToLower().Contains("png"))
                        {
                            var filePath = HttpContext.Current.Server.MapPath(ProfilePath + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                        }
                    }
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
                while (sqlRed.Read())
                {
                    Interests = Convert.ToString(sqlRed["volInterests"]);
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
                sqlRed.Close();
                DataObjectFactory.CloseConnection(sqlCon);

                objResponse.Flag = true;
                objResponse.Message = Message.GetData;
                objResponse.Data = volLanguage;

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
        public IHttpActionResult UpdatePassword([FromBody] tbl_Volunteers tblVolunteers)
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

                sqlCmd = new SqlCommand("sp_UpdateUserPassword", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@volUniqueID", Volunteers.volUniqueID);
                sqlCmd.Parameters.AddWithValue("@VolUserPassword", tblVolunteers.VolUserPassword);

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


    }
}
