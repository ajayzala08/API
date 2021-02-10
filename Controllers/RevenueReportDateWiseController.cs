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
    public class RevenueReportDateWiseController : ApiController
    {
        public ResponseModel GetRevenueReportDateWise(string username, string ssd, string eed)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                DateTime dd1 = Convert.ToDateTime(ssd);
                DateTime dd2 = Convert.ToDateTime(eed);

                using (var db = new ATS2019_dbEntities())
                {
                    var userroles = (from c in db.User_Master
                                     where c.E_UserName == username
                                     join d in db.Role_Master on c.E_Code equals d.R_ecode
                                     select d.R_role).ToList();

                    if (userroles.Contains("Admin"))
                    {
                        List<ViewRevenueReportModel> revnuedatalist = (from c in db.Offer_Master
                                                                       where c.O_Status == "Join" && EntityFunctions.TruncateTime(c.O_Join_Date) >= EntityFunctions.TruncateTime(dd1) && EntityFunctions.TruncateTime(c.O_Join_Date) <= EntityFunctions.TruncateTime(dd2)
                                                                       join
                                                                           d in db.Resume_Master on c.O_Rid equals d.R_Id
                                                                       join
                                                                           e in db.Requirement_Master on c.O_Jid equals e.J_Id
                                                                       join
                                                                           f in db.Client_Master on e.J_Client_Id equals f.C_id
                                                                       join
                                                                           g in db.User_Master on c.O_Recruiter equals g.E_UserName
                                                                       select new
                                                                       {
                                                                           c,
                                                                           d,
                                                                           e,
                                                                           f,
                                                                           g
                                                                       }).Select(x => new ViewRevenueReportModel
                                                                       {
                                                                           Name = x.d.R_Name,
                                                                           Client = x.f.C_Name,
                                                                           Location = x.e.J_Location,
                                                                           Skill = x.e.J_Skill,
                                                                           Type = x.e.J_Employment_Type,
                                                                           CTC = x.c.O_CTC != null ? (double)x.c.O_CTC : (double)0,
                                                                           BR = x.c.O_BR != null ? (double)x.c.O_BR :(double)0,
                                                                           PR = x.c.O_PR != null ? (double)x.c.O_PR : (double)0,
                                                                           GP = x.c.O_GP != null ? (double)x.c.O_GP : (double)0,
                                                                           GPM = x.c.O_GPM != null ? (double)x.c.O_GPM:(double)0,
                                                                           Recruiter = x.g.E_Fullname
                                                                       }).ToList();
                        if (revnuedatalist.Count > 0)
                        {
                            responseModel.Message = "Data Found";
                            responseModel.Status = true;
                            responseModel.Data = revnuedatalist;
                        }
                        else
                        {
                            responseModel.Message = "Data Not Found";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "User has no rights";
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
