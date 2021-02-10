using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RequirementProgressReportController : ApiController
    {
        public ResponseModel GetRequirementProgressReport()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                List<ClientRequirementSIOHReport_Model> sior = new List<ClientRequirementSIOHReport_Model>();
                using (var db = new ATS2019_dbEntities())
                {

                    var reqlist = (from c in db.Requirement_Master
                                   join
                                       d in db.Client_Master on c.J_Client_Id equals d.C_id
                                   select new { c, d }).ToList().OrderByDescending(m => m.c.J_CreatedOn);
                    foreach (var item in reqlist)
                    {
                        int subcnt = (from e in db.Resume_Master where e.R_Jid == item.c.J_Id && e.R_Status == "Accept" select e).Count();

                        int intcnt = (from f in db.Interview_Master where f.I_JId == item.c.J_Id && (f.I_Status != "Set" && f.I_Status != "Re Schedule" && f.I_Status != "No Show") select f).Count();

                        int offcnt = (from g in db.Offer_Master where g.O_Jid == item.c.J_Id select g).Count();

                        int hirecnt = (from h in db.Offer_Master where h.O_Jid == item.c.J_Id && h.O_Status == "Join" select h).Count();

                        int bdcnt = (from i in db.Offer_Master where i.O_Jid == item.c.J_Id && i.O_Status == "BD" select i).Count();
                        string createddate = DateTime.Parse(Convert.ToDateTime(item.c.J_CreatedOn).ToShortDateString()).ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                        sior.Add(new ClientRequirementSIOHReport_Model
                        {
                            client = item.d.C_Name,
                            skill = item.c.J_Skill,
                            jobcode = item.c.J_Code,
                            position = item.c.J_Position,
                            submission = subcnt,
                            interview = intcnt,
                            offer = offcnt,
                            hire = hirecnt,
                            bd = bdcnt,
                            type = item.c.J_Employment_Type,
                            status = item.c.J_Status,
                            location = item.c.J_Location,
                            createdon = createddate

                        });
                    }
                    
                }
                if (sior.Count > 0)
                {
                    responseModel.Message = "Data Found";
                    responseModel.Status = true;
                    responseModel.Data = sior;

                }
                else
                {
                    responseModel.Message = "Data Not Found";
                    responseModel.Status = false;
                    responseModel.Data = sior;
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
