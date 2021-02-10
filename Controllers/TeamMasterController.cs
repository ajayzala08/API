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
    public class TeamMasterController : ApiController
    {

        public ResponseModel GetTeamMaster()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    List<TeamMemberModel> teamlist = db.Team_Master.Select(x => new TeamMemberModel{ 
                    tid =x.T_Id,
                    teamlead = x.T_TeamLead,
                    teammember =x.T_TeamMember,
                    createdby=x.T_CreatedBy

                    }).ToList();
                    if (teamlist.Count > 0)
                    {
                        responseModel.Message = "Data Found";
                        responseModel.Data = teamlist;
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
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }
        }

        public ResponseModel GetTeamMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var teamlist = db.Team_Master.Where(x => x.T_Id == id).FirstOrDefault();
                    if(teamlist !=null) 
                    {
                        TeamMemberModel teamMemberModel = new TeamMemberModel();
                        teamMemberModel.tid = teamlist.T_Id;
                        teamMemberModel.teamlead = teamlist.T_TeamLead;
                        teamMemberModel.teammember = teamlist.T_TeamMember;
                        teamMemberModel.createdby = teamlist.T_CreatedBy;
                        responseModel.Message = "Data Found";
                        responseModel.Data = teamMemberModel;
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
                responseModel.Status = false;
                responseModel.Data = null;
                return responseModel;
            }
        }

        public ResponseModel DeleteTeamMaster(decimal id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var teamlist = db.Team_Master.Where(x => x.T_Id == id).FirstOrDefault();
                    if (teamlist != null)
                    {
                        db.Entry(teamlist).State = EntityState.Deleted;
                        db.SaveChanges();
                        responseModel.Message = "Team Deleted Successfully";
                        responseModel.Data = null;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Delete Team.";
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


        public ResponseModel PutTeamMaster(TeamMemberModel team)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var teamlist = db.Team_Master.Where(x => x.T_Id == team.tid).FirstOrDefault();
                    if (teamlist != null)
                    {
                        //Team_Master team_Master = new Team_Master();
                        teamlist.T_TeamLead = team.teamlead;
                        teamlist.T_TeamMember = team.teammember;
                        teamlist.T_UpdatedBy = team.createdby;
                        teamlist.T_UpdatedOn = System.DateTime.Now;
                        db.Entry(teamlist).State = EntityState.Modified;
                        db.SaveChanges();
                        responseModel.Message = "Team Updated Successfully";
                        responseModel.Data = null;
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Fail To Update Team.";
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

        public ResponseModel PostTeamMaster(TeamMemberModel team)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var db = new ATS2019_dbEntities())
                {
                    var teamlist = db.Team_Master.Where(x => x.T_TeamLead == team.teamlead).FirstOrDefault();
                    if (teamlist == null)
                    {
                        Team_Master team_Master = new Team_Master();
                        team_Master.T_TeamLead = team.teamlead;
                        team_Master.T_TeamMember = team.teammember;
                        team_Master.T_CreatedBy = team.createdby;
                        team_Master.T_CreatedOn = System.DateTime.Now;
                        db.Team_Master.Add(team_Master);
                        var res= db.SaveChanges();
                        if (res > 0)
                        {
                            responseModel.Message = "Team Created Successfully";
                            responseModel.Data = null;
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = "Fail To Create Team";
                            responseModel.Data = null;
                            responseModel.Status = false;
                        }
                    }
                    else
                    {
                        responseModel.Message = "Team Already Exists With TeamLead";
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
    }
}