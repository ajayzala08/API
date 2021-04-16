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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SearchResumeController : ApiController
    {
        public ResponseModel GetSearchResume(string txtby, string txtvalue)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {

                    if (txtby == "Email")
                    {
                        var resumelist = (from c in db.Resume_Master where c.R_Email.Contains(txtvalue) select c).ToList();
                        if (resumelist != null)
                        {
                            responseModel.Message = "Data Found";
                            responseModel.Status = true;
                            responseModel.Data = resumelist;
                        }
                        else
                        {
                            responseModel.Message = "Data Not Found";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                        
                    }
                    if (txtby == "Name")
                    {
                        var resumelist = (from c in db.Resume_Master where c.R_Name.Contains(txtvalue) select c).ToList();
                        if (resumelist != null)
                        {
                            responseModel.Message = "Data Found";
                            responseModel.Status = true;
                            responseModel.Data = resumelist;
                        }
                        else
                        {
                            responseModel.Message = "Data Not Found";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }

                    }
                    if (txtby == "Number")
                    {
                        var resumelist = (from c in db.Resume_Master where c.R_Cnt.Contains(txtvalue) select c).ToList();
                        if (resumelist != null)
                        {
                            responseModel.Message = "Data Found";
                            responseModel.Status = true;
                            responseModel.Data = resumelist;
                        }
                        else
                        {
                            responseModel.Message = "Data Not Found";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }

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

        public ResponseModel POSTSearchResume(SearchResumeModel model)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var resumelist = (from c in db.Resume_Master where c.R_Email.Contains(model.byemail) && c.R_Name.Contains(model.byname) && c.R_Cnt.Contains(model.bynumber) && c.R_Skills.Contains(model.byskill) select c).ToList();
                    if (resumelist != null)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = resumelist;
                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
                        responseModel.Status = false;
                        responseModel.Data = null;
                    }
                }
                return responseModel;
            }
            catch (Exception ex) {
                responseModel.Message = "Exception";
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }
        }
    }
}
