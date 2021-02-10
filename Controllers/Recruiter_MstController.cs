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
    public class Recruiter_MstController : ApiController
    {
        public ResponseModel GetUserRole_Master()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<UserModel> umlist = db.User_Master.Where(x => x.E_Is_Active == 1 && x.E_Is_Delete == 0 && x.E_Role== "Recruiter").Select(p => new UserModel
                    {
                        EId = p.E_Id,
                        ECode = p.E_Code,
                        EFullname = p.E_Fullname,
                        EGender = p.E_Gender,
                        EEmail = p.E_Email,
                        EDOB = p.E_DOB,
                        EContNo = p.E_ContNo,
                        EEmergencyContNo = p.E_EEmergencyContNo,
                        EUserName = p.E_UserName,
                        EPassword = p.E_Password,
                        EAddress = p.E_Address,
                        ECAddress = p.E_CAddress,
                        EPostal = (Decimal)p.E_Postal,
                        EEtype = p.E_Etype,
                        ECompany_Name = p.E_Company_Name,
                        EDepartment = p.E_Department,
                        EDesignation = p.E_Designation,
                        ELocation = p.E_Location,
                        EOfferDate = p.E_OfferDate,
                        EJoinDate = p.E_JoinDate,
                        ERole = p.E_Role,
                        EBloodGroup = p.E_BloodGroup,
                        EBankName = p.E_BankName,
                        EAccountNo = p.E_AccountNo,
                        EBranch = p.E_Branch,
                        EIFC_Code = p.E_IFC_Code,
                        EPFAccount = p.E_PF_Account,
                        EPAN_No = p.E_PAN_No,
                        EAadhar_no = p.E_Aadhar_no,
                        ESalary = p.E_Salary,
                        EPF = p.E_PF,
                        EPT = p.E_PT,
                        EESI = p.E_ESI,
                        EPFApply = p.E_PF_Apply.Value == 1 ? true : false,
                        EPTApply = p.E_PT_Apply.Value == 1 ? true : false,
                        EESIApply = p.E_ESI_Apply.Value == 1 ? true : false,
                        Ephoto = p.E_Photo,
                        eactive = (Int32)p.E_Is_Active,
                        edelete = (Int32)p.E_Is_Delete
                    }).ToList();
                    if (umlist.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = umlist;
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
                        responseModel.Status = false;
                        responseModel.Data = umlist;
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
    }
}
