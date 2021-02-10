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
    public class OfferListController : ApiController
    {
        public ResponseModel GetOfferList()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var userrole = "Admin";

                using (var db = new ATS2019_dbEntities())
                {
                    if (userrole == "Admin")
                    {
                        var offerdatalist = (from c in db.Offer_Master
                                             where c.O_Status == "Offer"
                                             join
                                                 d in db.Resume_Master on c.O_Rid equals d.R_Id
                                             join
                                                 e in db.Requirement_Master on d.R_Jid equals e.J_Id
                                             join
                                                 f in db.Client_Master on e.J_Client_Id equals f.C_id
                                             join
                                                 g in db.User_Master on c.O_Recruiter equals g.E_UserName
                                             select new { c, d, e, f, g }).ToList();
                        responseModel.Message = "Data Found";
                        responseModel.Data = offerdatalist;
                        responseModel.Status = true;

                    }
                    else if (userrole == "Sales")
                    {
                        var offerdatalist = (from c in db.Offer_Master
                                             where c.O_Status == "Offer"
                                             join
                                                 d in db.Resume_Master on c.O_Rid equals d.R_Id
                                             join
                                                 e in db.Requirement_Master on d.R_Jid equals e.J_Id
                                             join
                                                 f in db.Client_Master on e.J_Client_Id equals f.C_id
                                             join
                                                 g in db.User_Master on c.O_Recruiter equals g.E_UserName
                                             join
                                                 h in db.SalesClient_Master on f.C_id equals h.SC_CID
                                             join
                                                 i in db.User_Master on h.SC_UID equals i.E_Code
                                             where i.E_UserName == User.Identity.Name

                                             select new { c, d, e, f, g, h, i }).ToList();
                        responseModel.Message = "Data Found";
                        responseModel.Data = offerdatalist;
                        responseModel.Status = true;
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