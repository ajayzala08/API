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
    public class CriticalRequirementController : ApiController
    {
        ATS2019_dbEntities db = new ATS2019_dbEntities();
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel GetCriticalRequirement()
        {
            ResponseModel responseModel = new ResponseModel();
            DateTime comDate = System.DateTime.Now.AddDays(-3);
            var listRequirement = (from c in db.Requirement_Master where c.J_Status == "Active" select c).ToList();
            var jidlist = "";
            foreach (var req in listRequirement)
            {
                var lastResumeUploaded = db.Resume_Master.Where(c => c.R_Jid == req.J_Id).OrderByDescending(c => c.R_CreateOn).FirstOrDefault();
                if (lastResumeUploaded != null)
                {
                    if (lastResumeUploaded.R_CreateOn != null)
                    {
                        if (lastResumeUploaded.R_CreateOn <= comDate)
                        {
                            if (jidlist == "")
                            {
                                jidlist += lastResumeUploaded.R_Jid.ToString();
                            }
                            else
                            {
                                jidlist += "," + lastResumeUploaded.R_Jid.ToString();
                            }
                        }
                    }
                }
            }

            //var reqList1 = context.Requirement_Master.Where(c => jidlist.Contains(c.J_Id.ToString())).ToList().OrderByDescending(m => c.J_CreatedOn);
            var reqList = (from c in db.Requirement_Master
                           join d in db.Client_Master on c.J_Client_Id equals d.C_id
                           where jidlist.Contains(c.J_Id.ToString())
                           select new { c, d }).ToList().OrderByDescending(m => m.c.J_CreatedOn);


            responseModel.Message = "Data Found";
            responseModel.Status = true;
            responseModel.Data = reqList;

            return responseModel;

        }
       

    }
}
