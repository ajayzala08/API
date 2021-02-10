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
    public class SalesUserController : ApiController
    {
        public ResponseModel GetSalesUser()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<UserModel> userModels = db.User_Master.Where(x => x.E_Role == "Sales" && x.E_Is_Active == 1 && x.E_Is_Delete == 0).Select(x => new UserModel
                    {
                        EId = x.E_Id,
                        ECode = x.E_Code,
                        EFullname = x.E_Fullname,
                        EGender = x.E_Gender,
                        EEmail = x.E_Email,
                        EDOB = x.E_DOB,
                        EContNo = x.E_ContNo,
                        EEmergencyContNo = x.E_EEmergencyContNo,
                        EUserName = x.E_UserName,
                        EPassword = x.E_Password,
                        EAddress = x.E_Address,
                        ECAddress = x.E_CAddress,
                        EPostal = (decimal)x.E_Postal,
                        EEtype = x.E_Etype,
                        ECompany_Name = x.E_Company_Name,
                        EDepartment = x.E_Department,
                        EDesignation = x.E_Designation,
                        ELocation = x.E_Location,
                        EOfferDate = x.E_OfferDate,
                        EJoinDate = x.E_JoinDate,
                        EExitDate = x.E_ExitDate,
                        ERole = x.E_Role,
                        EBloodGroup = x.E_BloodGroup,
                        EBankName = x.E_BankName,
                        EAccountNo = x.E_AccountNo,
                        EBranch = x.E_Branch,
                        EIFC_Code = x.E_IFC_Code,
                        EPFAccount = x.E_PF_Account,
                        EPAN_No = x.E_PAN_No,
                        EAadhar_no = x.E_Aadhar_no,
                        ESalary = x.E_Salary,
                        EPF = x.E_PF,
                        EPT = x.E_PT,
                        EESI = x.E_ESI,
                        EPFApply = x.E_PF_Apply == 1 ? true : false,
                        EPTApply = x.E_PT_Apply == 1 ? true : false,
                        EESIApply = x.E_ESI_Apply == 1 ? true : false,
                        Ephoto = x.E_Photo,
                        eactive = 1,
                        edelete = 0

                    }).ToList();
                    if (userModels.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Data = userModels;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
                        responseModel.Data = null;
                        responseModel.Status = false;
                    }
                    return responseModel;
                }

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
