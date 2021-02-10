using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using ATSAPI;
using ATSAPI.Models;

namespace ATSAPI.Controllers
{
    public class User_MasterController : ApiController
    {
        private ATS2019_dbEntities db = new ATS2019_dbEntities();

        // GET: api/User_Master
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel GetUser_Master()
        {
            ResponseModel responseModel = new ResponseModel();
            List<UserModel> umlist = db.User_Master.Where(x => x.E_Is_Active == 1 && x.E_Is_Delete == 0).Select(p => new UserModel
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
                    eactive =(Int32)p.E_Is_Active,
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
            return responseModel;
        }

        //// GET: api/User_Master
        //public IEnumerable<User_Master> GetUser_Master()
        //{

        //    return db.User_Master.Where(x=>x.E_Is_Delete==0 && x.E_Is_Active==1).ToList();
        //}

        // GET: api/User_Master/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(User_Master))]
        
        public ResponseModel GetUser_Master(int id)
        {
            ResponseModel responseModel = new ResponseModel();

            var query = db.User_Master.Where(x=>x.E_Code==id && x.E_Is_Active == 1 && x.E_Is_Delete == 0).FirstOrDefault();
            if (query != null)
            {
                UserModel userModel = new UserModel()
                {
                    EId = query.E_Id,
                    ECode = query.E_Code,
                    EFullname = query.E_Fullname,
                    EGender = query.E_Gender,
                    EEmail = query.E_Email,
                    EDOB = query.E_DOB,
                    EContNo = query.E_ContNo,
                    EEmergencyContNo = query.E_EEmergencyContNo,
                    EUserName = query.E_UserName,
                    EPassword = query.E_Password,
                    EAddress = query.E_Address,
                    ECAddress = query.E_CAddress,
                    EPostal = (Decimal)query.E_Postal,
                    EEtype = query.E_Etype,
                    ECompany_Name = query.E_Company_Name,
                    EDepartment = query.E_Department,
                    EDesignation = query.E_Designation,
                    ELocation = query.E_Location,
                    EOfferDate = query.E_OfferDate,
                    EJoinDate = query.E_JoinDate,
                    ERole = query.E_Role,
                    EBloodGroup = query.E_BloodGroup,
                    EBankName = query.E_BankName,
                    EAccountNo = query.E_AccountNo,
                    EBranch = query.E_Branch,
                    EIFC_Code = query.E_IFC_Code,
                    EPFAccount = query.E_PF_Account,
                    EPAN_No = query.E_PAN_No,
                    EAadhar_no = query.E_Aadhar_no,
                    ESalary = query.E_Salary,
                    EPF = query.E_PF,
                    EPT = query.E_PT,
                    EESI = query.E_ESI,
                    EPFApply = query.E_PF_Apply.Value == 1 ? true : false,
                    EPTApply = query.E_PT_Apply.Value == 1 ? true : false,
                    EESIApply = query.E_ESI_Apply.Value == 1 ? true : false,
                    Ephoto = query.E_Photo,
                    eactive = (Int32)query.E_Is_Active,
                    edelete = (Int32)query.E_Is_Delete
                };
                responseModel.Data = userModel;
                responseModel.Message = "Record Found";
                responseModel.Status = true;
            }
            else
            {
                responseModel.Data = null;
                responseModel.Message = "Record Not Found";
                responseModel.Status = true;
            }
            return responseModel;
           
        }

        // PUT: api/User_Master/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(ResponseModel))]
        
        public ResponseModel PutUser_Master(UserModel umm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var exists = db.User_Master.Where(x => x.E_Code == umm.ECode).FirstOrDefault();
                if (exists != null)
                {
                    //User_Master finduser = new User_Master();
                    exists.E_Code = umm.ECode;
                    exists.E_Fullname = umm.EFullname;
                    exists.E_Gender = umm.EGender;
                    exists.E_Email = umm.EEmail;
                    exists.E_DOB = umm.EDOB;
                    exists.E_ContNo = umm.EContNo;
                    exists.E_EEmergencyContNo = umm.EEmergencyContNo;
                    exists.E_UserName = umm.EUserName;
                    exists.E_Password = umm.EPassword;
                    exists.E_Address = umm.EAddress;
                    exists.E_CAddress = umm.ECAddress;
                    exists.E_Postal = umm.EPostal;
                    exists.E_Etype = umm.EEtype;
                    exists.E_Company_Name = umm.ECompany_Name;
                    exists.E_Department = umm.EDepartment;
                    exists.E_Designation = umm.EDesignation;
                    exists.E_Location = umm.ELocation;
                    exists.E_OfferDate = umm.EOfferDate;
                    exists.E_JoinDate = umm.EJoinDate;
                    exists.E_Role = umm.ERole;
                    exists.E_BloodGroup = umm.EBloodGroup;
                    exists.E_BankName = umm.EBankName;
                    exists.E_AccountNo = umm.EAccountNo;
                    exists.E_Branch = umm.EBranch;
                    exists.E_IFC_Code = umm.EIFC_Code;
                    exists.E_PF_Account = umm.EPFAccount;
                    exists.E_PAN_No = umm.EPAN_No;
                    exists.E_Aadhar_no = umm.EAadhar_no;
                    exists.E_Salary = umm.ESalary;
                    exists.E_PF = umm.EPF;
                    exists.E_PT = umm.EPT;
                    exists.E_ESI = umm.EESI;
                    exists.E_PF_Apply = umm.EPFApply == true ? Convert.ToInt32("1") : Convert.ToInt32("0");
                    exists.E_PT_Apply = umm.EPTApply == true ? Convert.ToInt32("1") : Convert.ToInt32("0");
                    exists.E_ESI_Apply = umm.EESIApply == true ? Convert.ToInt32("1") : Convert.ToInt32("0");
                    //exists.E_Updated_By = "Username";
                    exists.E_Created_On = System.DateTime.Now;
                    exists.E_Photo = umm.Ephoto;
                    db.Entry(exists).State = EntityState.Modified;
                    db.SaveChanges();
                    responseModel.Message = "User Updated Successfully.";
                    responseModel.Status = true;
                    responseModel.Data = null;
                   
                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Message = "Exception.";
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }
        }

        // POST: api/User_Master
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(ResponseModel))]
        
