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
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class ViewRequirementSummaryController : ApiController
    {
        
        public ResponseModel GET(decimal jid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {

                    int subcount = (from c in db.Resume_Master where c.R_Jid == jid && c.R_Status == "Accept" select c).ToList().Count();

                    int intcount = (from d in db.Interview_Master where d.I_JId == jid && (d.I_Status != "Set" || d.I_Status != "No Show") select d).ToList().Count();

                    int offcount = (from e in db.Offer_Master where e.O_Jid == jid select e).ToList().Count();

                    int stacount = (from f in db.Offer_Master where f.O_Jid == jid && f.O_Status == "Join" select f).ToList().Count();

                    int bdcount = (from g in db.Offer_Master where g.O_Jid == jid && g.O_Status == "BD" select g).ToList().Count();

                    var result = new { submission = subcount, interview = intcount, offer = offcount, start = stacount, bd = bdcount, requirementid = jid };

                    responseModel.Status = true;
                    responseModel.Message = "Data Found";
                    responseModel.Data = result;
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = "Exception";
                responseModel.Data = null;
                return responseModel;
            }
        }
    }
}
