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
    public class TobeJoinListController : ApiController
    {
        public ResponseModel GetTobeJoinListt()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var userrole = "Admin";

                using (var db = new ATS2019_dbEntities())
                {
                    List<ViewTobejoinModel> vtbjm = new List<ViewTobejoinModel>();
                    if (userrole =="Team Lead")
                    {

                        var leadname = (from c in db.User_Master where c.E_UserName == User.Identity.Name select c.E_Fullname).FirstOrDefault();
                        if (leadname != null)
                        {
                            var teammember = (from d in db.Team_Master where d.T_TeamLead == leadname select d.T_TeamMember).FirstOrDefault();
                            if (teammember != null)
                            {
                                string[] tmlist = teammember.ToString().Split(',');
                                foreach (var item in tmlist)
                                {
                                    var offerlist = (from e in db.User_Master
                                                     where e.E_Fullname == item
                                                     join
                                                         f in db.Offer_Master on e.E_UserName equals f.O_Recruiter
                                                     where f.O_Status == "To be join"
                                                     join
                                                         g in db.Resume_Master on f.O_Rid equals g.R_Id
                                                     join
                                                         h in db.Requirement_Master on f.O_Jid equals h.J_Id
                                                     join
                                                         j in db.Client_Master on h.J_Client_Id equals j.C_id

                                                     select new { e, f, g, h, j }).ToList();
                                    foreach (var item1 in offerlist)
                                    {
                                        vtbjm.Add(new ViewTobejoinModel
                                        {
                                            oid = item1.f.O_Id,
                                            name = item1.g.R_Name,
                                            client = item1.j.C_Name,
                                            seldate = Convert.ToDateTime(item1.f.O_Sel_Date),
                                            offdate = Convert.ToDateTime(item1.f.O_Off_Date),
                                            joindate = Convert.ToDateTime(item1.f.O_Join_Date),
                                            location = item1.h.J_Location,
                                            type = item1.h.J_Employment_Type,
                                            status = item1.f.O_Status,
                                            recruitername = item1.e.E_Fullname,
                                            skill = item1.h.J_Skill
                                        });
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        var offerlist = (from e in db.User_Master
                                         join
                                             f in db.Offer_Master on e.E_UserName equals f.O_Recruiter
                                         where f.O_Status == "To be join"
                                         join
                                             g in db.Resume_Master on f.O_Rid equals g.R_Id
                                         join
                                             h in db.Requirement_Master on f.O_Jid equals h.J_Id
                                         join
                                             j in db.Client_Master on h.J_Client_Id equals j.C_id

                                         select new { e, f, g, h, j }).ToList();
                        foreach (var item1 in offerlist)
                        {
                            vtbjm.Add(new ViewTobejoinModel
                            {
                                oid = item1.f.O_Id,
                                name = item1.g.R_Name,
                                client = item1.j.C_Name,
                                seldate = Convert.ToDateTime(item1.f.O_Sel_Date),
                                offdate = Convert.ToDateTime(item1.f.O_Off_Date),
                                joindate = Convert.ToDateTime(item1.f.O_Join_Date),
                                location = item1.h.J_Location,
                                type = item1.h.J_Employment_Type,
                                status = item1.f.O_Status,
                                recruitername = item1.e.E_Fullname,
                                skill = item1.h.J_Skill
                            });
                        }

                    }
                    responseModel.Message = "Data Found";
                    responseModel.Status = true;
                    responseModel.Data = vtbjm;
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
