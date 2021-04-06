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
    public class RequirementUploadController : ApiController
    {
        public ResponseModel POSTRequirementUpload(string username)
        {
             ResponseModel responseModel  = new ResponseModel();
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
                    if (httpRequest.Files[0].FileName.EndsWith(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                    else if (httpRequest.Files[0].FileName.EndsWith(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                    else
                        responseModel.Message = "The file format is not supported.";

                    dsexcelRecords = reader.AsDataSet();
                    reader.Close();
                    if ((dsexcelRecords != null) && (dsexcelRecords.Tables.Count > 0))
                    {
                        int tot = 0, fail = 0, success = 0;
                        DataTable dtrequirement = dsexcelRecords.Tables[0];
                        for (int i = 0; i < dtrequirement.Rows.Count; i++)
                        {
                            if (!dtrequirement.Rows[i].ItemArray.Contains("Requirement Master"))
                            {
                                if ((dtrequirement.Rows[i][0].ToString() != "") && (dtrequirement.Rows[i][0].ToString() != "Client Name"))
                                {
                                    using (var db = new ATS2019_dbEntities())
                                    {
                                        tot = tot + 1;
                                        Requirement_Master master = new Requirement_Master();
                                        master.J_Code = FindJobCode(dtrequirement.Rows[i][0].ToString());
                                        if (master.J_Code.ToString() != "")
                                        {
                                            master.J_Client_Id = FindClientId(dtrequirement.Rows[i][0].ToString());
                                            master.J_Skill = dtrequirement.Rows[i][1].ToString();
                                            master.J_Position = dtrequirement.Rows[i][2].ToString() != "" ? Convert.ToInt32(dtrequirement.Rows[i][2].ToString()) : Convert.ToInt32("0");
                                            master.J_Location = dtrequirement.Rows[i][3].ToString() != "" ? dtrequirement.Rows[i][3].ToString() : "";
                                            master.J_EndClient = dtrequirement.Rows[i][4].ToString() != "" ? dtrequirement.Rows[i][4].ToString() : "";
                                            master.J_AssignUser = dtrequirement.Rows[i][5].ToString() != "" ? dtrequirement.Rows[i][5].ToString() : "UnAssign";
                                            master.J_Tot_Min_Exp = dtrequirement.Rows[i][6].ToString() != "" ? Convert.ToDouble(dtrequirement.Rows[i][6].ToString()) : Convert.ToDouble("0");
                                            master.J_Tot_Max_Exp = dtrequirement.Rows[i][7].ToString() != "" ? Convert.ToDouble(dtrequirement.Rows[i][7].ToString()) : Convert.ToDouble("0");
                                            master.J_Rel_Min_Exp = dtrequirement.Rows[i][8].ToString() != "" ? Convert.ToDouble(dtrequirement.Rows[i][8].ToString()) : Convert.ToDouble("0");
                                            master.J_Rel_Max_Exp = dtrequirement.Rows[i][9].ToString() != "" ? Convert.ToDouble(dtrequirement.Rows[i][9].ToString()) : Convert.ToDouble("0");
                                            master.J_Bill_Rate = dtrequirement.Rows[i][10].ToString() != "" ? Convert.ToDecimal(dtrequirement.Rows[i][10].ToString()) : Convert.ToDecimal("0");
                                            master.J_Pay_Rate = dtrequirement.Rows[i][11].ToString() != "" ? Convert.ToDecimal(dtrequirement.Rows[i][11].ToString()) : Convert.ToDecimal("0");
                                            master.J_Category = dtrequirement.Rows[i][12].ToString() != "" ? dtrequirement.Rows[i][12].ToString() : "";
                                            master.J_Type = dtrequirement.Rows[i][13].ToString() != "" ? dtrequirement.Rows[i][13].ToString() : "";
                                            master.J_Employment_Type = dtrequirement.Rows[i][14].ToString() != "" ? dtrequirement.Rows[i][14].ToString() : "";
                                            master.J_POC = dtrequirement.Rows[i][15].ToString() != "" ? dtrequirement.Rows[i][15].ToString() : "";
                                            master.J_POC_No = dtrequirement.Rows[i][16].ToString() != "" ? Convert.ToDecimal(dtrequirement.Rows[i][16].ToString()) : Convert.ToDecimal("0");
                                            master.J_JD = dtrequirement.Rows[i][17].ToString() != "" ? dtrequirement.Rows[i][17].ToString() : "";
                                            master.J_Status = "Create";
                                            master.J_Is_Active = Convert.ToInt32("1");
                                            master.J_Is_Delete = Convert.ToInt32("0");
                                            master.J_CreatedBy = username;
                                            master.J_CreatedOn = System.DateTime.Now;
                                            master.J_MandatorySkill = dtrequirement.Rows[i][18].ToString() != "" ? dtrequirement.Rows[i][18].ToString() : "";
                                            db.Requirement_Master.Add(master);
                                            var result = db.SaveChanges();
                                            if (result > 0)
                                            {
                                                success = success + 1;
                                            }
                                            else
                                            {
                                                fail = fail + 1;
                                            }
                                        }
                                        else
                                        {
                                            fail = fail + 1;
                                        }
                                    }
                                    
                                }
                            }
                        }
                        responseModel.Message = success.ToString() + " out of " + tot.ToString() + " record added successfully.";
                        responseModel.Data = null;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "File is empty";
                        responseModel.Status = false;
                        responseModel.Data = null;
                    }

                }
                else
                {
                    responseModel.Message = "File Not Supported. Only '.xls' or '.xlsx' file supported.";
                    responseModel.Status = false;
                    responseModel.Data = null;
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

        private string FindJobCode(string companyname)
        {
            string jobcode=string.Empty;
            using (var db = new ATS2019_dbEntities())
            {
                var company = db.Client_Master.Where(x => x.C_Name == companyname).FirstOrDefault();
                if (company != null)
                {
                    var reqlist = db.Requirement_Master.Where(x => x.J_Client_Id == company.C_id).ToList();
                    if (reqlist.Count > 0)
                    {
                        var lastreq = reqlist.Last();
                        string[] lastjobcode = lastreq.J_Code.Split('-');
                        var nextjobcode = lastjobcode[0] + "-" + (Convert.ToDecimal(lastjobcode[1]) + Convert.ToDecimal("1")).ToString();
                        jobcode = nextjobcode;
                    }
                    else
                    {
                        char[] comchar = companyname.ToCharArray();
                        for (int i = 0; i < comchar.Length; i++)
                        {
                            int ind = Convert.ToInt32("1") + i;
                            var jcc = comchar[0].ToString() + comchar[ind].ToString() + "-1";

                            Boolean ch1 = string.IsNullOrWhiteSpace(comchar[0].ToString());
                            Boolean ch2 = string.IsNullOrWhiteSpace(comchar[ind].ToString());
                            if (!ch1 && !ch2)
                            {
                                Boolean check = db.Requirement_Master.Where(x=>x.J_Code == jcc.ToUpper()).Any();
                                if (!check)
                                {
                                    jobcode = jcc.ToUpper();
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            return jobcode;

        }

        private decimal FindClientId(string companyname)
        {
            using (var db = new ATS2019_dbEntities())
            {
                decimal cid = db.Client_Master.Where(x => x.C_Name == companyname).Select(x => x.C_id).FirstOrDefault();
                return cid;
            }
        }
    }
}
