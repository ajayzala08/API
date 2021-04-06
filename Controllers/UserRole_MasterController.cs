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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserRole_MasterController : ApiController
    {
        public ResponseModel PostUserRole_Master(userrolemodel _userrolemodel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Boolean ifexists = db.Role_Master.Where(c => c.R_ecode == _userrolemodel.uid && c.R_role == _userrolemodel.role).Any();
                    if (!ifexists)
                    {
                        Role_Master rm = new Role_Master();
                        rm.R_ecode = _userrolemodel.uid;
                        rm.R_role = _userrolemodel.role.ToString();
                        db.Role_Master.Add(rm);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Role Created Successfully.";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Create Role";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Role Already Exists";
                        responseModel.Status = false;
                        responseModel.Data = null;

                    }
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


        public ResponseModel GetUserRole_Master()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<UserRoleViewModel> userroles = (from c in db.Role_Master join d in db.User_Master on c.R_ecode equals d.E_Code select new { c, d }).Select(x => new UserRoleViewModel
                    {
                        id = x.c.R_id,
                        username = x.d.E_Fullname,
                        role = x.c.R_role
                    }).ToList();
                    if (userroles.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = userroles;
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
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }
        }

        public ResponseModel DeleteUserRole_Master(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var finduserrole = db.Role_Master.Where(m => m.R_id == id).FirstOrDefault();
                    db.Entry(finduserrole).State = EntityState.Deleted;
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Role Deleted Successfully";
                        responseModel.Status = true;
                        responseModel.Data = null;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Delete Role";
                        responseModel.Status = false;
                        responseModel.Data = null;
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

        public ResponseModel GetUserRole_Master(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<UserRoleViewModel> userroles = (from c in db.Role_Master where c.R_ecode==id join d in db.User_Master on c.R_ecode equals d.E_Code select new { c, d }).Select(x => new UserRoleViewModel
                    {
                        id = x.c.R_id,
                        username = x.d.E_Fullname,
                        role = x.c.R_role
                    }).ToList();
                    if (userroles.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Status = true;
                        responseModel.Data = userroles;
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
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Data = null;
                responseModel.Status = true;
                return responseModel;
            }
        }
    }
}
