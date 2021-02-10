using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ATSAPI.Models;

using System.Data.Entity;
using System.Web.Http.Cors;

namespace ATSAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Client_MasterController : ApiController
    {

      //  [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel PostClient_Master(ClientModel cmm)
        {
            ResponseModel responseModel = new ResponseModel();
            //try
            //{
                using (var db = new ATS2019_dbEntities())
                {
                    Boolean alreadyexists = db.Client_Master.Where(c => c.C_Name == cmm.cname).Any();
                    if (!alreadyexists)
                    {

                        Client_Master cm = new Client_Master();
                        cm.C_Name = cmm.cname;
                        cm.C_Address = cmm.caddress;
                        cm.C_Cont_Person1 = cmm.cperson1;
                        cm.C_Cont_Person1_Email = cmm.cemail1;
                        cm.C_Cont_Person1_No = cmm.ccnt1;
                        cm.C_Cont_Person2 = cmm.cperson2;
                        cm.C_Cont_Person2_Email = cmm.cemail2;
                        cm.C_Cont_Person2_No = cmm.ccnt2;
                        cm.C_Category = cmm.ccategory;
                        cm.C_Type = cmm.ctype;
                        cm.C_Segment = cmm.csegment;
                        cm.C_Margin_Type = cmm.cmargintype;
                        cm.C_Margin = cmm.cmargin;
                        cm.C_Is_Active = Convert.ToInt16("1");
                        cm.C_Is_Delete = Convert.ToInt16("0");
                        cm.C_Created_By = cmm.cuser.ToString();
                        cm.C_Created_On = System.DateTime.Now;
                        db.Client_Master.Add(cm);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Client created Successfully";
                            responseModel.Status = true;
                            responseModel.Data = null;
                            return responseModel;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Create Client";
                            responseModel.Status = false;
                            responseModel.Data = null;
                            return responseModel;
                        }


                    }
                    else
                    {
                        responseModel.Message = "Client Name Already Exists";
                        responseModel.Status = false;
                        responseModel.Data = null;
                        return responseModel;
                    }


                }
            //}
            //catch (Exception ex)
            //{
            //    responseModel.Message = "Exception";
            //    responseModel.Status = false;
            //    responseModel.Data = null;
            //    return responseModel;
            //}

        }
      //  [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel GETClient_Master()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<ClientModel> clientModels = db.Client_Master.Where(c => c.C_Is_Active == 1 && c.C_Is_Delete == 0).Select(x => new ClientModel
                    {
                        cid = x.C_id,
                        cname = x.C_Name,
                        caddress = x.C_Address,
                        cperson1 = x.C_Cont_Person1,
                        ccnt1 = x.C_Cont_Person1_No,
                        cemail1 = x.C_Cont_Person1_Email,
                        cperson2 = x.C_Cont_Person2,
                        ccnt2 = (decimal)x.C_Cont_Person2_No,
                        cemail2 = x.C_Cont_Person2_Email,
                        ccategory = x.C_Category,
                        ctype =x.C_Type,
                        csegment =x.C_Segment,
                        cmargin = (decimal)x.C_Margin,
                        cmargintype =x.C_Margin_Type,
                        cisactive =x.C_Is_Active,
                        cisdelete=x.C_Is_Delete,
                        cuser = x.C_Created_By,
                        cdate= (DateTime)x.C_Created_On
                        
                    }).ToList();
                    if (clientModels.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = clientModels;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
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

     //   [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseModel GETClient_Master(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var query = db.Client_Master.Where(c => c.C_Is_Active == 1 && c.C_Is_Delete == 0 && c.C_id == id).FirstOrDefault();
                    if(query != null)
                    {
                        ClientModel clientModel = new ClientModel()
                        {
                            cid = query.C_id,
                            cname = query.C_Name,
                            caddress = query.C_Address,
                            cperson1 = query.C_Cont_Person1,
                            ccnt1 = query.C_Cont_Person1_No,
                            cemail1 = query.C_Cont_Person1_Email,
                            cperson2 = query.C_Cont_Person2,
                            ccnt2 = (decimal)query.C_Cont_Person2_No,
                            cemail2 = query.C_Cont_Person2_Email,
                            ccategory = query.C_Category,
                            ctype = query.C_Type,
                            csegment = query.C_Segment,
                            cmargin = (decimal)query.C_Margin,
                            cmargintype = query.C_Margin_Type,
                            cisactive = query.C_Is_Active,
                            cisdelete = query.C_Is_Delete,
                            cuser = query.C_Created_By,
                            cdate =(DateTime)query.C_Created_On

                        };
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = clientModel;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
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

     //   [EnableCors(origins: "*", headers: "accept,content-type,origin,x-my-header", methods: "*")]
     
        public ResponseModel DELETEClient_Master(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var query = db.Client_Master.Where(c => c.C_Is_Active == 1 && c.C_Is_Delete == 0 && c.C_id == id).FirstOrDefault();
                    if (query != null)
                    {
                        query.C_Is_Delete = Convert.ToInt16("1");
                        db.Entry(query).State = EntityState.Modified;
                        db.SaveChanges();
                        responseModel.Message = "Clilent Deleted Successfully";
                        responseModel.Status = true;
                        responseModel.Data = null;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Delete Client";
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


   //     [EnableCors(origins: "*", headers: "*", methods: "*")]
        
        public ResponseModel PUTClient_Master(ClientModel cmm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    Client_Master cm = db.Client_Master.Where(c => c.C_Is_Active == 1 && c.C_Is_Delete == 0 && c.C_id == cmm.cid).FirstOrDefault();
                    if (cm != null)
                    {
                        
                        cm.C_Name = cmm.cname;
                        cm.C_Address = cmm.caddress;
                        cm.C_Cont_Person1 = cmm.cperson1;
                        cm.C_Cont_Person1_Email = cmm.cemail1;
                        cm.C_Cont_Person1_No = cmm.ccnt1;
                        cm.C_Cont_Person2 = cmm.cperson2;
                        cm.C_Cont_Person2_Email = cmm.cemail2;
                        cm.C_Cont_Person2_No = cmm.ccnt2;
                        cm.C_Category = cmm.ccategory;
                        cm.C_Type = cmm.ctype;
                        cm.C_Segment = cmm.csegment;
                        cm.C_Margin_Type = cmm.cmargintype;
                        cm.C_Margin = cmm.cmargin;
                        cm.C_Updated_By = cmm.cuser.ToString();
                        cm.C_Updated_On = System.DateTime.Now;
                       
                        db.Entry(cm).State = EntityState.Modified;
                        db.SaveChanges();
                        responseModel.Message = "Clilent Updated Successfully";
                        responseModel.Status = true;
                        responseModel.Data = null;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Update Client";
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
    }
}
