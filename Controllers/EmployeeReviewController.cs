using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    }
}
