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
    public class GraphMasterController : ApiController
    {
        public ResponseModel Get()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db =new ATS2019_dbEntities())
                {
                    List<GraphViewModel> graphs = new List<GraphViewModel>();

                    int mm = System.DateTime.Now.Month;
                    int yy = System.DateTime.Now.Year;
                    var userlist = db.User_Master.Where(c => (c.E_Role == "Recruiter" || c.E_Role == "Teamlead") && c.E_Is_Delete == 0 && c.E_Is_Active == 1).ToList();
                    foreach (var item in userlist)
                    {
                        //submission count
                        var subcnt = db.Resume_Master.Where(c => c.R_CreateBy == item.E_UserName && c.R_CreateOn.Value.Month == System.DateTime.Now.Month && c.R_CreateOn.Value.Year == System.DateTime.Now.Year).ToList().Count();

                        //interview count
                        var intcnt = db.Interview_Master.Where(c => c.I_CreateBy == item.E_UserName && c.I_Date.Month == System.DateTime.Now.Month && c.I_Date.Year == System.DateTime.Now.Year && (c.I_Status != "Set" && c.I_Status != "Re Schedule" && c.I_Status != "No Show")).ToList().Count();

                        //offer Count
                        var offcnt = db.Offer_Master.Where(c => c.O_Recruiter == item.E_UserName && c.O_Off_Date.Value.Month == System.DateTime.Now.Month && c.O_Off_Date.Value.Year == System.DateTime.Now.Year).ToList().Count();
                       
                        //Hire count
                        var startcnt = db.Offer_Master.Where(c => c.O_Join_Date.Value.Month == System.DateTime.Now.Month && c.O_Join_Date.Value.Year == System.DateTime.Now.Year && c.O_Status == "Join" && c.O_Recruiter == item.E_UserName).ToList().Count();
                        graphs.Add(new GraphViewModel
                        {
                            name = item.E_Fullname,
                            submission = subcnt,
                            interview = intcnt,
                            offer = offcnt,
                            start =startcnt
                        });
                    }
                    if (graphs.Count > 0)
                    {
                        responseModel.Status = true;
                        responseModel.Message = "Record Found";
                        responseModel.Data = graphs;
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Record Not Found";
                        responseModel.Data = null;
                    }
                }

                return responseModel;

            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Data = null;
                responseModel.Message = "Exception";
                return responseModel;
            }

        }
    }
}
