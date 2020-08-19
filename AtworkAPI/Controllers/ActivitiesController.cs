using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AtworkAPI.Token;
using AtworkAPI.Models;
using AtworkAPI.Helpers;
using System.Text;

namespace AtworkAPI.Controllers
{
    [RoutePrefix("api/activities")]
    public class ActivitiesController : ApiController
    {
        private ModelContext db = new ModelContext();

        [Route("getlist/{id}")]
        [HttpGet]
        public IHttpActionResult GetActivityFeed(string id)
        {
            string username = String.Empty;
            string password = String.Empty;
            try
            {
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        var list = from a in db.ActivityDateFullTypes
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
                                       a.proBackgroundImage
                                   };
                        //filter by company
                        list = list.Where(x => x.coUniqueID == id);
                        list = list.Where(x => x.proAddActivityDate >= DateTime.Today);
                        list = list.OrderBy(ord => ord.proAddActivityDate);
                        list = list.OrderBy(ord => ord.proAddActivity_StartTime);
                        return Ok(list);
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("getrow/{id}")]
        [HttpGet]
        public IHttpActionResult GetRow(string id)
        {
            string username = String.Empty;
            string password = String.Empty;
            try
            {
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        var row = db.Activities.FirstOrDefault(a => a.proUniqueID == id);
                        return Ok(row);
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("insertrow")]
        [HttpPost]
        public IHttpActionResult InsertRow([FromBody] Activity a)
        {
            string username = String.Empty;
            string password = String.Empty;
            try
            {
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        a.proUniqueID = $"vol{DateTime.UtcNow.ToString()}";
                        db.Activities.Add(a);
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("updaterow")]
        [HttpPost]
        public IHttpActionResult UpdateRow([FromBody] Activity a)
        {
            string username = String.Empty;
            string password = String.Empty;
            try
            {
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        Activity toDB = db.Activities.FirstOrDefault(x => x.proUniqueID == a.proUniqueID);
                        toDB.proTitle = a.proTitle;
                        toDB.proDescription = a.proDescription;
                        //toDB.proLocation = a.proLocation;
                        //toDB.proAddress1 = a.proAddress1;
                        //toDB.proAddress2 = a.proAddress2;
                        //toDB.proPostalCode = a.proPostalCode;
                        //toDB.proCity = a.proCity;
                        //toDB.proCountry = a.proCountry;
                        //toDB.proContinent = a.proContinent;
                        //toDB.proTimeCommitment = a.proTimeCommitment;
                        //toDB.proTimeCommitmentDecimal = a.proTimeCommitmentDecimal;
                        //toDB.proDatesConfirmed = a.proDatesConfirmed;
                        //toDB.proType = a.proType;
                        //toDB.proCategory = a.proCategory;
                        //toDB.proCategoryName = a.proCategoryName;
                        //toDB.proSubCategory = a.proSubCategory;
                        //toDB.proSubCategoryName = a.proSubCategoryName;
                        //toDB.proStatus = a.proStatus;
                        //toDB.proPartner = a.proPartner;
                        //toDB.proPartnerusername = a.proPartnerusername;
                        //toDB.proActivityLanguage = a.proActivityLanguage;
                        //toDB.proActivityLanguageID = a.proActivityLanguageID;
                        //toDB.proAudience = a.proAudience;
                        //toDB.proSpecialRequirements = a.proSpecialRequirements;
                        //toDB.proCostCoveredEmployee = a.proCostCoveredEmployee;
                        //toDB.proCostCoveredCompany = a.proCostCoveredCompany;
                        //toDB.proAddActivity_HoursCommitted = a.proAddActivity_HoursCommitted;
                        //toDB.proAddActivity_StartTime = a.proAddActivity_StartTime;
                        //toDB.proAddActivity_ParticipantsMinNumber = a.proAddActivity_ParticipantsMinNumber;
                        //toDB.proAddActivity_ParticipantsMaxNumber = a.proAddActivity_ParticipantsMaxNumber;
                        //toDB.proAddActivity_OrgName = a.proAddActivity_OrgName;
                        //toDB.proAddActivity_Website = a.proAddActivity_Website;
                        //toDB.proAddActivity_AdditionalInfo = a.proAddActivity_AdditionalInfo;
                        //toDB.proAddActivity_CoordinatorEmail = a.proAddActivity_CoordinatorEmail;
                        //toDB.proPublishedDate = a.proPublishedDate;
                        //toDB.proAddActivityDate = a.proAddActivityDate;
                        //toDB.proBackgroundImage = a.proBackgroundImage;
                        //toDB.proDeliveryMethod = a.proDeliveryMethod;
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("deleterow/{id}")]
        [HttpPost]
        public IHttpActionResult DeleteRow(string id)
        {
            string username = String.Empty;
            string password = String.Empty;
            try
            {
                string header = HttpHelper.GetHeader(Request, "Authorization");
                if (header != null && header.StartsWith("Basic"))
                {
                    string encodedHeader = header.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedHeader));

                    int separatorIndex = usernamePassword.IndexOf(":");
                    username = usernamePassword.Substring(0, separatorIndex);
                    password = usernamePassword.Substring(separatorIndex + 1);

                    //validate
                    var c = db.Volunteers.Count(u => u.volUserName == username && u.volUserPassword == password);
                    if (c == 1)
                    {
                        //logic
                        Activity a = db.Activities.FirstOrDefault(x => x.proUniqueID == id);
                        db.Activities.Remove(a);
                        db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}