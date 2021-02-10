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
    public class TodayReportController : ApiController
    {
        public ResponseModel GetTodayReport(string userName)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var context = new ATS2019_dbEntities())
                {
                    List<SIOSModel> sioslist = new List<SIOSModel>();
                    int subcnt = 0, intcnt = 0, offcnt = 0, startcnt = 0;

                    var userroles = (from c in context.User_Master
                                     where c.E_UserName == userName
                                     join d in context.Role_Master on c.E_Code equals d.R_ecode
                                     select d.R_role).ToList();

                    if ((userroles.Contains("Admin")) || (userroles.Contains("Manager")))
                    {
                        var userlist = context.User_Master.Where(c => (c.E_Role == "Recruiter" || c.E_Role == "Teamlead") && c.E_Is_Delete == 0 && c.E_Is_Active == 1).ToList();
                        foreach (var item in userlist)
                        {
                            subcnt = context.Resume_Master.Where(c => c.R_CreateBy == item.E_UserName && (EntityFunctions.TruncateTime(c.R_CreateOn) == EntityFunctions.TruncateTime(System.DateTime.Now))).ToList().Count();

                            intcnt = context.Interview_Master.Where(c => c.I_CreateBy == item.E_UserName && (EntityFunctions.TruncateTime(c.I_Date) == EntityFunctions.TruncateTime(System.DateTime.Now)) && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                            offcnt = context.Offer_Master.Where(c => c.O_Recruiter == item.E_UserName && (EntityFunctions.TruncateTime(c.O_Off_Date) == EntityFunctions.TruncateTime(System.DateTime.Now))).ToList().Count();

                            startcnt = context.Offer_Master.Where(c => (EntityFunctions.TruncateTime(c.O_Join_Date) == EntityFunctions.TruncateTime(System.DateTime.Now)) && c.O_Status == "Join" && c.O_Recruiter == item.E_UserName).ToList().Count();

                            sioslist.Add(new SIOSModel
                            {
                                name = item.E_Fullname,
                                submission = subcnt,
                                interview = intcnt,
                                offer = offcnt,
                                start = startcnt
                            });
                        }
                    }
                    if (userroles.Contains("Team Lead"))
                    {
                        var teamlist = (from c in context.User_Master
                                        where c.E_UserName == userName
                                        join
                                            d in context.Team_Master on c.E_Fullname equals d.T_TeamLead
                                        select d.T_TeamMember).First();


                        string[] members = teamlist.ToString().Split(',');
                        foreach (var item in members)
                        {
                            string username = context.User_Master.Where(c => c.E_Fullname == item).Select(c => c.E_UserName).First();

                            subcnt = context.Resume_Master.Where(c => c.R_CreateBy == username && (EntityFunctions.TruncateTime(c.R_CreateOn) == EntityFunctions.TruncateTime(System.DateTime.Now))).ToList().Count();

                            intcnt = context.Interview_Master.Where(c => c.I_CreateBy == username && (EntityFunctions.TruncateTime(c.I_Date) == EntityFunctions.TruncateTime(System.DateTime.Now)) && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                            offcnt = context.Offer_Master.Where(c => c.O_Recruiter == username && (EntityFunctions.TruncateTime(c.O_Off_Date) == EntityFunctions.TruncateTime(System.DateTime.Now))).ToList().Count();

                            startcnt = context.Offer_Master.Where(c => (EntityFunctions.TruncateTime(c.O_Join_Date) == EntityFunctions.TruncateTime(System.DateTime.Now)) && c.O_Status == "Join" && c.O_Recruiter == username).ToList().Count();

                            sioslist.Add(new SIOSModel
                            {
                                name = item,
                                submission = subcnt,
                                interview = intcnt,
                                offer = offcnt,
                                start = startcnt
                            });
                        }



                    }
                    if (userroles.Contains("Recruiter"))
                    {
                        string fullname = context.User_Master.Where(c => c.E_UserName == userName).Select(c => c.E_Fullname).First();
                        subcnt = context.Resume_Master.Where(c => c.R_CreateBy == userName && (EntityFunctions.TruncateTime(c.R_CreateOn) == EntityFunctions.TruncateTime(System.DateTime.Now))).ToList().Count();

                        intcnt = context.Interview_Master.Where(c => c.I_CreateBy == userName && (EntityFunctions.TruncateTime(c.I_Date) == EntityFunctions.TruncateTime(System.DateTime.Now)) && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                        offcnt = context.Offer_Master.Where(c => c.O_Recruiter == userName && (EntityFunctions.TruncateTime(c.O_Off_Date) == EntityFunctions.TruncateTime(System.DateTime.Now))).ToList().Count();

                        startcnt = context.Offer_Master.Where(c => (EntityFunctions.TruncateTime(c.O_Join_Date) == EntityFunctions.TruncateTime(System.DateTime.Now)) && c.O_Status == "Join" && c.O_Recruiter == User.Identity.Name).ToList().Count();

                        sioslist.Add(new SIOSModel
                        {
                            name = fullname,
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
                    return responseModel;
                }
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
