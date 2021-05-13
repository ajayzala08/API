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
    public class ResumeStatusController : ApiController
    {
        public ResponseModel put(decimal resid,string resumestatus)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {

                    if ((resid.ToString() != "") && (resumestatus != ""))
                    {
                        var findresume = db.Resume_Master.Where(c => c.R_Id == resid).FirstOrDefault();
                        if (findresume != null)
                        {
                            findresume.R_Status = resumestatus;
                            db.Entry(findresume).State = EntityState.Modified;
                            var result = db.SaveChanges();
                            if (result > 0)
                            {
                                response.Data = null;
                                response.Message = "Status updated successfully";
                                response.Status = true;
                            }
                            else
                            {
                                response.Data = null;
                                response.Message = "Fail to update successfully";
                                response.Status = false;
                            }
                        }
                        else
                        {
                            response.Data = null;
                            response.Message = "Record not found";
                            response.Status = false;
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = "Exception";
                response.Status = false;
                return response;
            }

        }
    }
}
