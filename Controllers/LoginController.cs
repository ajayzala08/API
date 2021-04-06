using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ATSAPI.Models;


namespace ATSAPI.Controllers
{

    public class LoginController : ApiController
    {
        ATS2019_dbEntities db = new ATS2019_dbEntities();
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel CheckLogin(loginmodel lm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                User_Master res = db.User_Master.Where(x => x.E_UserName == lm.username && x.E_Password == lm.password && x.E_Is_Active == 1 && x.E_Is_Delete == 0).FirstOrDefault();
                if (res != null)
                {
                    string firstname = string.Empty, lastname = string.Empty;
                    string[] names = res.E_Fullname.ToString().Split(' ');
                    if (names.Length > 2)
                    {
                        firstname = names[0];
                        lastname = names[2];
                    }
                    else
                    {
                        firstname = names[0];
                        lastname = names[1];
                    }

                    LoginResponseModel login = new LoginResponseModel();
                    login.EmployeeCode = Convert.ToDecimal(res.E_Code);
                    login.Firstname = firstname;
                    login.Lastname = lastname;
                    login.Username = res.E_UserName;
                    login.Role = res.E_Role;
                    login.token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJjb2RlcnRoZW1lcyIsImlhdCI6MTU4NzM1NjY0OSwiZXhwIjoxOTAyODg5NDQ5LCJhdWQiOiJjb2RlcnRoZW1lcy5jb20iLCJzdWIiOiJzdXBwb3J0QGNvZGVydGhlbWVzLmNvbSIsImxhc3ROYW1lIjoiVGVzdCIsIkVtYWlsIjoic3VwcG9ydEBjb2RlcnRoZW1lcy5jb20iLCJSb2xlIjoiQWRtaW4iLCJmaXJzdE5hbWUiOiJTaHJleXUifQ.D-isMYoGH6Ah4i_dHxplgJNGtXTLEqZYvha_ULSJRFU";

                    //var result = "{\"EmployeeCode\":" + res.E_Code + ",\"Firstname\":" + firstname + ",\"Lastname\":" + lastname + ",\"Username\":" + res.E_UserName + ",\"Role\":" + res.E_Role + "}";
                    //return Ok(res);
                    var checkexpiry = db.PasswordExpiry_Master.Where(x => x.Employeecode == res.E_Code).FirstOrDefault();
                    if (checkexpiry != null)
                    {
                        TimeSpan span = checkexpiry.ExpirtyDate - System.DateTime.Now;
                        string msg = string.Empty;
                        if (span.Days > 5)
                        {
                            responseModel.Message = "Success.";
                            responseModel.Status = true;
                            responseModel.Data = login;

                        }
                        else if (span.Days > 0 && span.Days <= 5)
                        {
                            responseModel.Message = "Success." + " Your Password Expire in " + span.Days.ToString() + " Days.";
                            responseModel.Status = true;
                            responseModel.Data = login;

                        }
                        else
                        {
                            responseModel.Message = "Your Password Is Expire";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Success. Login First Time";
                        responseModel.Status = true;
                        responseModel.Data = login;
                    }
                }
                else
                {
                    // return Ok(res);
                    responseModel.Message = "Fail";
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
    }
}
