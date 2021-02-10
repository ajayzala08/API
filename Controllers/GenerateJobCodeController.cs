using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ATSAPI.Models;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GenerateJobCodeController : ApiController
    {
        public ResponseModel GetJobCode(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    string jc = "";

                   // decimal comid = Convert.ToDecimal(id);

                    var ifclientexists = db.Requirement_Master.Where(c => c.J_Client_Id == id).ToList();
                    if (ifclientexists.Count() > 0)
                    {
                        var nextjobcode = ifclientexists.Last();
                        string[] jcs = nextjobcode.J_Code.ToString().Split('-');
                        decimal nextnumber = Convert.ToDecimal(jcs[1].ToString()) + Convert.ToDecimal("1");
                        var jcc = jcs[0] + "-" + nextnumber.ToString();
                        jc = jcc.ToUpper();
                    }
                    else
                    {
                        string companyname = db.Client_Master.Where(c => c.C_id == id).Select(c => c.C_Name).First();
                        if (companyname != null)
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
                                    Boolean check = checkjobcodeexistsornot(jcc.ToUpper());
                                    if (!check)
                                    {
                                        jc = jcc.ToUpper();
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            responseModel.Message = "Client Not Exists";
                            responseModel.Status = false;
                            responseModel.Data = null;
                            return responseModel;
                        }
                    }
                    responseModel.Message = "JobCode Generated Successfully";
                    responseModel.Status = true;
                    responseModel.Data = jc;
                    return responseModel;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }


        }
        private Boolean checkjobcodeexistsornot(string jc)
        {
            using (var db = new ATS2019_dbEntities())
            {
                bool findjobcode = db.Requirement_Master.Where(c => c.J_Code == jc).Any();
                if (findjobcode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
