using ATSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class ResumeMasterController : ApiController
    {
        public ResponseModel PostResumeMaster(ResumeUploadModel resume)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var jobid = resume.jid;
                    var username = resume.username;
                    var httpfile = resume.httpPostedFile;
                    var filename = httpfile.FileName;
                    string[] extension = httpfile.FileName.Split('.');
                    if (extension[1].ToString().ToLower() == "docx")
                    { 
                    
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
