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
    public class SalesMonthlyReportController : ApiController
    {
        public ResponseModel PostSalesMonthlyReport(SalesMonthlyReportModel _model)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesMonthlyReport_Master _master = new SalesMonthlyReport_Master();

                    if (_model.months != null && _model.years != null)
                    {
                        _master.SMR_Month = findmonth(_model.months);
                        _master.SMR_Year = _model.years;
                        _master.SMR_Client = _model.client;
                        _master.SMR_OpenPosition = _model.position;
                        _master.SMR_BusinessReceived = _model.business;
                        _master.SMR_Submission = _model.submission;
                        _master.SMR_Int_received = _model.intreceived;
                        _master.SMR_Feedback_Pending = _model.feedbackpending;
                        _master.SMR_Noshow = _model.noshow;
                        _master.SMR_Offer = _model.offer;
                        _master.SMR_BD = _model.bd;
                        _master.SMR_Join = _model.join;
                        _master.SMR_PassThrough = _model.passthrough;
                        _master.SMR_BulkDeal = _model.bulkdeal;
                        _master.SMR_POExtended = _model.poextend;
                        _master.SMR_AttritionSaved = _model.attrition;
                        _master.SMR_TotalRevenue = _model.totrevenue;
                        _master.SMR_Remark = _model.remark;
                        _master.CreatedBy = _model.createby;
                        _master.CreatedOn = System.DateTime.Now;
                        db.SalesMonthlyReport_Master.Add(_master);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Saved Successfully.";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Save Record.";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Fail To Save Record.";
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

        public ResponseModel GetSalesMonthlyReport()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<SalesMonthlyReportViewModel> viewmonthlist;
                    string userrole = "Admin",username="";
                    if (userrole =="Admin")
                    {
                         viewmonthlist = (from c in db.SalesMonthlyReport_Master
                                             join d in db.User_Master on c.CreatedBy equals d.E_UserName
                                             join e in db.Client_Master on c.SMR_Client equals e.C_id
                                             select new { c, d, e }).Select(x => new SalesMonthlyReportViewModel
                                             {
                                                 id= x.c.SMR_Id,
                                                 name = x.d.E_Fullname,
                                                 months = x.c.SMR_Month,
                                                 years = x.c.SMR_Year,
                                                 client = x.e.C_Name,
                                                 position = x.c.SMR_OpenPosition,
                                                 business = x.c.SMR_BusinessReceived,
                                                 submission = x.c.SMR_Submission,
                                                 intreceived= x.c.SMR_Int_received,
                                                 feedbackpending = x.c.SMR_Feedback_Pending,
                                                 noshow = x.c.SMR_Noshow,
                                                 offer = x.c.SMR_Offer,
                                                 bd = x.c.SMR_BD,
                                                 join = x.c.SMR_Join,
                                                 passthrough = x.c.SMR_PassThrough,
                                                 bulkdeal = x.c.SMR_BulkDeal,
                                                 poextend = x.c.SMR_POExtended,
                                                 attrition = x.c.SMR_AttritionSaved,
                                                 totrevenue = x.c.SMR_TotalRevenue,
                                                 remark = x.c.SMR_Remark
                                             }).ToList();
                       
                    }
                    else
                    {
                         viewmonthlist = (from c in db.SalesMonthlyReport_Master where c.CreatedBy == username 
                                             join d in db.User_Master on c.CreatedBy equals d.E_UserName 
                                             join e in db.Client_Master on c.SMR_Client equals e.C_id
                                             select new { c, d, e }).Select(x => new SalesMonthlyReportViewModel
                                             {
                                                 name = x.d.E_Fullname,
                                                 months = x.c.SMR_Month,
                                                 years = x.c.SMR_Year,
                                                 client = x.e.C_Name,
                                                 position = x.c.SMR_OpenPosition,
                                                 business = x.c.SMR_BusinessReceived,
                                                 submission = x.c.SMR_Submission,
                                                 intreceived = x.c.SMR_Int_received,
                                                 feedbackpending = x.c.SMR_Feedback_Pending,
                                                 noshow = x.c.SMR_Noshow,
                                                 offer = x.c.SMR_Offer,
                                                 bd = x.c.SMR_BD,
                                                 join = x.c.SMR_Join,
                                                 passthrough = x.c.SMR_PassThrough,
                                                 bulkdeal = x.c.SMR_BulkDeal,
                                                 poextend = x.c.SMR_POExtended,
                                                 attrition = x.c.SMR_AttritionSaved,
                                                 totrevenue = x.c.SMR_TotalRevenue,
                                                 remark = x.c.SMR_Remark
                                             }).ToList();

                    }
                    if (viewmonthlist.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = viewmonthlist;
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

        public ResponseModel DeleteSalesMonthlyReport(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesMonthlyReport_Master _master = db.SalesMonthlyReport_Master.Where(x => x.SMR_Id == id).FirstOrDefault();
                    if (_master != null)
                    {
                        db.Entry(_master).State = EntityState.Deleted;
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

        private string findmonth(string m)
        {
            string monthname = "";
            switch (m)
            {
                case "1":
                    monthname = "January";
                    break;
                case "2":
                    monthname = "February";
                    break;
                case "3":
                    monthname = "March";
                    break;
                case "4":
                    monthname = "April";
                    break;
                case "5":
                    monthname = "May";
                    break;
                case "6":
                    monthname = "June";
                    break;
                case "7":
                    monthname = "July";
                    break;
                case "8":
                    monthname = "August";
                    break;
                case "9":
                    monthname = "September";
                    break;
                case "10":
                    monthname = "October";
                    break;
                case "11":
                    monthname = "November";
                    break;
                case "12":
                    monthname = "December";
                    break;
            }
            return monthname;
        }
    }
}
