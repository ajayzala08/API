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
    public class UpdateRequirementStatusController : ApiController
    {
        public ResponseModel put(decimal jid, string status)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    
                    var UpdateReqStats = db.Requirement_Master.Where(c => c.J_Id == jid).FirstOrDefault();
                    if (UpdateReqStats != null)
                    {
                        UpdateReqStats.J_Status = status;
                        if (status == "Delete")
                        {
                            UpdateReqStats.J_Is_Delete = Convert.ToInt16("0");
                        }
                        if (status == "Deactive")
                        {
                            UpdateReqStats.J_Is_Active = Convert.ToInt16("1");
                        }
                        db.Entry(UpdateReqStats).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Status = true;
                            responseModel.Message = "Requirement status updatd";
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Status = false;
                            responseModel.Message = "Fail to update requirement tatus";
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Record not found.";
                        responseModel.Data = null;
                    }
                }
                return responseModel;

            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = "Exception";
                responseModel.Data = "";
                return responseModel;
            }
        }
    }
}
