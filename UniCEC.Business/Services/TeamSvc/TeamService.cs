using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRoleRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ParticipantInTeam;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Business.Services.TeamSvc
{
    public class TeamService : ITeamService
    {
        private ITeamRepo _teamRepo;
        private IParticipantRepo _participantRepo;
        private ICompetitionRepo _competitionRepo;
        private ITeamRoleRepo _teamRoleRepo;
        private IParticipantInTeamRepo _participantInTeamRepo;
        private JwtSecurityTokenHandler _tokenHandler;
        


        public TeamService(ITeamRepo teamRepo,
                           IParticipantRepo participantRepo,
                           ICompetitionRepo competitionRepo,
                           ITeamRoleRepo teamRoleRepo,                          
                           IParticipantInTeamRepo participantInTeamRepo)
        {
            _teamRepo = teamRepo;
            _participantRepo = participantRepo;
            _competitionRepo = competitionRepo;
            _teamRoleRepo = teamRoleRepo;
            _participantInTeamRepo = participantInTeamRepo;
           
        }

        public TeamService()
        {
        }



        //GET ALL TEAM IN COMPETITION
        public async Task<PagingResult<ViewTeam>> GetAllTeamInCompetition(TeamRequestModel request, string token)
        {
            try
            {
                if (request.CompetitionId == 0)
                    throw new ArgumentNullException("|| Competition Id Null");

                int UserId = DecodeToken(token, "Id");
                Competition competition = await _competitionRepo.Get(request.CompetitionId);
                //----- Check Competition id
                if (competition != null)
                {
                    //----- Check Participant 
                    Participant p = await _participantRepo.Participant_In_Competition(UserId, request.CompetitionId);
                    if (p != null)
                    {
                        PagingResult<ViewTeam> result = await _teamRepo.GetAllTeamInCompetition(request);
                        return result;
                    }
                    //end check is Participant
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't participant in Competition");
                    }
                }
                else
                {
                    throw new ArgumentException("Competition is not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewDetailTeam> GetDetailTeamInCompetition(int teamId, int competitionId, string token)
        {
            try
            {
                if (teamId == 0 || competitionId == 0) throw new ArgumentNullException("|| Competition Id Null  || Team Null");

                int UserId = DecodeToken(token, "Id");
                Competition competition = await _competitionRepo.Get(competitionId);
                //----- Check Competition id
                if (competition != null)
                {
                    //----- Check Team id
                    Team team = await _teamRepo.Get(teamId);
                    if (team != null)
                    {
                        //----- Check Team id is belong to this competition
                        if (competition.Id == team.CompetitionId)
                        {
                            //----- Check Participant  
                            Participant p = await _participantRepo.Participant_In_Competition(UserId, competitionId);
                            if (p != null)
                            {
                                ViewDetailTeam vdt = await _teamRepo.GetDetailTeamInCompetition(teamId, competitionId);
                                return vdt;
                            }
                            //end check is Participant
                            else
                            {
                                throw new UnauthorizedAccessException("You aren't participant in Competition");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Team is not belong to this competition");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Team is not found");
                    }
                }
                else
                {
                    throw new ArgumentException("Competition is not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //INSERT
        public async Task<ViewTeam> InsertTeam(TeamInsertModel model, string token)
        {

            //đã vào thấy screen này thì có nghĩa là đã trở thành Paricipant Check phase

            //---------------------------------------------------------------------INSERT
            //flow insert -- TEAM
            //CHECK Null
            //CHECK có phải là participant hay là không ? bởi vì chỉ có participant mới có quyền tạo
            //CHECK số lượng team trong 1 cuộc thi < Number Of Team
            //Add vào [Participant in Team] -> thằng tạo là leader của Team [TeamRole]
            //TeamStatus  
            //0.Available -> Can Join
            //1.IsLocked  -> Full Member
            //2.InActive  -> Delete Team by leader of team 


            //flow insert -- PARTICIPANT IN TEAM (CompetitionId, InvitedCode)
            //CHECK Null
            //CHECK Competition Id in system ?
            //CHECK InvitedCODE đó là của Team thuộc Competition nào?
            //CHECK Team Status -> team is full
            //CHECK Member Of Team is available
            //add Participant by InvitedCode


            //ROLE OF Team Member
            //1.Leader
            //2.Member       
            try
            {
                int UserId = DecodeToken(token, "Id");

                if (model.CompetitionId == 0
                 || string.IsNullOrEmpty(model.Name)
                 || string.IsNullOrEmpty(model.Description)) throw new ArgumentNullException("Competition Id Null || Name Null || Description Null");

                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                //Check Competition
                if (competition != null)
                {
                    //Check Competition can create Team StartTimeRegister < local time < StartTime
                    if (CheckDate(competition.StartTimeRegister, competition.StartTime))
                    {
                        //check competition is Event or not -> if event can't create team
                        if (competition.NumberOfTeam != 0)
                        {
                            if (await _participantRepo.Participant_In_Competition(UserId, model.CompetitionId) != null) // Check xem có phải là Participant kh 
                            {
                                //-----------------Add Team
                                Team team = new Team()
                                {
                                    CompetitionId = model.CompetitionId,
                                    Name = model.Name,
                                    Description = model.Description,
                                    //number of student in team
                                    NumberOfStudentInTeam = 1,// auto vừa tạo là 1
                                    //generate code
                                    InvitedCode = await CheckExistCode(),
                                    //status available
                                    Status = TeamStatus.Available
                                };
                                int Team_Id = await _teamRepo.Insert(team);
                                if (Team_Id > 0)
                                {
                                    Team getTeam = await _teamRepo.Get(Team_Id);
                                    //-----------------Add ParticiPant in Team with Role Leader
                                    ParticipantInTeam pit = new ParticipantInTeam()
                                    {
                                        ParticipantId = model.CompetitionId,
                                        //Team Id
                                        TeamId = getTeam.Id,
                                        //auto leader 
                                        TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Leader"),
                                        //auto status 
                                        Status = ParticipantInTeamStatus.InTeam
                                    };
                                    int result = await _participantInTeamRepo.Insert(pit);
                                    if (result > 0)
                                    {
                                        return TransformViewTeam(getTeam);
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Add Team Leader Failed");
                                    }
                                }//end add team
                                else
                                {
                                    throw new ArgumentException("Add Team Failed");
                                }
                            }
                            //end check is Participant
                            else
                            {
                                throw new UnauthorizedAccessException("You aren't participant in Competition");
                            }
                        }
                        //end check Event
                        else
                        {
                            throw new ArgumentException("Event can't not create team !");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Can't create Team at this moment !!!");
                    }
                }
                //end check competition != null
                else
                {
                    throw new ArgumentException("Competition is not found");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<ViewParticipantInTeam> InsertMemberInTeam(ParticipantInTeamInsertModel model, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");

                if (string.IsNullOrEmpty(model.InvitedCode)) throw new ArgumentNullException("Invited Code Null");

                //----- Check invited code 
                Team team = await _teamRepo.GetTeamByInvitedCode(model.InvitedCode);
                if (team != null)
                {
                    //----- Check student is participant in that competiiton
                    Participant participant = await _participantRepo.Participant_In_Competition(UserId, team.CompetitionId);
                    if (participant != null)
                    {
                        //----- Check Team Status Is Locked 
                        if (team.Status == TeamStatus.IsLocked)
                        {
                            //----- Check join in the same Team in Competition
                            ParticipantInTeam Participant_In_Team = await _participantInTeamRepo.CheckParticipantInTeam(team.Id, UserId);
                            if (Participant_In_Team == null)
                            {
                                //kh có trong team này nhưng có thể ở team khác
                                //----- Check join another team when Student not out the previous team
                                ParticipantInTeam Participant_In_Another_Team = await _participantInTeamRepo.CheckParticipantInAnotherTeam(team.CompetitionId, UserId);
                                if (Participant_In_Another_Team == null)
                                {
                                    //----- Check number of member in team 
                                    Competition competition = await _competitionRepo.Get(team.CompetitionId);
                                    if (await _participantInTeamRepo.CheckNumberParticipantInTeam(team.Id, (int)competition.MaxNumber))
                                    {
                                        //------ Add Participant in team
                                        ParticipantInTeam pit = new ParticipantInTeam()
                                        {
                                            ParticipantId = participant.Id,
                                            TeamId = team.Id,
                                            //auto member
                                            TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Member"),
                                            //auto status 
                                            Status = ParticipantInTeamStatus.InTeam
                                        };

                                        await _participantInTeamRepo.Insert(pit);

                                        //------------------Update Number of member in Team
                                        Team t = await _teamRepo.Get(team.Id);

                                        t.NumberOfStudentInTeam = t.NumberOfStudentInTeam++;
                                      
                                        await _teamRepo.Update();

                                        return TransformViewParticipantInTeam(pit, competition.Id);
                                    } //end check number of member in team
                                    else
                                    {
                                        throw new ArgumentException("Team is full");
                                    }
                                }//end join another team when Student not out the previous team
                                else
                                {
                                    throw new ArgumentException("You are already in Team, Please out team previous to join the next Team");
                                }
                            }//end in the same Team in Competition
                            else
                            {
                                throw new ArgumentException("You are already in that Team");
                            }
                        }// end check team is locked 
                        else
                        {
                            throw new ArgumentException("Team Is Locked you can't join");
                        }
                    }
                    //end check is participant
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't participant in Competition");
                    }
                }
                //end invited code 
                else
                {
                    throw new ArgumentException("Not found team with Invited Code");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTeam(TeamUpdateModel model, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");

                if (model.TeamId == 0) throw new ArgumentNullException("Team Id Null");
                //check team
                Team team = await _teamRepo.Get(model.TeamId);

                if (team != null)
                {
                    //role leader of team can update
                    ParticipantInTeam Participant_In_Team = await _participantInTeamRepo.CheckParticipantInTeam(model.TeamId, UserId);
                    if (Participant_In_Team != null)
                    {
                        //3.check teamRole of user is Leader 
                        if (Participant_In_Team.TeamRoleId == await _teamRoleRepo.GetRoleIdByName("Leader"))
                        {
                            team.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : team.Name;
                            team.Description = (!string.IsNullOrEmpty(model.Description)) ? model.Description : team.Description;

                            if (model.Status.HasValue)
                            {
                                //IsLocked
                                if (model.Status.Value == TeamStatus.IsLocked)
                                {
                                    Competition competition = await _competitionRepo.Get(team.CompetitionId);
                                    int numberOfMemberInTeam = await _participantInTeamRepo.GetNumberOfMemberInTeam(model.TeamId, team.CompetitionId);
                                    if (numberOfMemberInTeam - competition.MinNumber >= 0)
                                    {
                                        if (numberOfMemberInTeam - competition.MaxNumber <= 0)
                                        {
                                            team.Status = model.Status.Value;
                                        }
                                        else
                                        {
                                            throw new ArgumentException("Number of member in team is > " + competition.MaxNumber);
                                        }
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Number of member in team is < " + competition.MinNumber);
                                    }
                                }
                                //Available
                                else
                                {
                                    team.Status = model.Status.Value;
                                }
                            }
                            await _teamRepo.Update();
                            return true;

                        }//end check role leader
                        else
                        {
                            throw new UnauthorizedAccessException("You don't have permission to update team role");
                        }
                    }
                    //end check participant in Team
                    else
                    {
                        throw new ArgumentException("You aren't participant in that team");
                    }
                }
                //end Check Team
                else
                {
                    throw new ArgumentException("Not found this team");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTeamRole(ParticipantInTeamUpdateModel model, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");
                if (model.ParticipantInTeamId != 0) throw new ArgumentNullException("Participant In Team Id Null");
                //----- Check participant
                ParticipantInTeam Member_pit = await _participantInTeamRepo.Get(model.ParticipantInTeamId);
                if (Member_pit != null)
                {
                    Team team = await _teamRepo.Get(Member_pit.TeamId);
                    Competition competition = await _competitionRepo.Get(team.CompetitionId);
                    int competitionId = competition.Id;
                    //check user gọi hàm này phải là leader của Team này 
                    //1.check student is participant in that competiiton
                    Participant participant = await _participantRepo.Participant_In_Competition(UserId, team.CompetitionId);
                    if (participant != null)
                    {
                        //2.check user in same Team in Competition
                        ParticipantInTeam User_In_Team = await _participantInTeamRepo.CheckParticipantInTeam(team.Id, UserId);
                        if (User_In_Team != null)
                        {
                            //3.check teamRole of user is Leader 
                            if (User_In_Team.TeamRoleId == await _teamRoleRepo.GetRoleIdByName("Leader"))
                            {
                                //---UPDATE PIT ROLE
                                //USER TO MEMBER
                                User_In_Team.TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Member");
                                //MEMBER TO LEADER
                                Member_pit.TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Leader");
                                await _participantInTeamRepo.Update();
                                return true;
                            }
                            //end check role leader
                            else
                            {
                                throw new UnauthorizedAccessException("You don't have permission to update team role");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("You aren't participant in that team");
                        }
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't participant in Competition");
                    }
                }
                //end check pariticpant
                else
                {
                    throw new ArgumentException("Not found this participant in team");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Delete nguyên team
        public async Task<bool> DeleteByLeader(int TeamId, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");

                if (TeamId != 0) throw new ArgumentNullException("Team Id Null");
                //1.check team
                Team team = await _teamRepo.Get(TeamId);
                if (team != null)
                {
                    //2.check user in same Team in Competition
                    ParticipantInTeam Participant_In_Team = await _participantInTeamRepo.CheckParticipantInTeam(TeamId, UserId);
                    if (Participant_In_Team != null)
                    {
                        //3.check teamRole of user is Leader 
                        if (Participant_In_Team.TeamRoleId == await _teamRoleRepo.GetRoleIdByName("Leader"))
                        {
                            //Delete Participant In Team
                            await _participantInTeamRepo.DeleteParticipantInTeam(TeamId);
                            //Delete Team
                            await _teamRepo.DeleteTeam(TeamId);
                            //
                            return true;

                        }//end check role leader
                        else
                        {
                            throw new UnauthorizedAccessException("You don't have permission to update team role");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("You aren't participant in that team");
                    }
                }
                //end check Team
                else
                {
                    throw new ArgumentException("Not found this team");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Member out team
        public async Task<bool> OutTeam(int TeamId, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");

                if (TeamId != 0) throw new ArgumentNullException("Team Id Null");

                //1.check team
                Team team = await _teamRepo.Get(TeamId);
                if (team != null)
                {
                    //2. if team is locked can't out
                    if(team.Status == TeamStatus.IsLocked) throw new ArgumentException("Can't out team because team is locked");

                    //3.check user in same Team in Competition
                    ParticipantInTeam Participant_In_Team = await _participantInTeamRepo.CheckParticipantInTeam(TeamId, UserId);
                    if (Participant_In_Team != null)
                    {                     
                        //Delete Participant In Team
                        await _participantInTeamRepo.DeleteParticipantInTeam(TeamId);
                        //------------------Update number of member in Team
                        Team t = await _teamRepo.Get(team.Id);
                        t.NumberOfStudentInTeam = t.NumberOfStudentInTeam - 1;
                        await _teamRepo.Update();
                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("You aren't participant in that team");
                    }

                } //end check Team
                else
                {
                    throw new ArgumentException("Not found this team");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ViewParticipantInTeam TransformViewParticipantInTeam(ParticipantInTeam participantInTeam, int CompetitionId)
        {
            return new ViewParticipantInTeam()
            {
                Id = participantInTeam.Id,
                ParticipantId = participantInTeam.ParticipantId,
                TeamId = participantInTeam.TeamId,
                CompetitionId = CompetitionId
            };
        }

        public ViewTeam TransformViewTeam(Team team)
        {
            return new ViewTeam()
            {
                TeamId = team.Id,
                Name = team.Name,
                CompetitionId = team.CompetitionId,
                Description = team.Description,
                InvitedCode = team.InvitedCode,
            };
        }


        //Generate Seed code length 15
        private string GenerateSeedCode()
        {
            string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[15];
            string code = "";
            var random = new Random();

            for (int i = 0; i < chars.Length; i++)
            {
                code += string.Concat(codePool[random.Next(codePool.Length)]);
            }
            return code;
        }

        //----------------------------------------------------------------------------------------Check
        //check exist code
        private async Task<string> CheckExistCode()
        {
            //auto generate seedCode
            bool check = true;
            string seedCode = "";
            while (check)
            {
                string generateCode = GenerateSeedCode();
                check = await _teamRepo.CheckExistCode(generateCode);
                seedCode = generateCode;
            }
            return seedCode;
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        private bool CheckDate(DateTime StartTimeRegister, DateTime StartTime)
        {
            bool check = false;
            DateTime localTime = new LocalTime().GetLocalTime().DateTime;
            //STR < LT < ST
            //STR < LT
            int resultLT1 = DateTime.Compare(localTime, StartTimeRegister);
            if (resultLT1 > 0)
            {
                // LT < ST
                int resultLT2 = DateTime.Compare(localTime, StartTime);
                if (resultLT2 < 0)
                {
                    check = true;
                    return check;
                }
            }
            return check;
        }



    }
}
