using ATSAPI.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResumeMasterController : ApiController
    {
        public ResponseModel PostResumeMaster(decimal jid, string username)
        {

            ResponseModel responseModel = new ResponseModel();
            try
            {
                string message = "";

                var httpRequest = HttpContext.Current.Request;
                DataSet dsexcelRecords = new DataSet();
                //IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                string[] ext = httpRequest.Files[0].FileName.Split('.');
                if (ext[1].ToString().ToLower() == "docx")
                {
                    Stream input = httpRequest.Files[0].InputStream;

                    using (Stream file = File.OpenWrite(HttpContext.Current.Server.MapPath("~/ResumeDirectory/" + httpRequest.Files[0].FileName)))
                    {
                        input.CopyTo(file);
                        //close file  
                        file.Close();
                    }
                    //input.CopyTo(HttpContext.Current.Server.MapPath("~/ResumeDirectory/" + httpRequest.Files[0].FileName));
                    //var resumename = ext[0].ToString() + "-" + jid.ToString() + "." + ext[1].ToString();
                    ResumeModel rm = new ResumeModel();
                    //httpRequest.SaveAs(resumename, true);
                    var filePath = HttpContext.Current.Server.MapPath("~/ResumeDirectory/" + httpRequest.Files[0].FileName);
                    //httpRequest.SaveAs(filePath, true);

                    //string fileName = Server.MapPath("\\ResumeDirectory\\" + resumename);
                    using (WordprocessingDocument doc =
                        WordprocessingDocument.Open(filePath, true))
                    {
                        try
                        {
                            // Find the first table in the document.  
                            Table table =
                                doc.MainDocumentPart.Document.Body.Elements<Table>().First(); ;

                            // Find the second row in the table.  
                            TableRow row = table.Elements<TableRow>().ElementAt(1);
                            rm.rname = row.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the third row in the table
                            TableRow row1 = table.Elements<TableRow>().ElementAt(2);
                            rm.rcnt = row1.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            //Find the fourth row in the table
                            TableRow row2 = table.Elements<TableRow>().ElementAt(3);
                            rm.rfamilycnt = row2.Elements<TableCell>().ElementAt(1).InnerText.ToString().Trim();

                            //Find the fifth row in the table
                            TableRow row3 = table.Elements<TableRow>().ElementAt(4);
                            rm.rfriendcnt = row3.Elements<TableCell>().ElementAt(1).InnerText.ToString().Trim();

                            //Find the sixth row in the table
                            TableRow row4 = table.Elements<TableRow>().ElementAt(5);
                            rm.remail = row4.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the seventh row in the table
                            TableRow row5 = table.Elements<TableRow>().ElementAt(6);
                            rm.rtotexp = row5.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the eighth row in the table
                            TableRow row6 = table.Elements<TableRow>().ElementAt(7);
                            rm.rrelexp = row6.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the nineth row in the table
                            TableRow row7 = table.Elements<TableRow>().ElementAt(8);
                            rm.redu = row7.Elements<TableCell>().ElementAt(1).InnerText.ToString();


                            // Find the tenth row in the table
                            TableRow row8 = table.Elements<TableRow>().ElementAt(9);
                            rm.rreasongap = row8.Elements<TableCell>().ElementAt(1).InnerText.ToString();


                            // Find the eleven row in the table
                            TableRow row9 = table.Elements<TableRow>().ElementAt(10);
                            rm.rcom = row9.Elements<TableCell>().ElementAt(1).InnerText.ToString();


                            // Find the twelve row in the table
                            TableRow row10 = table.Elements<TableRow>().ElementAt(11);
                            rm.rdes = row10.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the thirteen row in the table
                            TableRow row11 = table.Elements<TableRow>().ElementAt(12);
                            rm.rctc = row11.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the fourteen row in the table
                            TableRow row12 = table.Elements<TableRow>().ElementAt(13);
                            rm.rcurtakehome = row12.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the fifteen row in the table
                            TableRow row13 = table.Elements<TableRow>().ElementAt(14);
                            rm.rcurdrawing = row13.Elements<TableCell>().ElementAt(1).InnerText.ToString();


                            // Find the sixteen row in the table
                            TableRow row14 = table.Elements<TableRow>().ElementAt(15);
                            rm.rlastsalaryhike = row14.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the seventeen row in the table
                            TableRow row15 = table.Elements<TableRow>().ElementAt(16);
                            rm.rectc = row15.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the eighteen row in the table
                            TableRow row16 = table.Elements<TableRow>().ElementAt(17);
                            rm.rexptakehome = row16.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the nineteen row in the table
                            TableRow row17 = table.Elements<TableRow>().ElementAt(18);
                            rm.rexpdrawing = row17.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twenty row in the table
                            TableRow row18 = table.Elements<TableRow>().ElementAt(19);
                            rm.rhikereason = row18.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentytwo row in the table
                            TableRow row20 = table.Elements<TableRow>().ElementAt(20);
                            rm.rnp = row20.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentythree row in the table
                            TableRow row21 = table.Elements<TableRow>().ElementAt(21);
                            rm.rcanjoin = row21.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentyfour row in the table
                            TableRow row22 = table.Elements<TableRow>().ElementAt(22);
                            rm.rhowearlyjoinreason = row22.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentyfive row in the table
                            TableRow row23 = table.Elements<TableRow>().ElementAt(23);
                            rm.rwhyjoinarche = row23.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentysix row in the table
                            TableRow row24 = table.Elements<TableRow>().ElementAt(24);
                            rm.rhavedocs = row24.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentyseven row in the table
                            TableRow row25 = table.Elements<TableRow>().ElementAt(25);
                            rm.rcloc = row25.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentyeight row in the table
                            TableRow row26 = table.Elements<TableRow>().ElementAt(26);
                            rm.rpcloc = row26.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the twentynine row in the table
                            TableRow row27 = table.Elements<TableRow>().ElementAt(27);
                            rm.rreasonforlocation = row27.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the thirty row in the table
                            TableRow row28 = table.Elements<TableRow>().ElementAt(28);
                            rm.rnative = row28.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the thirtyone row in the table
                            TableRow row29 = table.Elements<TableRow>().ElementAt(29);
                            string dd = row29.Elements<TableCell>().ElementAt(1).InnerText.ToString().Trim();
                            if ((dd == null) || (dd.Trim() == ""))
                            {
                                rm.rdob = null;
                            }
                            else
                            {
                                try
                                {
                                    if (dd.Contains("-"))
                                    {
                                        dd = dd.Replace("-", "/");
                                        string dd1 = " 00:00:00";
                                        dd = dd + dd1;
                                        CultureInfo provider = CultureInfo.InvariantCulture;
                                        provider = new CultureInfo("en-Us", true);
                                        string format = "dd/MM/yyyy HH:mm:ss";
                                        rm.rdob = DateTime.ParseExact(dd, format, provider);
                                    }
                                    else
                                    {
                                        string dd1 = " 00:00:00";
                                        dd = dd + dd1;

                                        rm.rdob = Convert.ToDateTime(dd.ToString());
                                    }
                                }
                                catch (FormatException fe)
                                {
                                    responseModel.Message = "Closing Block Exception";
                                    responseModel.Data = null;
                                    responseModel.Status = false;
                                    return responseModel;
                                }

                            }

                            // Find the thirtythree row in the table
                            TableRow row31 = table.Elements<TableRow>().ElementAt(30);
                            rm.rpannumber = row31.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the thirtyfour row in the table
                            TableRow row32 = table.Elements<TableRow>().ElementAt(31);
                            rm.rtelephonictiming = row32.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the thirtyfive row in the table
                            TableRow row33 = table.Elements<TableRow>().ElementAt(32);
                            rm.rf2favailibilty = row33.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the thirtysix row in the table
                            TableRow row34 = table.Elements<TableRow>().ElementAt(33);
                            rm.rskills = row34.Elements<TableCell>().ElementAt(1).InnerText.ToString();

                            // Find the first paragraph in the table cell.  
                            //Paragraph parag = cell.Elements<Paragraph>().First();

                            // Find the first run in the paragraph.  
                            //Run run = parag.Elements<Run>().First();

                            // Set the text for the run.  
                            //Text text = run.Elements<Text>().First();
                            //text.Text = addedText;

                            using (var db = new ATS2019_dbEntities())
                            {
                                Resume_Master master = new Resume_Master();
                                master.R_Jid = jid;
                                master.R_Name = rm.rname;
                                master.R_DOB = rm.rdob;
                                master.R_Address = rm.radd;
                                master.R_Cnt = rm.rcnt;
                                master.R_Alt_Cnt = rm.raltcnt;
                                master.R_Email = rm.remail;
                                master.R_Tot_Exp = rm.rtotexp;
                                master.R_Rel_Exp = rm.rrelexp;
                                master.R_Cur_Com = rm.rcom;
                                master.R_Cur_Des = rm.rdes;
                                master.R_NP = rm.rnp;
                                master.R_Canjoin = rm.rcanjoin;
                                master.R_Cur_CTC = rm.rctc;
                                master.R_Exp_CTC = rm.rectc;
                                master.R_Reason = rm.rreason;
                                master.R_Any_Int_Of = rm.rintoff;
                                master.R_Education = rm.redu;
                                master.R_Cur_Location = rm.rcloc;
                                master.R_Pref_Location = rm.rpcloc;
                                master.R_Native = rm.rnative;
                                master.R_Resumename = httpRequest.Files[0].FileName;
                                master.R_Status = "New";
                                master.R_CreateBy = username;
                                master.R_CreateOn = System.DateTime.Now;
                                master.R_Skills = rm.rskills;
                                master.R_FamilyCnt = rm.rfamilycnt;
                                master.R_friendcnt = rm.rfriendcnt;
                                master.R_reasongap = rm.rreasongap;
                                master.R_Currenttakehome = rm.rcurtakehome;
                                master.R_Currentdrawing = rm.rcurdrawing;
                                master.R_Lastsalaryhike = rm.rlastsalaryhike;
                                master.R_Expectedtakehome = rm.rexptakehome;
                                master.R_Expecteddrawing = rm.rexpdrawing;
                                master.R_HikeReason = rm.rhikereason;
                                master.R_Howjoinearlyreason = rm.rhowearlyjoinreason;
                                master.R_WhyJoinArche = rm.rwhyjoinarche;
                                master.R_HaveDocs = rm.rhavedocs;
                                master.R_Reasonofrelocation = rm.rreasonforlocation;
                                master.R_Pannumber = rm.rpannumber;
                                master.R_Telephonicinttiming = rm.rtelephonictiming;
                                master.R_F2Favailibility = rm.rf2favailibilty;
                                db.Resume_Master.Add(master);
                                var result = db.SaveChanges();
                                if (result > 0)
                                {
                                    responseModel.Message = "Resume Added Succssfully";
                                    responseModel.Data = null;
                                    responseModel.Status = true;
                                }
                                else
                                {
                                    responseModel.Message = "Fail To Add Resume";
                                    responseModel.Data = null;
                                    responseModel.Status = false;
                                }
                            }

                        }
                        catch (System.ArgumentOutOfRangeException exex)
                        {
                            responseModel.Message = "Invalid closing block format.";
                            responseModel.Data = null;
                            responseModel.Status = false;

                        }
                    }
                }
                else
                {
                    responseModel.Message = "File Format Not Supported.";
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

        public ResponseModel GetResumeMaster(decimal jid, string username)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var resumelist = (from c in db.Resume_Master where c.R_CreateBy==username
                                      where c.R_Jid == jid
                                      join
                                          d in db.User_Master on c.R_CreateBy equals d.E_UserName 
                                      select new { c, d }).ToList().OrderByDescending(m => m.c.R_CreateOn);
                    if (resumelist.Count() > 0)
                    {
                        responseModel.Status = true;
                        responseModel.Message = "Data Found";
                        responseModel.Data = resumelist;
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Data Not Found";
                        responseModel.Data = null;
                    }
                }

                return responseModel;
            } catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = "Exception";
                responseModel.Data = null;
                return responseModel;
            }
        }
    }
}
