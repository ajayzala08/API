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
    public class SalesMonthlyRevenueReportController : ApiController
    {
        public ResponseModel PostSalesMonthlyRevenueReport(SalesClientWiseMonthlyRevenueModel salesClientWiseMonthlyRevenueModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClientwiseMonthlyRevenue_Master salesClientwiseMonthlyRevenue_Master = new SalesClientwiseMonthlyRevenue_Master();
                  //  int mm = Convert.ToDateTime(salesClientWiseMonthlyRevenueModel.srmonth).Month;
                    string monthname = findmonth(salesClientWiseMonthlyRevenueModel.srmonth);
                    string yy = salesClientWiseMonthlyRevenueModel.sryear;
                    salesClientwiseMonthlyRevenue_Master.SRR_Month = monthname;
                    salesClientwiseMonthlyRevenue_Master.SRR_Year = Convert.ToDecimal(yy);
                    salesClientwiseMonthlyRevenue_Master.SRR_Client = salesClientWiseMonthlyRevenueModel.srclient;
                    salesClientwiseMonthlyRevenue_Master.SRR_Current_HC = salesClientWiseMonthlyRevenueModel.srcurrenthc;
                    salesClientwiseMonthlyRevenue_Master.SRR_Total_GP = Convert.ToDecimal(salesClientWiseMonthlyRevenueModel.srtotgp);
                    salesClientwiseMonthlyRevenue_Master.SRR_Avg_GP_Added = Convert.ToDecimal(salesClientWiseMonthlyRevenueModel.sravggpadded);
                    salesClientwiseMonthlyRevenue_Master.SRR_Start = salesClientWiseMonthlyRevenueModel.srstart;
                    salesClientwiseMonthlyRevenue_Master.SRR_Net_Total_GP = Convert.ToDecimal(salesClientWiseMonthlyRevenueModel.srnettotgp);
                    salesClientwiseMonthlyRevenue_Master.SRR_Total_GP_Added = Convert.ToDecimal(salesClientWiseMonthlyRevenueModel.srnettotgpadded);
                    salesClientwiseMonthlyRevenue_Master.SRR_Typeof_Employement = salesClientWiseMonthlyRevenueModel.srtypeofemployement;
                    salesClientwiseMonthlyRevenue_Master.SRR_Attrition = salesClientWiseMonthlyRevenueModel.srattrition;
                    salesClientwiseMonthlyRevenue_Master.SRR_BD = salesClientWiseMonthlyRevenueModel.srbd;
                    salesClientwiseMonthlyRevenue_Master.SRR_Actual_Start = salesClientWiseMonthlyRevenueModel.sractualstart;
                    salesClientwiseMonthlyRevenue_Master.SRR_Create_By = salesClientWiseMonthlyRevenueModel.srusername;
                    salesClientwiseMonthlyRevenue_Master.SRR_Create_On = System.DateTime.Now;
                    db.SalesClientwiseMonthlyRevenue_Master.Add(salesClientwiseMonthlyRevenue_Master);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Record Inserted Successfully.";
                        responseModel.Data = null;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Insert Record";
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

        public ResponseModel GetSalesMonthlyRevenueReport()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<SalesClientWiseMonthlyRevenueViewModel> clientWiseMonthlyRevenueViewModels;
                    string userrole = "Admin", username = "";
                    if (userrole =="Sales")
                    {
                        clientWiseMonthlyRevenueViewModels = (from c in db.SalesClientwiseMonthlyRevenue_Master
                                                              where c.SRR_Create_By == username
                                                              join
                                                                  d in db.User_Master on c.SRR_Create_By equals d.E_UserName
                                                              join
                                                                  e in db.Client_Master on c.SRR_Client equals e.C_id
                                                              select new { c, d, e }).Select(x => new SalesClientWiseMonthlyRevenueViewModel
                                                              {
                                                                  id= x.c.SRR_Id,
                                                                  salesname = x.d.E_Fullname,
                                                                  month = x.c.SRR_Month,
                                                                  year = x.c.SRR_Year.ToString(),
                                                                  clientname = x.e.C_Name,
                                                                  currenthc = x.c.SRR_Current_HC != null ? (int)x.c.SRR_Current_HC : (int)0,
                                                                  totalgp = x.c.SRR_Total_GP != null ? (decimal)x.c.SRR_Total_GP : (decimal)0,
                                                                  averagegp = x.c.SRR_Avg_GP_Added != null ? (decimal)x.c.SRR_Avg_GP_Added : (decimal)0,
                                                                  start = x.c.SRR_Start != null ? (int)x.c.SRR_Start : (int)0,
                                                                  attrition = x.c.SRR_Attrition != null ? (int)x.c.SRR_Attrition : (int)0,
                                                                  bd = x.c.SRR_BD != null ? (int)x.c.SRR_BD : (int)0,
                                                                  actualstart = x.c.SRR_Actual_Start != null ? (int)x.c.SRR_Actual_Start : (int)0,
                                                                  nettotalgp = x.c.SRR_Net_Total_GP != null ? (decimal)x.c.SRR_Net_Total_GP : (decimal)0,
                                                                  nettotalgpadded = x.c.SRR_Total_GP_Added != null ? (decimal)x.c.SRR_Total_GP_Added : (decimal)0,
                                                                  typeofemployment = x.c.SRR_Typeof_Employement
                                                              }).ToList();

                    }
                    else
                    {
                        clientWiseMonthlyRevenueViewModels = (from c in db.SalesClientwiseMonthlyRevenue_Master
                                                              join
                                                                  d in db.User_Master on c.SRR_Create_By equals d.E_UserName
                                                              join
                                                                  e in db.Client_Master on c.SRR_Client equals e.C_id
                                                              select new { c, d, e }).Select(x => new SalesClientWiseMonthlyRevenueViewModel
                                                              {
                                                                  id = x.c.SRR_Id,
                                                                  salesname = x.d.E_Fullname,
                                                                  month = x.c.SRR_Month,
                                                                  year = x.c.SRR_Year.ToString(),
                                                                  clientname = x.e.C_Name,
                                                                  currenthc = x.c.SRR_Current_HC != null?(int)x.c.SRR_Current_HC:(int)0,
                                                                  totalgp = x.c.SRR_Total_GP != null?(decimal)x.c.SRR_Total_GP:(decimal)0,
                                                                  averagegp = x.c.SRR_Avg_GP_Added != null ? (decimal)x.c.SRR_Avg_GP_Added: (decimal)0,
                                                                  start = x.c.SRR_Start != null ? (int)x.c.SRR_Start:(int)0,
                                                                  attrition = x.c.SRR_Attrition != null ? (int)x.c.SRR_Attrition:(int)0,
                                                                  bd = x.c.SRR_BD != null ?(int)x.c.SRR_BD:(int)0,
                                                                  actualstart = x.c.SRR_Actual_Start != null ? (int)x.c.SRR_Actual_Start: (int)0,
                                                                  nettotalgp = x.c.SRR_Net_Total_GP != null?(decimal)x.c.SRR_Net_Total_GP:(decimal)0,
                                                                  nettotalgpadded = x.c.SRR_Total_GP_Added != null ?(decimal)x.c.SRR_Total_GP_Added:(decimal)0,
                                                                  typeofemployment = x.c.SRR_Typeof_Employement
                                                              }).ToList();

                    }
                    if (clientWiseMonthlyRevenueViewModels.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = clientWiseMonthlyRevenueViewModels;
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

        public ResponseModel DeleteSalesMonthlyRevenueReport(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClientwiseMonthlyRevenue_Master _Master = db.SalesClientwiseMonthlyRevenue_Master.Where(x => x.SRR_Id == id).FirstOrDefault();
                    if (_Master != null)
                    {
                        db.Entry(_Master).State = EntityState.Deleted;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Deleted Successfully";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Delete Record";
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
