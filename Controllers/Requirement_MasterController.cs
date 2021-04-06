using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ATSAPI.Models;
using ATSAPI;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    public class Requirement_MasterController : ApiController
    {
        ATS2019_dbEntities db = new ATS2019_dbEntities();

        // GET: api/Requirement_Master
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel GetRequirement_Master()
        {
            ResponseModel responseModel = new ResponseModel();
            List<RequirementModel>
            requirementModel = (from x in db.Requirement_Master where x.J_Id<=30
                                 join y in db.Client_Master on x.J_Client_Id equals y.C_id where x.J_Is_Active==1 && x.J_Is_Delete==0 select new { x, y }).Select(x => new RequirementModel {
                                     jid = x.x.J_Id,
                                     jobcode = x.x.J_Code,
                                     jclientid = x.x.J_Client_Id,
                                     jskill = x.x.J_Skill,
                                     jposition = x.x.J_Position,
                                     jlocation = x.x.J_Location,
                                     jendclient = x.x.J_EndClient,
                                     jassignuser = x.x.J_AssignUser,
                                     jtotmin = (float)x.x.J_Tot_Min_Exp,
                                     jtotmax = (float)x.x.J_Tot_Max_Exp,
                                     jrelmax = (float)x.x.J_Rel_Max_Exp,
                                     jrelmin = (float)x.x.J_Rel_Min_Exp,
                                     jbillrate = (Decimal)x.x.J_Bill_Rate,
                                     jpayrate = (Decimal)x.x.J_Pay_Rate,
                                     jcategory = x.x.J_Category,
                                     jtype = x.x.J_Type,
                                     jemployementtyp = x.x.J_Employment_Type,
                                     jpoc = x.x.J_POC,
                                     jpocno = (Decimal)x.x.J_POC_No,
                                     jjd = x.x.J_JD,
                                     jstatus = x.x.J_Status,
                                     jmandatoryskill = x.x.J_MandatorySkill,
                                     jclientname = x.y.C_Name,
                                     jcreatedby = x.x.J_CreatedBy,
                                     jcreatedon = x.x.J_CreatedOn.Value

                                 }).ToList();


            //List<RequirementModel>
            //requirementModel = db.Requirement_Master.Where(x=>x.J_Is_Active==1 && x.J_Is_Delete==0).Select(x => new RequirementModel
            //{
            //    jid = x.J_Id,
            //    jobcode = x.J_Code,
            //    jclientid = x.J_Client_Id,
            //    jskill = x.J_Skill,
            //    jposition = x.J_Position,
            //    jlocation = x.J_Location,
            //    jendclient = x.J_EndClient,
            //    jassignuser = x.J_AssignUser,
            //    jtotmin = (float)x.J_Tot_Min_Exp,
            //    jtotmax = (float)x.J_Tot_Max_Exp,
            //    jrelmax = (float)x.J_Rel_Max_Exp,
            //    jrelmin = (float)x.J_Rel_Min_Exp,
            //    jbillrate = (Decimal)x.J_Bill_Rate,
            //    jpayrate = (Decimal)x.J_Pay_Rate,
            //    jcategory = x.J_Category,
            //    jtype = x.J_Type,
            //    jemployementtyp = x.J_Employment_Type,
            //    jpoc = x.J_POC,
            //    jpocno = (Decimal)x.J_POC_No,
            //    jjd = x.J_JD,
            //    jstatus = x.J_Status,
            //    jmandatoryskill = x.J_MandatorySkill


            //}).ToList();
            
            if (requirementModel.Count() > 0)
            {
                responseModel.Message = "Data Found";
                responseModel.Status = true;
                responseModel.Data = requirementModel.OrderByDescending(x=>x.jid);
                return responseModel;
            }
            else
            {
                responseModel.Message = "No Record Found";
                responseModel.Status = true;
                responseModel.Data = requirementModel;
                return responseModel;
            }
        }

        // GET: api/Requirement_Master/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(Requirement_Master))]
        
        public ResponseModel GetRequirement_Master(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var query = (from x in db.Requirement_Master
                             join y in db.Client_Master on x.J_Client_Id equals y.C_id
                             where x.J_Is_Active == 1 && x.J_Is_Delete == 0 && x.J_Id==id
                             select new { x, y }).FirstOrDefault();
                //var query = db.Requirement_Master.Where(x => x.J_Is_Active == 1 && x.J_Is_Delete == 0 && x.J_Code == id).FirstOrDefault();
                if (query != null)
                { 
                RequirementModel requirementModel = new RequirementModel(){
                    jid = query.x.J_Id,
                    jobcode = query.x.J_Code,
                    jclientid = query.x.J_Client_Id,
                    jskill = query.x.J_Skill,
                    jposition = query.x.J_Position,
                    jlocation = query.x.J_Location,
                    jendclient = query.x.J_EndClient,
                    jassignuser = query.x.J_AssignUser,
                    jtotmin = (float)query.x.J_Tot_Min_Exp,
                    jtotmax = (float)query.x.J_Tot_Max_Exp,
                    jrelmax = (float)query.x.J_Rel_Max_Exp,
                    jrelmin = (float)query.x.J_Rel_Min_Exp,
                    jbillrate = (Decimal)query.x.J_Bill_Rate,
                    jpayrate = (Decimal)query.x.J_Pay_Rate,
                    jcategory = query.x.J_Category,
                    jtype = query.x.J_Type,
                    jemployementtyp = query.x.J_Employment_Type,
                    jpoc = query.x.J_POC,
                    jpocno = (Decimal)query.x.J_POC_No,
                    jjd = query.x.J_JD,
                    jstatus = query.x.J_Status,
                    jmandatoryskill = query.x.J_MandatorySkill,
                    jclientname = query.y.C_Name,
                    jcreatedby= query.x.J_CreatedBy,
                    jcreatedon =query.x.J_CreatedOn.Value
                };
            
                
            
                    responseModel.Message = "Data Found";
                    responseModel.Status = true;
                    responseModel.Data = requirementModel;
                    return responseModel;
                }
                else
                {
                    responseModel.Message = "No Record Found";
                    responseModel.Status = true;
                    responseModel.Data = query;
                    return responseModel;
                }
            }
            catch (Exception ex)
            {
                
                responseModel.Message = "Exception";
                responseModel.Status = true;
                responseModel.Data = null;
                return responseModel;
            }
        }

        // PUT: api/Requirement_Master/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(void))]
        
        public ResponseModel PutRequirement_Master(RequirementModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            var req = db.Requirement_Master.Where(c => c.J_Code == rm.jobcode).FirstOrDefault();
            if (req != null)
            {
                req.J_Code = rm.jobcode;
                req.J_Client_Id = rm.jclientid;
                req.J_Skill = rm.jskill;
                req.J_Position = rm.jposition;
                req.J_Location = rm.jlocation;
                req.J_EndClient = rm.jendclient;
                req.J_Tot_Min_Exp = rm.jtotmin;
                req.J_Tot_Max_Exp = rm.jtotmax;
                req.J_Rel_Min_Exp = rm.jrelmin;
                req.J_Rel_Max_Exp = rm.jrelmax;
                req.J_Bill_Rate = rm.jbillrate;
                req.J_Pay_Rate = rm.jpayrate;
                req.J_Category = rm.jcategory;
                req.J_Type = rm.jtype;
                req.J_Employment_Type = rm.jemployementtyp;
                req.J_POC = rm.jpoc;
                req.J_POC_No = rm.jpocno;
                req.J_JD = rm.jjd;
                req.J_MandatorySkill = rm.jmandatoryskill;
                req.J_UpdatedBy = rm.jcreatedby;
                req.J_UpdatedOn = System.DateTime.Now;
                db.Entry(req).State = EntityState.Modified;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    responseModel.Message = "Requirement Updated Successfully";
                    responseModel.Status = true;
                    responseModel.Data = null;
                }
                else
                {
                    responseModel.Message = "Failed To Update Requirement";
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
            return responseModel; 
        }

        // POST: api/Requirement_Master
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(ResponseModel))]
        
        public ResponseModel PostRequirement_Master(RequirementModel rmm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (db.Requirement_Master.Where(x => x.J_Code == rmm.jobcode).Count() > 0)
                {
                    responseModel.Message = "Job Code Already Exists";
                    responseModel.Status = false;
                    responseModel.Data = null;
                    return responseModel;
                }
                else
                {
                    Requirement_Master rm = new Requirement_Master();
                    rm.J_Code = rmm.jobcode;
                    rm.J_Client_Id = rmm.jclientid;
                    rm.J_Skill = rmm.jskill;
                    rm.J_Position = rmm.jposition;
                    rm.J_Location = rmm.jlocation;
                    rm.J_EndClient = rmm.jendclient;
                    rm.J_AssignUser = rmm.jassignuser;
                    rm.J_Tot_Max_Exp = rmm.jtotmax;
                    rm.J_Tot_Min_Exp = rmm.jtotmin;
                    rm.J_Rel_Max_Exp = rmm.jrelmax;
                    rm.J_Rel_Min_Exp = rmm.jrelmin;
                    rm.J_Bill_Rate = rmm.jbillrate;
                    rm.J_Pay_Rate = rmm.jpayrate;
                    rm.J_Category = rmm.jcategory;
                    rm.J_Type = rmm.jtype;
                    rm.J_Employment_Type = rmm.jemployementtyp;
                    rm.J_POC = rmm.jpoc;
                    rm.J_POC_No = 0;
                    rm.J_JD = rmm.jjd;
                    rm.J_Status = "Create";
                    rm.J_MandatorySkill = Convert.ToString(rmm.jmandatoryskill);
                    rm.J_Is_Active = Convert.ToInt16("1");
                    rm.J_Is_Delete = Convert.ToInt16("0");
                    rm.J_CreatedOn = System.DateTime.Now;
                    rm.J_CreatedBy = rmm.jcreatedby;
                    db.Requirement_Master.Add(rm);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Requirement created Successfully";
                        responseModel.Status = true;
                        responseModel.Data = null;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Create Requirement";
                        responseModel.Status = false;
                        responseModel.Data = null;
                        return responseModel;
                    }
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

        // DELETE: api/Requirement_Master/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(Requirement_Master))]
        
        public ResponseModel DeleteRequirement_Master(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Requirement_Master requirement_Master = db.Requirement_Master.Where(x => x.J_Id == id).FirstOrDefault();
                if (requirement_Master == null)
                {
                    responseModel.Message = "Record Not Found";
                    responseModel.Status = false;
                    responseModel.Data = null;

                }
                else
                {
                    requirement_Master.J_Is_Delete = 1;
                    db.Entry(requirement_Master).State = EntityState.Modified;
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Requirement Deleted Successfully.";
                        responseModel.Status = true;
                        responseModel.Data = null;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Delete Requirement.";
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Requirement_MasterExists(decimal id)
        {
            return db.Requirement_Master.Count(e => e.J_Id == id) > 0;
        }
    }
}