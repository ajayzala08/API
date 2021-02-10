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
    public class MonthToBeJoinController : ApiController
    {
        public ResponseModel GetMonthToBeJoin()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    int month = 1; //System.DateTime.Now.Month;
                    int year = 2020; //System.DateTime.Now.Year;
                    List<MonthModel> offerlist = (from c in db.Offer_Master
                                                  where c.O_Off_Date.Value.Month == month && c.O_Off_Date.Value.Year == year && c.O_Status == "To be join"
                                                  join
                                                      d in db.Resume_Master on c.O_Rid equals d.R_Id
                                                  join
                                                      e in db.User_Master on c.O_Recruiter equals e.E_UserName
                                                  join
                                                      f in db.Requirement_Master on d.R_Jid equals f.J_Id
                                                  join
                                                  g in db.Client_Master on f.J_Client_Id equals g.C_id
                                                  select new { c, d, e, f, g }).Select(x => new MonthModel
                                                  {
                                                      candidatename = x.d.R_Name,
                                                      clientname = x.g.C_Name,
                                                      dates = (DateTime)x.c.O_Join_Date,
                                                      recruitername = x.e.E_Fullname


                                                  }).ToList();
                    if (offerlist.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = offerlist;
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
