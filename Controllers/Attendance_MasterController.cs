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
using Newtonsoft.Json;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Attendance_MasterController : ApiController
    {
        public ResponseModel PostAttendance(AttendanceMasterModel attendanceMasterModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {

                    var finddata = (from c in db.Attendance_Master where EntityFunctions.TruncateTime(c.A_InTime) == EntityFunctions.TruncateTime(System.DateTime.Now) && c.A_Name == attendanceMasterModel.username select c).FirstOrDefault();
                    //var finddata = context.Attendance_Master.Where(c => EntityFunctions.TruncateTime(Convert.ToDateTime(c.A_InTime)) == EntityFunctions.TruncateTime(System.DateTime.Now) && c.A_Name==User.Identity.Name).FirstOrDefault();
                    if (finddata != null)
                    {
                        TimeSpan td = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(finddata.A_InTime);
                        string totaltime = td.Hours.ToString() + ":" + td.Minutes.ToString();
                        finddata.A_OutTime = System.DateTime.Now;
                        finddata.A_OutIP = attendanceMasterModel.userip;
                        finddata.A_Time = totaltime;
                        db.Entry(finddata).State = EntityState.Modified;
                        db.SaveChanges();
                        responseModel.Message = "OutTime";
                        responseModel.Status = true;
                        responseModel.Data = null;

                    }
                    else
                    {
                        Attendance_Master am = new Attendance_Master();
                        am.A_Name = attendanceMasterModel.username;
                        am.A_InTime = System.DateTime.Now;
                        am.A_InIP = attendanceMasterModel.userip;
                        db.Attendance_Master.Add(am);
                        db.SaveChanges();
                        responseModel.Message = "InTime";
                        responseModel.Status = true;
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

        public ResponseModel GETCheckAttendance(string username)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {

                using (var db = new ATS2019_dbEntities())
                {
                    string un = JsonConvert.DeserializeObject<string>(username);
                    var findattenance = (from c in db.Attendance_Master where c.A_Name == un && EntityFunctions.TruncateTime(c.A_InTime) == EntityFunctions.TruncateTime(System.DateTime.Now) select c).FirstOrDefault();
                    //var findattenance = context.Attendance_Master.Where(c => (c.A_Name == User.Identity.Name) && (EntityFunctions.TruncateTime(Convert.ToDateTime(c.A_InTime)) == EntityFunctions.TruncateTime(System.DateTime.Now))).FirstOrDefault();
                    if (findattenance == null)
                    {
                        responseModel.Message = "InTime";
                        responseModel.Status = true;
                        responseModel.Data = "Null";
                    }
                    else
                    {
                        findattenance = (from d in db.Attendance_Master where d.A_Name == un && EntityFunctions.TruncateTime(d.A_InTime) == EntityFunctions.TruncateTime(System.DateTime.Now) && EntityFunctions.TruncateTime(d.A_OutTime) == EntityFunctions.TruncateTime(System.DateTime.Now) select d).FirstOrDefault();
                        //findattenance = context.Attendance_Master.Where(c => (c.A_Name == User.Identity.Name) && (EntityFunctions.TruncateTime(c.A_InTime) == EntityFunctions.TruncateTime(System.DateTime.Now)) && (EntityFunctions.TruncateTime(c.A_OutTime)==EntityFunctions.TruncateTime(System.DateTime.Now))).FirstOrDefault();
                        if (findattenance == null)
                        {
                            responseModel.Message = "OutTime";
                            responseModel.Status = true;
                            responseModel.Data = "Null";
                        }
                        else
                        {
                            responseModel.Message = "Complete";
                            responseModel.Status = true;
                            responseModel.Data = "Null";
                        }

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

        public ResponseModel GetAttendance(string username,int month, int year)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var attendance = db.Attendance_Master.Where(c => c.A_Name == username && c.A_InTime.Value.Month == month && c.A_InTime.Value.Year == year).ToList();
                    if (attendance.Count > 0)
                    {
                        List<AttendanceModel> modelslist = new List<AttendanceModel>();
                        foreach (var item in attendance)
                        {
                            TimeSpan timespan = Convert.ToDateTime(item.A_OutTime) - Convert.ToDateTime(item.A_InTime);
                            modelslist.Add(new AttendanceModel
                            {
                                aid = item.A_Id,
                                aname = item.A_Name,
                                intime = item.A_InTime.ToString(),
                                inip = item.A_InIP,
                                outtime = item.A_OutTime.ToString(),
                                outip = item.A_OutIP,
                                atime = item.A_Time,
                                

                            }); ;
                        }
                        if (modelslist.Count > 0)
                        {
                            responseModel.Message = "Record Found";
                            responseModel.Status = true;
                            responseModel.Data = modelslist;
                        }
                        else
                        {
                            responseModel.Message = "Record Not Found";
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

        public ResponseModel GetAttendance()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var attendance = db.Attendance_Master.Where(c => c.A_InTime.Value.Month == System.DateTime.Now.Month && c.A_InTime.Value.Year == System.DateTime.Now.Year).ToList();
                    if (attendance.Count > 0)
                    {
                        List<AttendanceModel> modelslist = new List<AttendanceModel>();
                        foreach (var item in attendance)
                        {
                            TimeSpan timespan = Convert.ToDateTime(item.A_OutTime) - Convert.ToDateTime(item.A_InTime);
                            modelslist.Add(new AttendanceModel
                            {
                                aid = item.A_Id,
                                aname = item.A_Name,
                                intime = item.A_InTime.ToString(),
                                inip = item.A_InIP,
                                outtime = item.A_OutTime.ToString(),
                                outip = item.A_OutIP,
                                atime = item.A_Time,
                                

                            }); ;
                        }
                        if (modelslist.Count > 0)
                        {
                            responseModel.Message = "Record Found";
                            responseModel.Status = true;
                            responseModel.Data = modelslist;
                        }
                        else
                        {
                            responseModel.Message = "Record Not Found";
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
    }
}
