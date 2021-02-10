using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InterviewMasterController : ApiController
    {
        public ResponseModel PostInterviewMaster(InterviewModel interviewModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var alreadyexists = db.Interview_Master.Where(x => x.I_RId == interviewModel.resumeid && x.I_JId == interviewModel.requirementid && EntityFunctions.TruncateTime(x.I_Date) == EntityFunctions.TruncateTime(interviewModel.interviewdate) && x.I_By == interviewModel.interviewby && x.I_Location == interviewModel.interviewlocation && x.I_CreateBy == interviewModel.username).FirstOrDefault();
                    if (alreadyexists == null)
                    {
                        Interview_Master master = new Interview_Master();
                        master.I_RId = interviewModel.resumeid;
                        master.I_JId = interviewModel.requirementid;
                        master.I_Date = interviewModel.interviewdate;
                        master.I_Time = interviewModel.interviewtime;
                        master.I_By = interviewModel.interviewby;
                        master.I_Location = interviewModel.interviewlocation;
                        master.I_Note = interviewModel.interviewnote;
                        master.I_Status = interviewModel.status;
                        master.I_CreateBy = interviewModel.username;
                        master.I_CreateOn = System.DateTime.Now;
                        db.Interview_Master.Add(master);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Interview Scheduled Successfully";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Schedule Interview";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Interview Already Scheduled";
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

        public ResponseModel GetInterviewMaster(decimal requirementid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<InterviewViewModel> interviewModels = (from c in db.Interview_Master
                                                                where c.I_JId == requirementid
                                                                join
                                                                    d in db.Resume_Master on c.I_RId equals d.R_Id
                                                                select new { c, d }).Select(x => new InterviewViewModel
                                                                {
                                                                    iid = x.c.I_Id,
                                                                    name = x.d.R_Name,
                                                                    idate = x.c.I_Date,
                                                                    itime = x.c.I_Time,
                                                                    by = x.c.I_By,
                                                                    location = x.c.I_Location,
                                                                    status = x.c.I_Status
                                                                }).ToList();
                    if (interviewModels.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Data = interviewModels;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
                        responseModel.Data = null;
                        responseModel.Status = false;
                    }

                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Data = null;
                responseModel.Status = false;
                return responseModel;
            }
        }

        public ResponseModel DeleteInterviewMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Interview_Master master = db.Interview_Master.Where(x => x.I_Id == id).FirstOrDefault();
                    if (master != null)
                    {
                        db.Entry(master).State = EntityState.Deleted;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Interview Deleted Successfully.";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Delete Interview";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
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

        //update inteview status only
        public ResponseModel PutInterviewMaster(decimal interviewid, string status)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Interview_Master master = db.Interview_Master.Where(x => x.I_Id == interviewid).FirstOrDefault();
                    if (master != null)
                    {
                        master.I_Status = status;
                        db.Entry(master).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            if (status == "Offer")
                            {
                                var candidatename = db.Resume_Master.Where(x => x.R_Id == master.I_RId).FirstOrDefault();
                                Boolean isofferadded = AddOffer(master.I_Id, master.I_RId, master.I_JId, candidatename.R_Name,candidatename.R_CreateBy);
                            }
                            responseModel.Message = "Interview Status Updated";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Update Status";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
                        responseModel.Data = null;
                        responseModel.Status = false;
                    }
                    return responseModel;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Data = null;
                responseModel.Status = false;
                return responseModel;
            }
        }

        protected Boolean AddOffer(decimal iid, decimal rid, decimal jid, string name, string recruiter)
        {
            using (var db = new ATS2019_dbEntities())
            {
                Offer_Master of = new Offer_Master();
                of.O_Iid = iid;
                of.O_Rid = rid;
                of.O_Jid = jid;
                of.O_Fullname = name.ToString();
                of.O_Status = "Offer";
                of.O_Recruiter = recruiter;
                of.O_Recuiter_Date = System.DateTime.Now;
                db.Offer_Master.Add(of);
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

    }
}
