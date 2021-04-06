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
    [EnableCors(origins:"*",headers:"*", methods:"*")]
    public class RMAppraisalMasterController : ApiController
    {
        public ResponseModel Post(empployeeReviewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    RM_Review_Master tbl = new RM_Review_Master();
                    tbl.paid = model.paid;
                    tbl.qualityofworkntaskcompletion = model.qualityofworkntaskcompletion;
                    tbl.goalsntargetachievement = model.goalsntargetachievement;
                    tbl.writtennverbalcommunicationskills = model.writtennverbalcommunicationskills;
                    tbl.initiativenmotivation = model.initiativenmotivation;
                    tbl.teamworknleadershipskills = model.teamworknleadershipskills;
                    tbl.abilitytoproblemsolve = model.abilitytoproblemsolve;
                    tbl.attendancenregualrization = model.attendancenregualrization;
                    tbl.total = model.total;
                    tbl.comment = model.comment;
                    db.RM_Review_Master.Add(tbl);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        var rmData = db.Appraisal_Master.Where(x => x.paId == model.paid).FirstOrDefault();
                        if (rmData != null)
                        {
                            rmData.rm_status = "Close";
                            rmData.rm_date = System.DateTime.Now;
                            db.Entry(rmData).State = EntityState.Modified;
                            db.SaveChanges();
                            response.Data = null;
                            response.Message = "Record Inserted Successfully";
                            response.Status = true;
                        }
                        else
                        {
                            response.Data = null;
                            response.Message = "Fail To Insert Record";
                            response.Status = false;
                        }
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "Fail To Insert Record";
                        response.Status = false;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = "Exception";
                response.Status = false;
                return response;
            }
        }
        public ResponseModel get(decimal empcode)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    DateTime cDate = System.DateTime.Now;

                    List<AppraisalViewModel> model = (from c in db.Appraisal_Master
                                                      where c.rm_empCode == empcode && c.emp_status == "Close" && EntityFunctions.TruncateTime(c.expiredon) >= EntityFunctions.TruncateTime(cDate) && c.rm_status=="Open"
                                                      join d in db.User_Master on c.empCode equals d.E_Code
                                                      join e in db.User_Master on c.rm_empCode equals e.E_Code
                                                      select new { c, d, e }

                                              ).Select(x => new AppraisalViewModel
                                              {
                                                  paid = x.c.paId,
                                                  empcode = x.c.empCode,
                                                  empname = x.d.E_Fullname,
                                                  rmempcode = x.c.rm_empCode,
                                                  rmname = x.e.E_Fullname,
                                                  createdOn = x.c.createdon,
                                                  period = x.c.period,
                                                  expiredOn = x.c.expiredon
                                              }).ToList();

                    if (model.Count > 0)
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
                return response;
                }
                
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Exception";
                response.Data = null;
                return response;
            }
        }
        public ResponseModel Get(decimal paid)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    empployeeReviewModel model = db.Employee_Review_Master.Where(x => x.paid == paid).Select(x=> new empployeeReviewModel { 
                        paid= x.paid,
                        qualityofworkntaskcompletion =(int)x.qualityofworkntaskcompletion,
                        goalsntargetachievement = (int)x.goalsntargetachievement,
                        writtennverbalcommunicationskills =(int)x.writtennverbalcommunicationskills,
                        initiativenmotivation = (int)x.initiativenmotivation,
                        teamworknleadershipskills = (int)x.teamworknleadershipskills,
                        abilitytoproblemsolve=(int)x.abilitytoproblemsolve,
                        attendancenregualrization =(int)x.attendancenregualrization,
                        total =(int)x.total,
                        comment =x.comment
                       
                    }).FirstOrDefault();

                    var RMdata = (from c in db.Appraisal_Master
                                  where c.paId == paid
                                  join d in db.User_Master on c.empCode equals d.E_Code
                                  join e in db.User_Master on c.rm_empCode equals e.E_Code
                                  select new { c, d, e }
                                  ).FirstOrDefault();
                    RMViewModel rMViewModel = new RMViewModel();
                    rMViewModel.paid = RMdata.c.paId;
                    rMViewModel.empcode = RMdata.c.empCode;
                    rMViewModel.empname = RMdata.d.E_Fullname;
                    rMViewModel.rmempcode = RMdata.c.rm_empCode;
                    rMViewModel.rmname = RMdata.e.E_Fullname;
                    rMViewModel.createdOn = RMdata.c.createdon;
                    rMViewModel.expiredOn = RMdata.c.expiredon;
                    rMViewModel.period = RMdata.c.period;
                    rMViewModel.empployeereview = model;
                    rMViewModel.rmreview = null;

                                  
                    if (RMdata != null)
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
