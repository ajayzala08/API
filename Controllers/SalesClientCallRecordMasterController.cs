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
    public class SalesClientCallRecordMasterController : ApiController
    {
        public ResponseModel PostSalesClientCallRecordMaster(SalesClientCallModel salesClientCallModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClientCall_Master call_Master = new SalesClientCall_Master();
                    call_Master.scr_date = salesClientCallModel.dt;
                    call_Master.scr_client = salesClientCallModel.client;
                    call_Master.scr_poc = salesClientCallModel.poc;
                    call_Master.scr_agenda = salesClientCallModel.agenda;
                    call_Master.scr_createdby = salesClientCallModel.username;
                    call_Master.scr_creaton = System.DateTime.Now;
                    db.SalesClientCall_Master.Add(call_Master);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        responseModel.Message = "Record Saved Successfully";
                        responseModel.Status = true;
                        responseModel.Data = null;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Save Record";
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

        public ResponseModel GetSalesClientCallRecordMaster(string username)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<SalesClientCallModel> salesClientCallModels;
                    var userroles = (from c in db.User_Master
                                     where c.E_UserName == username
                                     join d in db.Role_Master on c.E_Code equals d.R_ecode
                                     select d.R_role
                                     ).ToList();
                    if (userroles.Contains("Admin"))
                    {
                        salesClientCallModels = db.SalesClientCall_Master.Select(x => new SalesClientCallModel
                        {
                            id = x.scr_id,
                            dt = x.scr_date,
                            client = x.scr_client,
                            agenda = x.scr_agenda,
                            poc = x.scr_poc,
                            username = x.scr_createdby
                        }).ToList();
                    }
                    else
                    {
                        salesClientCallModels = (from e in db.SalesClientCall_Master
                                                 where e.scr_createdby == username
                                                 join f in db.Client_Master on e.scr_client equals f.C_Name
                                                 join g in db.User_Master on e.scr_createdby equals g.E_UserName
                                                 join h in db.SalesClient_Master on g.E_Code equals h.SC_UID
                                                 select new { e, f, g, h }
                                                ).Select(x => new SalesClientCallModel
                                                {
                                                    id = x.e.scr_id,
                                                    dt = x.e.scr_date,
                                                    client = x.e.scr_client,
                                                    agenda = x.e.scr_agenda,
                                                    poc = x.e.scr_poc,
                                                    username = x.e.scr_createdby
                                                }).Distinct().ToList();
                    }

                    if (salesClientCallModels.Count > 0)
                    {
                        responseModel.Message = "Record Found";
                        responseModel.Status = true;
                        responseModel.Data = salesClientCallModels;
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

        public ResponseModel DeleteSalesClientCallRecordMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    SalesClientCall_Master call_Master = db.SalesClientCall_Master.Where(x => x.scr_id == id).FirstOrDefault();
                    if (call_Master != null)
                    {
                        db.Entry(call_Master).State = EntityState.Deleted;
                        var result = db.SaveChanges();
                        if (result > 0)
                        {
                            responseModel.Message = "Record Deleted Successfully";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Delete Record";
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
