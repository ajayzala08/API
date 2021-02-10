﻿using ATSAPI.Models;
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
    public class SubmissionCountYesterdayController : ApiController
    {
        ATS2019_dbEntities db = new ATS2019_dbEntities();
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel PostCountSubmission(CountModel countModel)
        {
            ResponseModel responseModel = new ResponseModel();
            DateTime yesterday = System.DateTime.Now.AddDays(-1);
            int tysubcnt = 0;
            string val = "";
            if ((countModel.userrole=="Admin") || (countModel.userrole == "Manager"))
            {
                tysubcnt = db.Resume_Master.Where((c => EntityFunctions.TruncateTime(c.R_CreateOn) == EntityFunctions.TruncateTime(yesterday) && (c.R_Status == "Accept"))).ToList().Count();
            }
            else if (countModel.userrole == "Sales")
            {
                tysubcnt = (from c in db.User_Master
                            where c.E_UserName == countModel.username
                            join
                                d in db.SalesClient_Master on c.E_Code equals d.SC_UID
                            join
                                e in db.Requirement_Master on d.SC_CID equals e.J_Client_Id
                            join
                                f in db.Resume_Master on e.J_Id equals f.R_Jid
                            where EntityFunctions.TruncateTime(f.R_CreateOn) == EntityFunctions.TruncateTime(yesterday) && f.R_Status == "Accept"
                            select new { c, d, e, f }).ToList().Count();
            }
            else if (countModel.userrole == "Team Lead")
            {
                string tlname = db.User_Master.Where(c => c.E_UserName == countModel.username).Select(c => c.E_Fullname).FirstOrDefault();

                string temlist = db.Team_Master.Where(c => c.T_TeamLead == tlname).Select(c => c.T_TeamMember).FirstOrDefault();

                string[] tm = temlist.Split(',');
                foreach (var item in tm)
                {
                    tysubcnt += (from c in db.User_Master
                                 where c.E_Fullname == item
                                 join
                                     d in db.Resume_Master on c.E_UserName equals d.R_CreateBy
                                 where EntityFunctions.TruncateTime(d.R_CreateOn) == EntityFunctions.TruncateTime(yesterday) && d.R_Status == "Accept"
                                 select new { c, d }
                              ).Count();
                }
                // return Json(new { tysubcnt }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                tysubcnt = (from c in db.Resume_Master where c.R_CreateBy == countModel.username && EntityFunctions.TruncateTime(c.R_CreateOn) == EntityFunctions.TruncateTime(yesterday) && c.R_Status == "Accept" select c).Count();
                //context.Resume_Master.Where((c => c.R_CreateBy == User.Identity.Name && EntityFunctions.TruncateTime(c.R_CreateOn) == EntityFunctions.TruncateTime(System.DateTime.Now) && (c.R_Status == "Accept"))).ToList().Count();
            }
            responseModel.Message = "Count Found";
            responseModel.Data = tysubcnt;
            responseModel.Status = true;

            return responseModel;
        }
    }
}
