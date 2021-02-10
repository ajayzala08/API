using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DatewiseTeamLeadCountController : ApiController
    {
        public ResponseModel GetDatewiseTeamLeadCount(string teamlead, string ssd, string eed)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                DateTime dd1 = Convert.ToDateTime(ssd);
                DateTime dd2 = Convert.ToDateTime(eed);
                List<SIOSModel> sioslist = new List<SIOSModel>();
                int subcnt = 0, intcnt = 0, offcnt = 0, startcnt = 0;
                using (var context = new ATS2019_dbEntities())
                {
                    var teamlist = (from d in context.Team_Master where d.T_TeamLead == teamlead select d.T_TeamMember).FirstOrDefault();


                    string[] members = teamlist.ToString().Split(',');
                    foreach (var item in members)
                    {
                        string username = context.User_Master.Where(c => c.E_Fullname == item).Select(c => c.E_UserName).First();

                        subcnt = context.Resume_Master.Where(c => (c.R_CreateBy == username) && (EntityFunctions.TruncateTime(c.R_CreateOn) >= EntityFunctions.TruncateTime(dd1)) && (EntityFunctions.TruncateTime(c.R_CreateOn) <= EntityFunctions.TruncateTime(dd2))).ToList().Count();

                        intcnt = context.Interview_Master.Where(c => (c.I_CreateBy == username) && (EntityFunctions.TruncateTime(c.I_Date) >= EntityFunctions.TruncateTime(dd1)) && (EntityFunctions.TruncateTime(c.I_Date) <= EntityFunctions.TruncateTime(dd2)) && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                        offcnt = context.Offer_Master.Where(c => c.O_Recruiter == username && (EntityFunctions.TruncateTime(c.O_Off_Date) >= EntityFunctions.TruncateTime(dd1)) && (EntityFunctions.TruncateTime(c.O_Off_Date) <= EntityFunctions.TruncateTime(dd2))).ToList().Count();

                        startcnt = context.Offer_Master.Where(c => (EntityFunctions.TruncateTime(c.O_Off_Date) >= EntityFunctions.TruncateTime(dd1)) && (EntityFunctions.TruncateTime(c.O_Off_Date) <= EntityFunctions.TruncateTime(dd2)) && c.O_Status == "Join" && c.O_Recruiter == username).ToList().Count();

                        sioslist.Add(new SIOSModel
                        {
                            name = item,
                            submission = subcnt,
                            interview = intcnt,
                            offer = offcnt,
                            start = startcnt
                        });
                    }
                    if (sioslist.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = sioslist;
                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
                        responseModel.Status = false;
                        responseModel.Data = null;
                    }

                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;

            }
        }
    }
}
