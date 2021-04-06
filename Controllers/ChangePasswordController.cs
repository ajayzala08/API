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
    public class ChangePasswordController : ApiController
    {
        public ResponseModel PutChangePassword(Changepasswordmodel model)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var checkcredential = db.User_Master.Where(x => x.E_UserName == model.username && x.E_Password == model.oldpwd).FirstOrDefault();
                    if (checkcredential != null)
                    {
                        checkcredential.E_Password = model.newpwd;
                        db.Entry(checkcredential).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            DateTime lastupdate = System.DateTime.Now;
                            DateTime expirydate = System.DateTime.Now.AddDays(45);
                            var pwdexpiry = db.PasswordExpiry_Master.Where(x => x.Employeecode == checkcredential.E_Code).FirstOrDefault();
                            if (pwdexpiry != null)
                            {
                                pwdexpiry.LastChangeDate = lastupdate;
                                pwdexpiry.ExpirtyDate = expirydate;
                                db.Entry(pwdexpiry).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                PasswordExpiry_Master master = new PasswordExpiry_Master();
                                master.Employeecode =(decimal)checkcredential.E_Code;
                                master.LastChangeDate = lastupdate;
                                master.ExpirtyDate = expirydate;
                                db.PasswordExpiry_Master.Add(master);
                                db.SaveChanges();
                            }
                            responseModel.Message = "Password Change Successfully.";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Change Password.";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Old Password Not Match.";
                        responseModel.Data = null;
                        responseModel.Status = false;
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
