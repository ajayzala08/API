using ATSAPI.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class UploadSalarySheetController : ApiController
    {
        public ResponseModel POSTUploadSalarySheet(string companayname)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                DataSet dsExcelRecords = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                string[] extension = httpRequest.Files[0].FileName.Split('.');
                if ((extension[1].ToString().ToLower() == "xls") || (extension[1].ToString().ToLower() == "xlsx"))
                {
                    Inputfile = httpRequest.Files[0];
                    FileStream = Inputfile.InputStream;
                    if (extension[1].ToString().ToLower() == "xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                    else if (extension[1].ToString().ToLower() == "xlsx")
                        reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                    else
                        responseModel.Message = "File Format Not Supported. Only 'xls' and '.xlsx' file supported.";

                    dsExcelRecords = reader.AsDataSet();
                    reader.Close();
                    if ((dsExcelRecords != null) && (dsExcelRecords.Tables.Count > 0))
                    {
                        string month = string.Empty, year = string.Empty;
                        int total = 0, sucess = 0, fail = 0;
                        DataTable dtrecords = dsExcelRecords.Tables[0];
                        for (int i = 0; i < dtrecords.Rows.Count; i++)
                        {
                            if (!dtrecords.Rows[i].ItemArray.Contains("ARCHE SOFTRONIX PVT LTD."))
                            {
                                if (dtrecords.Rows[i].ItemArray.Contains("Salary Sheet") && dtrecords.Rows[i].ItemArray.Contains("Month"))
                                {
                                    string[] monthyear = dtrecords.Rows[i][5].ToString().Split('-');
                                    month = monthyear[0];
                                    year = monthyear[1];
                                    break;
                                }
                            }
                        }
                        foreach (DataRow row in dtrecords.Rows)
                        {
                            if ((row == null) || (row.ItemArray.Contains("ARCHE SOFTRONIX PVT LTD.")) || (row.ItemArray.Contains("Salary Sheet")) || (row.ItemArray.Contains("Fixed Salary Details")) || (row.ItemArray.Contains("EMPLOYEE CODE")) || (row.ItemArray.Contains("Regular Office Staff")) || (row.ItemArray.Contains("Total")) || (row.ItemArray.Contains("On Site Employees")) || (row.ItemArray.Contains("Trainee Office Staff")))
                            {
                                continue;
                            }
                            else
                            {
                                total++;
                                if (companayname == "ARCHE SOFTRONIX PVT LTD")
                                {
                                    bool arche = insertarchesoftronix(row, month, year);
                                    if (arche)
                                    {
                                        sucess++;
                                    }
                                    else
                                    {
                                        fail++;
                                    }
                                }
                                else
                                {
                                    bool reyna = insertreyna(row, month, year);
                                    if (reyna)
                                    {
                                        sucess++;
                                    }
                                    else
                                    {
                                        fail++;
                                    }
                                }
                                continue;
                            }
                        }
                        responseModel.Message = sucess + " record added succesfully.";
                    }
                    else
                    {
                        responseModel.Message = "File is empty";
                        responseModel.Data = null;
                        responseModel.Status = false;
                    }
                    
                }
                else
                {
                    responseModel.Message = "File Format Not Supported. Only 'xls' and '.xlsx' file supported.";
                    responseModel.Data = null;
                    responseModel.Status = false;
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
        private bool insertarchesoftronix(DataRow row, string month, string year)
        {
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Recruiter_SalarySlip_Master master = new Recruiter_SalarySlip_Master();
                    master.Employee_Code = row.ItemArray[1] != null ? Convert.ToDecimal(row.ItemArray[1].ToString()) : Convert.ToDecimal("0");
                    master.Employee_Name = row.ItemArray[2] != null ? row.ItemArray[2].ToString() : "";
                    master.Employee_Pan = row.ItemArray[3] != null ? row.ItemArray[3].ToString() : "";

                    master.CTC = row.ItemArray[4] != null ? Convert.ToDecimal(row.ItemArray[4].ToString()) : Convert.ToDecimal("0");
                    master.Basic_Salary = row.ItemArray[5] != null ? Convert.ToDecimal(row.ItemArray[5].ToString()) : Convert.ToDecimal("0");
                    master.Fixed_HRA = row.ItemArray[6] != null ? Convert.ToDecimal(row.ItemArray[6].ToString()) : Convert.ToDecimal("0");
                    master.Fixed_Conveyance_Allowance = row.ItemArray[7] != null ? Convert.ToDecimal(row.ItemArray[7].ToString()) : Convert.ToDecimal("0");
                    master.Fixed_Medical_Allowance = row.ItemArray[8] != null ? Convert.ToDecimal(row.ItemArray[8]) : Convert.ToDecimal("0");
                    master.Additional_HRA_Allowance = row.ItemArray[9] != null ? Convert.ToDecimal(row.ItemArray[9]) : Convert.ToDecimal("0");
                    master.Total_Days = row.ItemArray[10] != null ? Convert.ToDecimal(row.ItemArray[10]) : Convert.ToDecimal("0");
                    master.Paid_Leave = row.ItemArray[11] != null ? Convert.ToDecimal(row.ItemArray[11]) : Convert.ToDecimal("0");
                    master.LWP_Days = row.ItemArray[12] != null ? Convert.ToDecimal(row.ItemArray[12]) : Convert.ToDecimal("0");
                    master.Total_Payable_Days = row.ItemArray[13] != null ? Convert.ToDecimal(row.ItemArray[13]) : Convert.ToDecimal("0");
                    master.Gross_Salary_Payable = row.ItemArray[14] != null ? Convert.ToDecimal(row.ItemArray[14]) : Convert.ToDecimal("0");
                    master.Basic = row.ItemArray[15] != null ? Convert.ToDecimal(row.ItemArray[15]) : Convert.ToDecimal("0");
                    master.House_Rent = row.ItemArray[16] != null ? Convert.ToDecimal(row.ItemArray[16]) : Convert.ToDecimal("0");
                    master.Employer_Cont_To_PF = row.ItemArray[17] != null ? Convert.ToDecimal(row.ItemArray[17]) : Convert.ToDecimal("0");
                    master.Conveyance_Allowance = row.ItemArray[18] != null ? Convert.ToDecimal(row.ItemArray[18]) : Convert.ToDecimal("0");
                    master.Medical_Allowance = row.ItemArray[19] != null ? Convert.ToDecimal(row.ItemArray[19]) : Convert.ToDecimal("0");
                    master.Add_HRA_Allowance = row.ItemArray[20] != null ? Convert.ToDecimal(row.ItemArray[20]) : Convert.ToDecimal("0");
                    master.Flexible_Allowance = row.ItemArray[21] != null ? Convert.ToDecimal(row.ItemArray[21]) : Convert.ToDecimal("0");
                    master.Incentive_Allowance = row.ItemArray[22] != null ? Convert.ToDecimal(row.ItemArray[22]) : Convert.ToDecimal("0");
                    master.Total_Earning = row.ItemArray[23] != null ? Convert.ToDecimal(row.ItemArray[23]) : Convert.ToDecimal("0");
                    master.PF_Employer = row.ItemArray[24] != null ? Convert.ToDecimal(row.ItemArray[24]) : Convert.ToDecimal("0");
                    master.PF_Employee = row.ItemArray[25] != null ? Convert.ToDecimal(row.ItemArray[25]) : Convert.ToDecimal("0");
                    master.Esic_Employer = row.ItemArray[26] != null ? Convert.ToDecimal(row.ItemArray[26]) : Convert.ToDecimal("0");
                    master.Esic_Employee = row.ItemArray[27] != null ? Convert.ToDecimal(row.ItemArray[27]) : Convert.ToDecimal("0");
                    master.PT = row.ItemArray[28] != null ? Convert.ToDecimal(row.ItemArray[28]) : Convert.ToDecimal("0");
                    master.Advances = row.ItemArray[29] != null ? Convert.ToDecimal(row.ItemArray[29]) : Convert.ToDecimal("0");
                    master.Income_Tax = row.ItemArray[30] != null ? Convert.ToDecimal(row.ItemArray[30]) : Convert.ToDecimal("0");
                    master.Total_Deduction = row.ItemArray[31] != null ? Convert.ToDecimal(row.ItemArray[31]) : Convert.ToDecimal("0");
                    master.Net_Payable = row.ItemArray[32] != null ? Convert.ToDecimal(row.ItemArray[32]) : Convert.ToDecimal("0");
                    master.Salary_Month = month;
                    master.Salary_Year = year;
                    db.Recruiter_SalarySlip_Master.Add(master);
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
            catch (Exception ex)
            {
                return false;
            }


            
        }

        private bool insertreyna(DataRow row, string month, string year)
        {
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Reyna_SalarySlip_Master master = new Reyna_SalarySlip_Master();
                    master.Employee_Code = row.ItemArray[1] != null ? Convert.ToDecimal(row.ItemArray[1].ToString()) : Convert.ToDecimal("0");
                    master.Employee_Name = row.ItemArray[2] != null ? row.ItemArray[2].ToString() : "";
                    master.Employee_Pan = row.ItemArray[3] != null ? row.ItemArray[3].ToString() : "";

                    master.CTC = row.ItemArray[4] != null ? Convert.ToDecimal(row.ItemArray[4].ToString()) : Convert.ToDecimal("0");
                    master.Basic_Salary = row.ItemArray[5] != null ? Convert.ToDecimal(row.ItemArray[5].ToString()) : Convert.ToDecimal("0");
                    master.Fixed_HRA = row.ItemArray[6] != null ? Convert.ToDecimal(row.ItemArray[6].ToString()) : Convert.ToDecimal("0");
                    master.Fixed_Conveyance_Allowance = row.ItemArray[7] != null ? Convert.ToDecimal(row.ItemArray[7].ToString()) : Convert.ToDecimal("0");
                    master.Fixed_Medical_Allowance = row.ItemArray[8] != null ? Convert.ToDecimal(row.ItemArray[8]) : Convert.ToDecimal("0");
                    master.Additional_HRA_Allowance = row.ItemArray[9] != null ? Convert.ToDecimal(row.ItemArray[9]) : Convert.ToDecimal("0");
                    master.Total_Days = row.ItemArray[10] != null ? Convert.ToDecimal(row.ItemArray[10]) : Convert.ToDecimal("0");
                    master.Paid_Leave = row.ItemArray[11] != null ? Convert.ToDecimal(row.ItemArray[11]) : Convert.ToDecimal("0");
                    master.LWP_Days = row.ItemArray[12] != null ? Convert.ToDecimal(row.ItemArray[12]) : Convert.ToDecimal("0");
                    master.Total_Payable_Days = row.ItemArray[13] != null ? Convert.ToDecimal(row.ItemArray[13]) : Convert.ToDecimal("0");
                    master.Gross_Salary_Payable = row.ItemArray[14] != null ? Convert.ToDecimal(row.ItemArray[14]) : Convert.ToDecimal("0");
                    master.Basic = row.ItemArray[15] != null ? Convert.ToDecimal(row.ItemArray[15]) : Convert.ToDecimal("0");
                    master.House_Rent = row.ItemArray[16] != null ? Convert.ToDecimal(row.ItemArray[16]) : Convert.ToDecimal("0");
                    master.Employer_Cont_To_PF = row.ItemArray[17] != null ? Convert.ToDecimal(row.ItemArray[17]) : Convert.ToDecimal("0");
                    master.Conveyance_Allowance = row.ItemArray[18] != null ? Convert.ToDecimal(row.ItemArray[18]) : Convert.ToDecimal("0");
                    master.Medical_Allowance = row.ItemArray[19] != null ? Convert.ToDecimal(row.ItemArray[19]) : Convert.ToDecimal("0");
                    master.Add_HRA_Allowance = row.ItemArray[20] != null ? Convert.ToDecimal(row.ItemArray[20]) : Convert.ToDecimal("0");
                    master.Flexible_Allowance = row.ItemArray[21] != null ? Convert.ToDecimal(row.ItemArray[21]) : Convert.ToDecimal("0");
                    master.Incentive_Allowance = row.ItemArray[22] != null ? Convert.ToDecimal(row.ItemArray[22]) : Convert.ToDecimal("0");
                    master.Total_Earning = row.ItemArray[23] != null ? Convert.ToDecimal(row.ItemArray[23]) : Convert.ToDecimal("0");
                    master.PF_Employer = row.ItemArray[24] != null ? Convert.ToDecimal(row.ItemArray[24]) : Convert.ToDecimal("0");
                    master.PF_Employee = row.ItemArray[25] != null ? Convert.ToDecimal(row.ItemArray[25]) : Convert.ToDecimal("0");
                    master.Esic_Employer = row.ItemArray[26] != null ? Convert.ToDecimal(row.ItemArray[26]) : Convert.ToDecimal("0");
                    master.Esic_Employee = row.ItemArray[27] != null ? Convert.ToDecimal(row.ItemArray[27]) : Convert.ToDecimal("0");
                    master.PT = row.ItemArray[28] != null ? Convert.ToDecimal(row.ItemArray[28]) : Convert.ToDecimal("0");
                    master.Advances = row.ItemArray[29] != null ? Convert.ToDecimal(row.ItemArray[29]) : Convert.ToDecimal("0");
                    master.Income_Tax = row.ItemArray[30] != null ? Convert.ToDecimal(row.ItemArray[30]) : Convert.ToDecimal("0");
                    master.Total_Deduction = row.ItemArray[31] != null ? Convert.ToDecimal(row.ItemArray[31]) : Convert.ToDecimal("0");
                    master.Net_Payable = row.ItemArray[32] != null ? Convert.ToDecimal(row.ItemArray[32]) : Convert.ToDecimal("0");
                    master.Salary_Month = month;
                    master.Salary_Year = year;
                    db.Reyna_SalarySlip_Master.Add(master);
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
            catch (Exception ex)
            {
                return false;
            }



        }

    }
}
