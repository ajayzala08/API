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
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class EmployeeReviewController : ApiController
    {
        public ResponseModel post(empployeeReviewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Employee_Review_Master tbl = new Employee_Review_Master();
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
                    db.Employee_Review_Master.Add(tbl);
                    var result =db.SaveChanges();
                    if (result > 0)
                    {
                        var appraisal = db.Appraisal_Master.Where(x => x.paId == model.paid).FirstOrDefault();
                        appraisal.emp_status = "Close";
                        appraisal.emp_date = System.DateTime.Now;
                        appraisal.rm_status = "Open";
                        db.Entry(appraisal).State = EntityState.Modified;
                        db.SaveChanges();
                        response.Data = null;
                        response.Status = true;
                        response.Message = "Record inserted successfully.";
                    }
                    else
                    {
                        response.Data = null;
                        response.Status = false;
                        response.Message = "Fail insert record";
                    }
                }
                return response;
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Exception";
                response.Data = null;
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
                                                      where c.empCode == empcode && EntityFunctions.TruncateTime(c.expiredon) >= EntityFunctions.TruncateTime(cDate) 
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

    }
}
