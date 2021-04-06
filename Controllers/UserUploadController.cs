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
    public class UserUploadController : ApiController
    {
        public ResponseModel POSTUserUpload(string username)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                DataSet dsexcelRecords = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                string[] extention = httpRequest.Files[0].FileName.ToString().Split('.');
                if ((extention[1].ToString().ToLower() == "xls") || (extention[1].ToString().ToLower() == "xlsx"))
                {
                    Inputfile = httpRequest.Files[0];
                    FileStream = Inputfile.InputStream;
                    if (extention[1].ToString().ToLower() == "xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                    else if (extention[1].ToString().ToLower() == "xlsx")
                        reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                    else
                        responseModel.Message = "File Formate Not Supported.";
                    dsexcelRecords = reader.AsDataSet();
                    reader.Close();

                    if ((dsexcelRecords != null) && (dsexcelRecords.Tables.Count > 0))
                    {
                        int total = 0, fail = 0, success = 0;
                        DataTable dtusers = dsexcelRecords.Tables[0];
                        for (int i = 0; i < dtusers.Rows.Count; i++)
                        {
                            if (!dtusers.Rows[i].ItemArray.Contains("User Master"))
                            {
                                if ((dtusers.Rows[i][0].ToString() !="") && (dtusers.Rows[i][0].ToString() != "Employee Code"))
                                {
                                    using (var db = new ATS2019_dbEntities())
                                    {
                                        total++;
                                        decimal eid = dtusers.Rows[i][0].ToString() != "" ? Convert.ToDecimal(dtusers.Rows[i][0].ToString()) : Convert.ToDecimal("0");
                                        if (eid > 0)
                                        {
                                            bool exists = db.User_Master.Where(x => x.E_Code == eid).Any();
                                            if (!exists)
                                            {
                                                User_Master master = new User_Master();
                                                master.E_Code = eid;
                                                master.E_Fullname = dtusers.Rows[i][1].ToString() != "" ? dtusers.Rows[i][1].ToString() : "";
                                                master.E_Gender = dtusers.Rows[i][2].ToString() != "" ? dtusers.Rows[i][2].ToString() : "";
                                                master.E_Email = dtusers.Rows[i][3].ToString() != "" ? dtusers.Rows[i][3].ToString() : "";
                                                master.E_DOB = dtusers.Rows[i][4].ToString() != "" ? Convert.ToDateTime(dtusers.Rows[i][4].ToString()) : Convert.ToDateTime(null);
                                                master.E_ContNo = dtusers.Rows[i][5].ToString() != "" ? Convert.ToDecimal(dtusers.Rows[i][5].ToString()) : Convert.ToDecimal("0");
                                                master.E_EEmergencyContNo = dtusers.Rows[i][6].ToString() != "" ? Convert.ToDecimal(dtusers.Rows[i][6].ToString()) : Convert.ToDecimal("0");
                                                master.E_UserName = dtusers.Rows[i][7].ToString() != "" ? dtusers.Rows[i][7].ToString() : "";
                                                master.E_Password = dtusers.Rows[i][8].ToString() != "" ? dtusers.Rows[i][8].ToString() : "";
                                                master.E_Address = dtusers.Rows[i][9].ToString() != "" ? dtusers.Rows[i][9].ToString() : "";
                                                master.E_CAddress = dtusers.Rows[i][10].ToString() != "" ? dtusers.Rows[i][10].ToString() : "";
                                                master.E_Postal = dtusers.Rows[i][11].ToString() != "" ? Convert.ToDecimal(dtusers.Rows[i][11].ToString()) : Convert.ToDecimal("0");
                                                master.E_Etype = dtusers.Rows[i][12].ToString() != "" ? dtusers.Rows[i][12].ToString() : "";
                                                master.E_Company_Name = dtusers.Rows[i][13].ToString() != "" ? dtusers.Rows[i][13].ToString() : "";
                                                master.E_Department = dtusers.Rows[i][14].ToString() != "" ? dtusers.Rows[i][14].ToString() : "";
                                                master.E_Designation = dtusers.Rows[i][15].ToString() != "" ? dtusers.Rows[i][15].ToString() : "";
                                                master.E_Location = dtusers.Rows[i][16].ToString() != "" ? dtusers.Rows[i][16].ToString() : "";
                                                master.E_OfferDate = dtusers.Rows[i][17].ToString() != "" ? Convert.ToDateTime(dtusers.Rows[i][17]) : Convert.ToDateTime(null);
                                                master.E_JoinDate = dtusers.Rows[i][18].ToString() != "" ? Convert.ToDateTime(dtusers.Rows[i][18].ToString()) : Convert.ToDateTime(null);
                                                master.E_Role = dtusers.Rows[i][19].ToString() != "" ? dtusers.Rows[i][19].ToString() : "";
                                                master.E_BloodGroup = dtusers.Rows[i][20].ToString() != "" ? dtusers.Rows[i][20].ToString() : "";
                                                master.E_BankName = dtusers.Rows[i][21].ToString() != "" ? dtusers.Rows[i][21].ToString() : "";
                                                master.E_AccountNo = dtusers.Rows[i][22].ToString() != "" ? dtusers.Rows[i][22].ToString() : "";
                                                master.E_Branch = dtusers.Rows[i][23].ToString() != "" ? dtusers.Rows[i][23].ToString() : "";
                                                master.E_IFC_Code = dtusers.Rows[i][24].ToString() != "" ? dtusers.Rows[i][24].ToString() : "";
                                                master.E_PF_Account = dtusers.Rows[i][25].ToString() != "" ? dtusers.Rows[i][25].ToString() : "";
                                                master.E_PAN_No = dtusers.Rows[i][26].ToString() != "" ? dtusers.Rows[i][26].ToString() : "";
                                                master.E_Aadhar_no = dtusers.Rows[i][27].ToString() != "" ? Convert.ToDecimal(dtusers.Rows[i][27].ToString()) : Convert.ToDecimal("0");
                                                master.E_Salary = dtusers.Rows[i][28].ToString() != "" ? Convert.ToDouble(dtusers.Rows[i][28]) : Convert.ToDouble("0");
                                                master.E_PF = dtusers.Rows[i][29].ToString() != "" ? Convert.ToDouble(dtusers.Rows[i][29]) : Convert.ToDouble("0");
                                                master.E_PT = dtusers.Rows[i][30].ToString() != "" ? Convert.ToDouble(dtusers.Rows[i][30]) : Convert.ToDouble("0");
                                                master.E_ESI = dtusers.Rows[i][31].ToString() != "" ? Convert.ToDouble(dtusers.Rows[i][31]) : Convert.ToDouble("0");
                                                master.E_PF_Apply = Convert.ToInt16("1");
                                                master.E_PT_Apply = Convert.ToInt16("1");
                                                master.E_ESI_Apply = Convert.ToInt16("1");
                                                master.E_Photo = "Dummy.png";
                                                master.E_Is_Active = Convert.ToInt32("1");
                                                master.E_Is_Delete = Convert.ToInt32("0");
                                                master.E_Created_By = username;
                                                master.E_Created_On = System.DateTime.Now;
                                                master.E_ReportingManager = dtusers.Rows[i][36].ToString() != "" ? dtusers.Rows[i][36].ToString() : "";
                                                master.E_PersonalMailId = dtusers.Rows[i][37].ToString() != "" ? dtusers.Rows[i][37].ToString() : "";
                                                db.User_Master.Add(master);
                                                var result = db.SaveChanges();
                                                if (result > 0)
                                                {
                                                    success++;
                                                }
                                                else
                                                {
                                                    fail++;
                                                }

                                            }
                                            else
                                            {
                                                fail++;
                                            }
                                        }
                                        else
                                        {
                                            fail++;
                                        }
                                    }
                                }
                            }
                        }
                        responseModel.Message = success.ToString() + " out of " + total.ToString() + " user added successfully.";
                        responseModel.Data = null;
                        responseModel.Status = true;
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
