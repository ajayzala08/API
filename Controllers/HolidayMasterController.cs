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
    public class HolidayMasterController : ApiController
    {
        public ResponseModel GetHolidayMaster()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<HolidayModel> holidayModels = db.HolidayList_Master.Select(x=> new HolidayModel { 
                        id=x.Hid,
                        name= x.Hname,
                        date=x.Hdate,
                        day=x.Hday,
                        type=x.Htype,
                        username=x.CreatedBy
                    }).ToList();

                    if (holidayModels.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Data = holidayModels;
                        responseModel.Status = true;


                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
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

        public ResponseModel POSTHolidayMaster(HolidayModel model)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var exists = db.HolidayList_Master.Where(x=>x.Hname==model.name).FirstOrDefault();
                    if (exists == null)
                    {
                        HolidayList_Master master = new HolidayList_Master();
                        master.Hname = model.name;
                        master.Hdate = model.date;
                        master.Hday = model.day;
                        master.Htype = model.type;
                        master.CreatedBy = model.username;
                        master.CreatedOn = System.DateTime.Now;
                        db.HolidayList_Master.Add(master);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Holiday Added Succssfully.";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail to Add Holiday";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Holiday Already Exists";
                        responseModel.Data = null;
                        responseModel.Status = false;
                    }
                }
                return responseModel;
            }
            catch (Exception ex)
            {
                return responseModel;
            }
        
        }

        public ResponseModel GetHolidayMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    HolidayModel model = db.HolidayList_Master.Where(x => x.Hid == id).Select(x => new HolidayModel
                    {
                        id= x.Hid,
                        name=x.Hname,
                        date=x.Hdate,
                        day=x.Hday,
                        type=x.Htype,
                        username= x.CreatedBy
                    }).FirstOrDefault();
                    if (model != null)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Data = model;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
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

        public ResponseModel PutHolidayMaster(HolidayModel model)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db =new ATS2019_dbEntities())
                {
                    var master = db.HolidayList_Master.Where(x => x.Hid == model.id).FirstOrDefault();
                    if (master != null)
                    {
                        master.Hname = model.name;
                        master.Hdate = model.date;
                        master.Hday = model.day;
                        master.Htype = model.type;
                        db.Entry(master).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Holiday Updated Successfully";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Update Holiday";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
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

        public ResponseModel DeleteHolidayMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    HolidayList_Master master = db.HolidayList_Master.Where(x => x.Hid == id).FirstOrDefault();
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

    }
}
