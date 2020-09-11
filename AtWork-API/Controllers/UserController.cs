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
using System.Web.Http;

namespace AtWork_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
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
    }
}
