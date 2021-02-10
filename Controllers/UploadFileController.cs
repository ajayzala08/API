using ATSAPI.Models;
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
using System.Data.OleDb;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;

namespace ATSAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class UploadFileController : ApiController
    {
        public ResponseModel PostUploads()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string message = "";
                HttpResponseMessage ResponseMessage = null;
                var httpRequest = HttpContext.Current.Request;
                DataSet dsexcelRecords = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                
                using (var db = new ATS2019_dbEntities())
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        Inputfile = httpRequest.Files[0];
                        FileStream = Inputfile.InputStream;

                        if (Inputfile != null && FileStream != null)
                        {
                            if (Inputfile.FileName.EndsWith(".xls"))
                                reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                            else if (Inputfile.FileName.EndsWith(".xlsx"))
                                reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            else
                                message = "The file format is not supported.";

                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();

                            if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                            {
                                int cnt = 0;
                                DataTable dtClientRecords = dsexcelRecords.Tables[0];
                                for (int i = 0; i < dtClientRecords.Rows.Count; i++)
                                {
                                    if (!dtClientRecords.Rows[i].ItemArray.Contains("Client Master"))
                                    {
                                        if ((dtClientRecords.Rows[i][0].ToString() != "") && (dtClientRecords.Rows[i][0].ToString() != "Client Name"))
                                        {
                                            string clientname = dtClientRecords.Rows[i][0].ToString();
                                            var exists = db.Client_Master.Where(x => x.C_Name == clientname).FirstOrDefault();
                                            if (exists == null)
                                            {
                                                Client_Master master = new Client_Master();
                                                master.C_Name = dtClientRecords.Rows[i][0].ToString();
                                                master.C_Address = dtClientRecords.Rows[i][1].ToString();
                                                master.C_Category = dtClientRecords.Rows[i][2].ToString();
                                                master.C_Type = dtClientRecords.Rows[i][3].ToString();
                                                master.C_Segment = dtClientRecords.Rows[i][4].ToString();
                                                master.C_Margin_Type = dtClientRecords.Rows[i][5].ToString();
                                                master.C_Margin = Convert.ToDecimal("0");
                                                master.C_Cont_Person1 = dtClientRecords.Rows[i][6].ToString();
                                                master.C_Cont_Person1_Email = dtClientRecords.Rows[i][7].ToString();
                                                master.C_Cont_Person1_No = dtClientRecords.Rows[i][8].ToString() != "" ? Convert.ToDecimal(dtClientRecords.Rows[i][8].ToString()) : Convert.ToDecimal("0");
                                                master.C_Cont_Person2 = dtClientRecords.Rows[i][9].ToString();
                                                master.C_Cont_Person2_Email = dtClientRecords.Rows[i][10].ToString();
                                                master.C_Cont_Person2_No = dtClientRecords.Rows[i][11].ToString() != "" ? Convert.ToDecimal(dtClientRecords.Rows[i][11].ToString()) : Convert.ToDecimal("0");
                                                master.C_Is_Active = Convert.ToInt32("1");
                                                master.C_Is_Delete = Convert.ToInt32("0");
                                                master.C_Created_By = "";
                                                master.C_Created_On = System.DateTime.Now;
                                                db.Client_Master.Add(master);
                                                var result = db.SaveChanges();
                                                if (result > 0)
                                                {
                                                    cnt++;
                                                }

                                            }
                                        }
                                    }
                                }
                                responseModel.Message = cnt.ToString() + "-Records Added";
                                responseModel.Data = null;
                                responseModel.Status = true;

                            }
                            else
                            {

                                responseModel.Message = "Selected file is empty";
                                responseModel.Data = null;
                                responseModel.Status = false;
                            }
                        }
                        else
                        {
                            responseModel.Message = "Invalid File";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                        ResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
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

            return responseModel;
        }
    }
}
