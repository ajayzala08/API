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

                    var result = "{\"EmployeeCode\":" + res.E_Code + ",\"EmployeeName\":" + res.E_Fullname + ",\"Username\":" + res.E_UserName + "}";
                    //return Ok(res);
                    responseModel.Message = "Success";
                    responseModel.Status = true;
                    responseModel.Data = result;
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
