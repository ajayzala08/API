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
    public class SalesClientMasterController : ApiController
    {
        public ResponseModel PostSalesClient(SalesClientModel salesClientModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var alreadyassign = db.SalesClient_Master.Where(x => x.SC_UID == salesClientModel.uid && x.SC_CID == salesClientModel.cid).FirstOrDefault();
                    if (alreadyassign == null)
                    {
                        SalesClient_Master salesClient_Master = new SalesClient_Master();
                        salesClient_Master.SC_UID = salesClientModel.uid;
                        salesClient_Master.SC_CID = salesClientModel.cid;
                        salesClient_Master.CreatedBy = salesClientModel.username;
                        salesClient_Master.CreatedOn = salesClientModel.createdatetime;
                        db.SalesClient_Master.Add(salesClient_Master);
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Inserted Successfully.";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Insert Record";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Record Already Exists";
                        responseModel.Data = null;
                        responseModel.Status = false;
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

        public ResponseModel DeleteSalesClient(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClient_Master salesClient_Master = db.SalesClient_Master.Where(x => x.SC_Id == id).FirstOrDefault();
                    if (salesClient_Master != null)
                    {
                        db.Entry(salesClient_Master).State = EntityState.Deleted;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Deleted Successfully.";
                            responseModel.Status = true;
                            responseModel.Data = null;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Delete Record";
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

        public ResponseModel GetSalesClient()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<SalesClientViewModel> salesClientViewModels = (from c in db.SalesClient_Master
                                                                        join
                                                                            d in db.User_Master on c.SC_UID equals d.E_Code
                                                                        join
                                                                            e in db.Client_Master on c.SC_CID equals e.C_id
                                                                        select new { c, d, e }
                                                                               ).Select(x => new SalesClientViewModel
                                                                               {
                                                                                   id = x.c.SC_Id,
                                                                                   name = x.d.E_Fullname,
                                                                                   clientname = x.e.C_Name
                                                                               }).ToList();
                    if (salesClientViewModels.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = salesClientViewModels;
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

        public ResponseModel GetSalesClient(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClientViewModel salesClient = (from c in db.SalesClient_Master where c.SC_Id==id
                                                        join
                                                            d in db.User_Master on c.SC_UID equals d.E_Code
                                                        join
                                                            e in db.Client_Master on c.SC_CID equals e.C_id
                                                        select new { c, d, e }
                                                                               ).Select(x => new SalesClientViewModel
                                                                               {
                                                                                   id = x.c.SC_Id,
                                                                                   name = x.d.E_Fullname,
                                                                                   clientname = x.e.C_Name
                                                                               }).FirstOrDefault();
                    if (salesClient != null)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Data = salesClient;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
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

        public ResponseModel PutSalesClient(SalesClientModel salesClientModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClient_Master master = db.SalesClient_Master.Where(x => x.SC_Id == salesClientModel.id).FirstOrDefault();
                    if (master != null)
                    {
                        master.SC_CID = salesClientModel.cid;
                        master.SC_UID = salesClientModel.uid;
                        master.CreatedBy = salesClientModel.username;
                        master.CreatedOn = System.DateTime.Now;
                        db.Entry(master).State = EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Updated Successfully";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Update Record";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Record Not Found";
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
