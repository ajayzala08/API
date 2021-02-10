using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DailyTargetController : ApiController
    {
        public ResponseModel GetDailyTarget()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    int yes = 1, no = 0;
                    int dailytargetvalue = Convert.ToInt32(ConfigurationManager.AppSettings["DailyTarget"].ToString());
                    DateTime dt = System.DateTime.Now;
                    var findall = db.User_Master.Where(c => ((c.E_Role == "Teamlead") || (c.E_Role == "Recruiter")) && (c.E_Is_Active == yes) && (c.E_Is_Delete == no)).ToList();
                    List<DailyTargetModel> dailyTargetModels = new List<DailyTargetModel>();
                    foreach (var item in findall)
                    {
                        int achivedcnt = db.Resume_Master.Where(x => x.R_CreateBy == item.E_UserName && x.R_Status == "Accept" && EntityFunctions.TruncateTime(x.R_CreateOn) == EntityFunctions.TruncateTime(dt)).Count();

                        int closurecnt = db.Resume_Master.Where(x => x.R_CreateBy == item.E_UserName && x.R_Status == "New" && EntityFunctions.TruncateTime(x.R_CreateOn) == EntityFunctions.TruncateTime(dt)).Count();

                        dailyTargetModels.Add(new DailyTargetModel {
                            username = item.E_Fullname,
                            target = dailytargetvalue,
                            achived = achivedcnt,
                            closure = closurecnt
                        }) ;
                        
                    }
                    responseModel.Message = "Data Found";
                    responseModel.Data = dailyTargetModels;
                    responseModel.Status = true;

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
