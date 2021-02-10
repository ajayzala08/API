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
    public class ActivityMasterController : ApiController
    {
        public ResponseModel GetActivityMaster()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    
                    List<ActivityViewModel> activitylist = (from c in db.User_Activity_Master join d in db.User_Master on c.Username equals d.E_UserName orderby c.Id descending select new { c, d }).Take(100).Select(x=> new ActivityViewModel {
                        name = x.d.E_Fullname,
                        activity = x.c.Activity,
                        activitytime = x.c.ActivityOn.ToString()
                    }).ToList();
                    if (activitylist.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = activitylist;
                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
                        responseModel.Status = false;
                        responseModel.Data = null;
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

        public ResponseModel PostActivityMaster(ActivityViewModel activityViewModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {

                    User_Activity_Master uam = new User_Activity_Master();
                    uam.Username = activityViewModel.name;
                    uam.Activity = activityViewModel.activity;
                    uam.ActivityOn = System.DateTime.Now;
                    db.User_Activity_Master.Add(uam);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Activity Added.";
                        responseModel.Status = true;
                        responseModel.Data = null;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Add Activity.";
                        responseModel.Status = false;
                        responseModel.Data = null;
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