        public ResponseModel PostUser_Master(UserModel umm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var exists = db.User_Master.Where(x => x.E_Code == umm.ECode).FirstOrDefault();
                if (exists != null)
                {
                    responseModel.Message = "Employee Code Already Exists";
                    responseModel.Status = false;
                    responseModel.Data = null;

                }
                else
                {
                    User_Master finduser = new User_Master();
                    finduser.E_Code = umm.ECode;
                    finduser.E_Fullname = umm.EFullname;
                    finduser.E_Gender = umm.EGender;
                    finduser.E_Email = umm.EEmail;
                    finduser.E_DOB = umm.EDOB;
                    finduser.E_ContNo = umm.EContNo;
                    finduser.E_EEmergencyContNo = umm.EEmergencyContNo;
                    finduser.E_UserName = umm.EUserName;
                    finduser.E_Password = umm.EPassword;
                    finduser.E_Address = umm.EAddress;
                    finduser.E_CAddress = umm.ECAddress;
                    finduser.E_Postal = umm.EPostal;
                    finduser.E_Etype = umm.EEtype;
                    finduser.E_Company_Name = umm.ECompany_Name;
                    finduser.E_Department = umm.EDepartment;
                    finduser.E_Designation = umm.EDesignation;
                    finduser.E_Location = umm.ELocation;
                    finduser.E_OfferDate = umm.EOfferDate;
                    finduser.E_JoinDate = umm.EJoinDate;
                    finduser.E_Role = umm.ERole;
                    finduser.E_BloodGroup = umm.EBloodGroup;
                    finduser.E_BankName = umm.EBankName;
                    finduser.E_AccountNo = umm.EAccountNo;
                    finduser.E_Branch = umm.EBranch;
                    finduser.E_IFC_Code = umm.EIFC_Code;
                    finduser.E_PF_Account = umm.EPFAccount;
                    finduser.E_PAN_No = umm.EPAN_No;
                    finduser.E_Aadhar_no = umm.EAadhar_no;
                    finduser.E_Salary = umm.ESalary;
                    finduser.E_PF = umm.EPF;
                    finduser.E_PT = umm.EPT;
                    finduser.E_ESI = umm.EESI;
                    finduser.E_PF_Apply = umm.EPFApply == true ? Convert.ToInt32("1") : Convert.ToInt32("0");
                    finduser.E_PT_Apply = umm.EPTApply == true ? Convert.ToInt32("1") : Convert.ToInt32("0");
                    finduser.E_ESI_Apply = umm.EESIApply == true ? Convert.ToInt32("1") : Convert.ToInt32("0");
                    //finduser.E_Updated_By = "Username";
                    finduser.E_Created_On = System.DateTime.Now;
                    finduser.E_Is_Active = 1;
                    finduser.E_Is_Delete = 0;
                    finduser.E_Photo = umm.Ephoto;
                    db.User_Master.Add(finduser);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "User Created Successfully.";
                        responseModel.Status = true;
                        responseModel.Data = null;
                    }
                    else
                    {
                        responseModel.Message = "Failed To Create User.";
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

        //// DELETE: api/User_Master/5
        //[ResponseType(typeof(User_Master))]
        //public IHttpActionResult DeleteUser_Master(int id)
        //{
        //    User_Master user_Master = db.User_Master.Find(id);
        //    if (user_Master == null)
        //    {
        //        return NotFound();
        //    }

        //    db.User_Master.Remove(user_Master);
        //    db.SaveChanges();

        //    return Ok(user_Master);
        //}
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel DeleteUser(DeleteUserModel deleteUserModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                decimal empcode = Convert.ToDecimal(deleteUserModel.ecode);
                User_Master user_Master = db.User_Master.Where(x => x.E_Code == empcode).FirstOrDefault();
                user_Master.E_ExitDate = deleteUserModel.exitdt;
                user_Master.E_Is_Active = 1;
                user_Master.E_Is_Delete = 1;
                db.Entry(user_Master).State = EntityState.Modified;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    responseModel.Message = "User Deleted Successfully";
                    responseModel.Status = true;
                    responseModel.Data = null;
                }
                else
                {
                    responseModel.Message = "Failed To Delete User";
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool User_MasterExists(int id)
        {
            return db.User_Master.Count(e => e.E_Id == id) > 0;
        }
    }
}