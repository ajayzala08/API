using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;
using System.Data.Entity.Core.Objects;

namespace ATSAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class AppraisalMasterController : ApiController
    {
        public ResponseModel Post(AppraisalModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    int expDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpiredDays"].ToString());
                    Appraisal_Master tbl = new Appraisal_Master();
                    tbl.empCode = model.empcode;
                    tbl.rm_empCode = model.rmempcode;
                    tbl.period = model.period;
                    tbl.createdon = System.DateTime.Now;
                    tbl.expiredon = System.DateTime.Now.AddDays(expDays);
                    tbl.emp_status = "Open";
                    db.Appraisal_Master.Add(tbl);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        response.Status = true;
                        response.Message = "Performance Appraisal Created";
                        response.Data = null;
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "Fail To Create Performance Appraisal.";
                        response.Data = null;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Excption";
                response.Data = null;
                return response;
            }
        }

        public ResponseModel Get(decimal empCode)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    DateTime cDate = System.DateTime.Now;
                    var appraisalModel = (from c in db.Appraisal_Master where c.empCode == empCode && c.emp_status == "Open" && EntityFunctions.TruncateTime(c.expiredon) >= EntityFunctions.TruncateTime(cDate)
                                          join d in db.User_Master on c.empCode equals d.E_Code
                                          join e in db.User_Master on c.rm_empCode equals e.E_Code
                                          select new {c,d,e }
                                          ).FirstOrDefault();
                    if (appraisalModel != null)
                    {
                        //var appraisalModels = db.Appraisal_Master.Where(x => x.empCode == empCode && x.emp_status == "Open" && EntityFunctions.TruncateTime(x.expiredon) >= EntityFunctions.TruncateTime(cDate)).FirstOrDefault();
                        AppraisalViewModel model = new AppraisalViewModel();
                        model.paid = appraisalModel.c.paId;
                        model.empcode = appraisalModel.c.empCode;
                        model.empname = appraisalModel.d.E_Fullname;
                        model.rmempcode = appraisalModel.c.rm_empCode;
                        model.rmname = appraisalModel.e.E_Fullname;
                        model.period = appraisalModel.c.period;
                        model.createdOn = appraisalModel.c.createdon;
                        model.expiredOn = appraisalModel.c.expiredon;
                        if (model != null)
                        {
                            response.Status = true;
                            response.Message = "Record Found";
                            response.Data = model;
                        }
                        else
                        {
                            response.Status = false;
                            response.Message = "Record Not Found";
                            response.Data = null;
                        }
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "Record Not Found";
                        response.Data = null;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Exception";
                response.Data = null;
                return response;
            }
        }

        public ResponseModel get(decimal paid)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    empployeeReviewModel empreview = db.Employee_Review_Master.Where(x => x.paid == paid).Select(x => new empployeeReviewModel
                    {
                        paid = x.paid,
                        qualityofworkntaskcompletion = (int)x.qualityofworkntaskcompletion,
                        goalsntargetachievement = (int)x.goalsntargetachievement,
                        writtennverbalcommunicationskills = (int)x.writtennverbalcommunicationskills,
                        initiativenmotivation = (int)x.initiativenmotivation,
                        teamworknleadershipskills = (int)x.teamworknleadershipskills,
                        abilitytoproblemsolve = (int)x.abilitytoproblemsolve,
                        attendancenregualrization = (int)x.attendancenregualrization,
                        total = (int)x.total,
                        comment = x.comment

                    }).FirstOrDefault();

                    empployeeReviewModel rmreview = db.RM_Review_Master.Where(x => x.paid == paid).Select(x => new empployeeReviewModel
                    {
                        paid = x.paid,
                        qualityofworkntaskcompletion = (int)x.qualityofworkntaskcompletion,
                        goalsntargetachievement = (int)x.goalsntargetachievement,
                        writtennverbalcommunicationskills = (int)x.writtennverbalcommunicationskills,
                        initiativenmotivation = (int)x.initiativenmotivation,
                        teamworknleadershipskills = (int)x.teamworknleadershipskills,
                        abilitytoproblemsolve = (int)x.abilitytoproblemsolve,
                        attendancenregualrization = (int)x.attendancenregualrization,
                        total = (int)x.total,
                        comment = x.comment

                    }).FirstOrDefault();

                    var RMdata = (from c in db.Appraisal_Master
                                  where c.paId == paid && c.emp_status=="Close" && c.rm_status=="Close"
                                  join d in db.User_Master on c.empCode equals d.E_Code
                                  join e in db.User_Master on c.rm_empCode equals e.E_Code
                                  select new { c, d, e }
                                  ).FirstOrDefault();
                    if (RMdata != null)
                    {
                        RMViewModel rMViewModel = new RMViewModel();
                        rMViewModel.paid = RMdata.c.paId;
                        rMViewModel.empcode = RMdata.c.empCode;
                        rMViewModel.empname = RMdata.d.E_Fullname;
                        rMViewModel.rmempcode = RMdata.c.rm_empCode;
                        rMViewModel.rmname = RMdata.e.E_Fullname;
                        rMViewModel.createdOn = RMdata.c.createdon;
                        rMViewModel.expiredOn = RMdata.c.expiredon;
                        rMViewModel.period = RMdata.c.period;
                        rMViewModel.empployeereview = empreview;
                        rMViewModel.rmreview = rmreview;

                        if (rMViewModel != null)
                        {
                            response.Status = true;
                            response.Message = "Record Found";
                            response.Data = rMViewModel;
                        }
                        else
                        {
                            response.Status = false;
                            response.Message = "Record Not Found";
                            response.Data = null;
                        }
                    }
                    else {
                        response.Status = false;
                        response.Message = "Record Not Found";
                        response.Data = null;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Exception";
                response.Data = null;
                return response;
            }
        }
    }
}
