using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MonthReportController : ApiController
    {
        public ResponseModel GetMonthReport(string userName)
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
                            subcnt = context.Resume_Master.Where(c => c.R_CreateBy == item.E_UserName && c.R_CreateOn.Value.Month == System.DateTime.Now.Month && c.R_CreateOn.Value.Year == System.DateTime.Now.Year).ToList().Count();

                            intcnt = context.Interview_Master.Where(c => c.I_CreateBy == item.E_UserName && c.I_Date.Month == System.DateTime.Now.Month && c.I_Date.Year == System.DateTime.Now.Year && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                            offcnt = context.Offer_Master.Where(c => c.O_Recruiter == item.E_UserName && c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year).ToList().Count();

                            startcnt = context.Offer_Master.Where(c => c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year && c.O_Status == "Join" && c.O_Recruiter == item.E_UserName).ToList().Count();

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

                            subcnt = context.Resume_Master.Where(c => c.R_CreateBy == username && c.R_CreateOn.Value.Month == System.DateTime.Now.Month && c.R_CreateOn.Value.Year == System.DateTime.Now.Year).ToList().Count();

                            intcnt = context.Interview_Master.Where(c => c.I_CreateBy == username && c.I_Date.Month == System.DateTime.Now.Month && c.I_Date.Year == System.DateTime.Now.Year && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                            offcnt = context.Offer_Master.Where(c => c.O_Recruiter == username && c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year).ToList().Count();

                            startcnt = context.Offer_Master.Where(c => c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year && c.O_Status == "Join" && c.O_Recruiter == username).ToList().Count();

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
                        subcnt = context.Resume_Master.Where(c => c.R_CreateBy == userName && c.R_CreateOn.Value.Month == System.DateTime.Now.Month && c.R_CreateOn.Value.Year == System.DateTime.Now.Year).ToList().Count();

                        intcnt = context.Interview_Master.Where(c => c.I_CreateBy == userName && c.I_Date.Month == System.DateTime.Now.Month && c.I_Date.Year == System.DateTime.Now.Year && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                        offcnt = context.Offer_Master.Where(c => c.O_Recruiter == userName && c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year).ToList().Count();

                        startcnt = context.Offer_Master.Where(c => c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year && c.O_Status == "Join" && c.O_Recruiter == User.Identity.Name).ToList().Count();

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
