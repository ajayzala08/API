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
    public class TodaysInterviewController : ApiController
    {
        public ResponseModel GetTodaysInterview()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    DateTime dateTime = System.DateTime.Now;
                   // DateTime dateTime = System.DateTime.Now.AddDays(-399);// interview date 27-12-2019
                    List<TodaysInterviewModel> interviewlist = (from c in db.Interview_Master
                                         where EntityFunctions.TruncateTime(c.I_Date) == EntityFunctions.TruncateTime(dateTime)
                                         join
                                             d in db.Resume_Master on c.I_RId equals d.R_Id
                                         join e in db.User_Master on d.R_CreateBy equals e.E_UserName
                                         select new { c, d,e }).Select(x=> new TodaysInterviewModel { 
                                          Candidatename=x.d.R_Name,
                                          InterviewBy=x.c.I_By,
                                          Location=x.c.I_Location,
                                          Time=x.c.I_Time,
                                          ContactNo=x.d.R_Cnt,
                                          Recruiter =x.e.E_Fullname
                                          
                                         }).ToList();
                    if (interviewlist.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = interviewlist;
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

