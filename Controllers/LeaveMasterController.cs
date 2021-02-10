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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LeaveMasterController : ApiController
    {
        public ResponseModel PostLeaveMaster(LeaveModel leaveModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var userroles = (from c in db.User_Master
                                     where c.E_UserName == leaveModel.createby
                                     join
                                            d in db.Role_Master on c.E_Code equals d.R_ecode
                                     select d.R_role).ToList();

                    Leave_Master master = new Leave_Master();
                    master.L_Type = leaveModel.type;
                    master.L_Noofdays = leaveModel.noofdays;
                    master.L_Inwords = leaveModel.inwords;
                    master.L_Reason = leaveModel.reason;
                    master.L_StartDate = leaveModel.startdate;
                    master.L_EndDate = leaveModel.enddate;
                    master.L_CreateBy = leaveModel.createby;
                    master.L_CreateOn = System.DateTime.Now;
                    if (userroles.Contains("Team Lead"))
                    {
                        master.L_TL_Status = "Approve";
                        master.L_TL_Datetime = System.DateTime.Now;
                    }
                    else
                    {
                        master.L_TL_Status = "Pending";
                        master.L_TL_Datetime = System.DateTime.Now;
                    }

                    if (userroles.Contains("Manager"))
                    {
                        master.L_M_Status = "Approve";
                        master.L_M_Datetime = System.DateTime.Now;
                    }
                    else
                    {
                        master.L_M_Status = "Pending";
                        master.L_M_Datetime = System.DateTime.Now;
                    }
                    master.L_Admin_Status = "Pending";
                    master.L_Admin_Datetime = System.DateTime.Now;
                    db.Leave_Master.Add(master);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Successfully Apply For Leave";
                        responseModel.Data = null;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Apply Leave";
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
        public ResponseModel DeleteLeaveMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Leave_Master master = db.Leave_Master.Where(x => x.L_Id == id).FirstOrDefault();
                    if (master != null)
                    {
                        db.Entry(master).State = EntityState.Deleted;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Deleted Successfully";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Delete Record";
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

        public ResponseModel PutLeaveMaster(decimal leaveid, string username, string status)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Leave_Master master = db.Leave_Master.Where(x => x.L_Id == leaveid).FirstOrDefault();
                    if (master != null)
                    {
                        var userroles = (from c in db.User_Master
                                         where c.E_UserName == username
                                         join d in db.Role_Master on c.E_Code equals d.R_ecode
                                         select d.R_role
                                       ).ToList();
                        if (userroles.Contains("Team Lead") || userroles.Contains("Manager") || userroles.Contains("Admin"))
                        {
                            if (userroles.Contains("Team Lead"))
                            {
                                master.L_TL_Status = status;
                                master.L_TL_Datetime = System.DateTime.Now;
                            }
                            if (userroles.Contains("Manager"))
                            {
                                master.L_M_Status = status;
                                master.L_M_Datetime = System.DateTime.Now;
                            }
                            if (userroles.Contains("Admin"))
                            {
                                master.L_Admin_Status = status;
                                master.L_Admin_Datetime = System.DateTime.Now;
                            }

                            db.Entry(master).State = EntityState.Modified;
                            var result = db.SaveChanges();
                            if (result > 0)
                            {
                                responseModel.Message = "Status Changed Successfully.";
                                responseModel.Data = null;
                                responseModel.Status = true;
                            }
                            else
                            {
                                responseModel.Message = "Fail To Change Status";
                                responseModel.Data = null;
                                responseModel.Status = false;
                            }
                        }
                        else
                        {
                            responseModel.Message = "No Rights TO Change Status";
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

        public ResponseModel GetLeaveMaster(string username)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                
                using (var db= new ATS2019_dbEntities())
                {
                    List<LeaveModel> leaves;
                    var userrloes = (from c in db.User_Master
                                     where c.E_UserName == username
                                     join d in db.Role_Master on c.E_Code equals d.R_ecode
                                     select d.R_role
                                    ).ToList();
                    if (userrloes.Contains("Admin") || userrloes.Contains("Manager"))
                    {

                        leaves = (from c in db.Leave_Master
                                  join d in db.User_Master on c.L_CreateBy equals d.E_UserName
                                  select new { c, d }).Select(x => new LeaveModel
                                  {
                                      id = x.c.L_Id,
                                      createby = x.d.E_Fullname,
                                      type = x.c.L_Type,
                                      noofdays = (float)x.c.L_Noofdays,
                                      inwords = x.c.L_Inwords,
                                      reason = x.c.L_Reason,
                                      startdate = x.c.L_StartDate,
                                      enddate = x.c.L_EndDate,
                                      leadstatus = x.c.L_TL_Status,
                                      leaddate = (DateTime)x.c.L_TL_Datetime,
                                      managerstatus = x.c.L_M_Status,
                                      managerdate = (DateTime)x.c.L_M_Datetime,
                                      adminstatus = x.c.L_Admin_Status,
                                      admindate = (DateTime)x.c.L_Admin_Datetime
                                  }).ToList();
                        if (leaves.Count > 0)
                        {
                            responseModel.Message = "Record Found";
                            responseModel.Data = leaves;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Record Not Found";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                       
                    }
                    else if (userrloes.Contains("Team Lead"))
                    {
                         List<LeaveModel> leaves1 = new List<LeaveModel>();
                        var listmembers = (from c in db.User_Master
                                           where c.E_UserName == username
                                           join d in db.Team_Master on c.E_Fullname equals d.T_TeamLead
                                           select new { c, d }).FirstOrDefault().d.T_TeamMember.Split(',');
                       // var lists = listmembers.d.T_TeamMember.Split(',');

                        foreach (var member in listmembers)
                        {
                            var leavlist = (from c in db.User_Master
                                      where c.E_Fullname == member
                                      join d in db.Leave_Master on c.E_UserName equals d.L_CreateBy
                                      select new { c, d }
                                     ).ToList();
                            foreach (var item in leavlist)
                            {
                                leaves1.Add(new LeaveModel
                                {
                                    id=item.d.L_Id,
                                    createby = item.c.E_Fullname,
                                    type = item.d.L_Type,
                                    noofdays= (float)item.d.L_Noofdays,
                                    inwords= item.d.L_Inwords,
                                    reason= item.d.L_Reason,
                                    startdate = item.d.L_StartDate,
                                    enddate = item.d.L_EndDate,
                                    leadstatus= item.d.L_TL_Status,
                                    leaddate = (DateTime)item.d.L_TL_Datetime,
                                    managerstatus= item.d.L_M_Status,
                                    managerdate=(DateTime)item.d.L_M_Datetime,
                                    adminstatus= item.d.L_Admin_Status,
                                    admindate= (DateTime)item.d.L_Admin_Datetime
                                    
                                });
                            }
                        }
                        if (leaves1.Count > 0)
                        {
                            responseModel.Message = "Record Found";
                            responseModel.Data = leaves1;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Record Not Found";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                        
                    }
                    else
                    {
                        leaves = (from c in db.Leave_Master
                                  where c.L_CreateBy == username
                                  join d in db.User_Master on c.L_CreateBy equals d.E_UserName
                                  select new { c, d }
                                 ).Select(x => new LeaveModel
                                 {
                                     id=x.c.L_Id,
                                     createby = x.d.E_Fullname,
                                     type = x.c.L_Type,
                                     noofdays = (float)x.c.L_Noofdays,
                                     inwords = x.c.L_Inwords,
                                     reason = x.c.L_Reason,
                                     startdate = x.c.L_StartDate,
                                     enddate = x.c.L_EndDate,
                                     leadstatus = x.c.L_TL_Status,
                                     leaddate = (DateTime)x.c.L_TL_Datetime,
                                     managerstatus = x.c.L_M_Status,
                                     managerdate=(DateTime)x.c.L_M_Datetime,
                                     adminstatus = x.c.L_Admin_Status,
                                     admindate= (DateTime)x.c.L_Admin_Datetime

                                 }).ToList();
                        if (leaves.Count > 0)
                        {
                            responseModel.Message = "Record Found";
                            responseModel.Data = leaves;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Record Not Found";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }

                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Message = "Excpetion";
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }
        }
    }
}
