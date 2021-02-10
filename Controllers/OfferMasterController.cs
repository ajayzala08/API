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
    public class OfferMasterController : ApiController
    {
        public ResponseModel GetOfferMaster(decimal requirementid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<OfferModel> offerModels = (from c in db.Offer_Master
                                                    where c.O_Jid == requirementid
                                                    join
                                                        d in db.Resume_Master on c.O_Rid equals d.R_Id
                                                    join
                                                     e in db.Requirement_Master on d.R_Jid equals e.J_Id
                                                    join
                                                    f in db.Client_Master on e.J_Client_Id equals f.C_id
                                                    select new { c, d, e, f }).Select(x => new OfferModel
                                                    {
                                                        offerid = x.c.O_Id,
                                                        interviewid = x.c.O_Iid,
                                                        resumeid = x.c.O_Rid,
                                                        requirementid = x.c.O_Jid,
                                                        name = x.c.O_Fullname,
                                                        clientname = x.f.C_Name,
                                                        skill = x.e.J_Skill,
                                                        location = x.e.J_Location,
                                                        type = x.e.J_Employment_Type,
                                                        status = x.c.O_Status,
                                                        recruitername = x.c.O_Recruiter,
                                                        recruiterdate = (DateTime)x.c.O_Recuiter_Date

                                                    }).ToList();
                    if (offerModels.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Data = offerModels;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Data Not Found";
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

        public ResponseModel PutOfferMaster(OfferModel offerModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try {
                using (var db = new ATS2019_dbEntities())
                {
                    Offer_Master master = db.Offer_Master.Where(x => x.O_Id == offerModel.offerid).FirstOrDefault();
                    if (master != null)
                    {
                        master.O_Fullname = offerModel.name;
                        var dobvalue = offerModel.dob.Year;

                        if (dobvalue >1)
                        {
                            master.O_DOB = offerModel.dob;
                        }
                        master.O_Gender = offerModel.gender;
                        master.O_MaritalStatus = offerModel.maritalstatus;
                        master.O_Cur_Add = offerModel.currentaddress;
                        master.O_Perm_Add = offerModel.permanantaddress;
                        master.O_Country = offerModel.country;
                        master.O_Email = offerModel.email;
                        master.O_Cur_Des = offerModel.currentdesignation;
                        master.O_Off_Des = offerModel.offerdesignation;
                        master.O_Skill = offerModel.skill;
                        master.O_Tot_Exp = offerModel.totalexperience;
                        master.O_Rel_Exp = offerModel.relavantexperience;
                        master.O_Mobile = offerModel.mobileno;
                        master.O_Alt_Mobile = offerModel.alternativno;
                        master.O_Pan = offerModel.panno;
                        master.O_Client = offerModel.clientname;
                        master.O_Location = offerModel.location;
                        master.O_CTC = offerModel.ctc;
                        master.O_ECTC = offerModel.ectc;
                        master.O_BR = offerModel.billrate;
                        master.O_PR = offerModel.payrate;
                        master.O_GP = offerModel.grossprofit;
                        master.O_GPM = offerModel.grossprofitmargin;
                        master.O_Sel_Date = offerModel.selectiondate;
                        master.O_Off_Date = offerModel.offerdate;
                        master.O_Join_Date = offerModel.joindate;
                        master.O_Emergency = offerModel.emergencyno;
                        master.O_Account_No = offerModel.bankaccountno;
                        master.O_Bank_Name = offerModel.bankname;
                        master.O_Branch = offerModel.branch;
                        master.O_IFCI = offerModel.ifci;
                        master.O_Aadhar = Convert.ToDecimal(offerModel.addarno);
                        master.O_Status = offerModel.status;
                        master.O_Note = offerModel.note;
                        master.O_Type = offerModel.type;
                        master.O_Teamlead = offerModel.teamleadname;
                        master.O_Teamlead_Date = System.DateTime.Now;
                        db.Entry(master).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Offer Details Added Successfully.";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Update Offer Details";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
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

        public ResponseModel PutOfferMaster(decimal offerid, string status)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Offer_Master master = db.Offer_Master.Where(x => x.O_Id == offerid).FirstOrDefault();
                    if (master != null)
                    {
                        master.O_Status = status;
                        db.Entry(master).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Status Updated Successfully";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Update Status";
                            responseModel.Status = false;
                            responseModel.Data = null;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
                        responseModel.Status = false;
                        responseModel.Data = null;
                    }
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Message = "Exception";
                responseModel.Status = true;
                responseModel.Data = false;
                return responseModel;
            }

        }
    }
}
