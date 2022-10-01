using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;
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
        private IClubRepo _clubRepo;
        private DecodeToken _decodeToken;
        private IMemberRepo _memberRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private ITeamInRoundRepo _teamInRoundRepo;



        public TeamService(ITeamRepo teamRepo,
                           IParticipantRepo participantRepo,
                           ICompetitionRepo competitionRepo,
                           ITeamRoleRepo teamRoleRepo,
                           IClubRepo clubRepo,
                           IParticipantInTeamRepo participantInTeamRepo,
                           IMemberRepo memberRepo,
                           IMemberInCompetitionRepo memberInCompetitionRepo,
                           ITeamInRoundRepo teamInRoundRepo)
        {
            _teamRepo = teamRepo;
            _participantRepo = participantRepo;
            _competitionRepo = competitionRepo;
            _teamRoleRepo = teamRoleRepo;
            _clubRepo = clubRepo;
            _participantInTeamRepo = participantInTeamRepo;
            _memberRepo = memberRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _teamInRoundRepo = teamInRoundRepo;
            _decodeToken = new DecodeToken();
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
                    throw new ArgumentNullException("Competition Id Null");

                int UserId = _decodeToken.Decode(token, "Id");
                Competition competition = await _competitionRepo.Get(request.CompetitionId);
                //----- Check Competition id
                if (competition == null) throw new ArgumentException("Competition is not found");

                //----- Check Participant 
                //Participant p = await _participantRepo.ParticipantInCompetition(UserId, request.CompetitionId);
                //if (p == null) throw new UnauthorizedAccessException("You aren't participant in Competition");

                PagingResult<ViewTeam> result = await _teamRepo.GetAllTeamInCompetition(request);

                if (result == null) throw new NullReferenceException();

                return result;


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
                if (teamId == 0 || competitionId == 0) throw new ArgumentNullException("Competition Id Null  || Team Null");

                int UserId = _decodeToken.Decode(token, "Id");
                Competition competition = await _competitionRepo.Get(competitionId);

                //----- Check Competition id
                if (competition == null) throw new ArgumentException("Competition is not found");

                //----- Check Team id
                Team team = await _teamRepo.Get(teamId);
                if (team == null) throw new ArgumentException("Team is not found");

                //----- Check Team id is belong to this competition
                if (competition.Id != team.CompetitionId) throw new ArgumentException("Team is not belong to this competition");

                ////----- Check Participant  
                //Participant p = await _participantRepo.ParticipantInCompetition(UserId, competitionId);
                //if (p == null) throw new UnauthorizedAccessException("You aren't participant in Competition");

                ViewDetailTeam vdt = await _teamRepo.GetDetailTeamInCompetition(teamId, competitionId);

                if (vdt == null) throw new NullReferenceException();

                return vdt;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewTeamInCompetition> GetTotalResultTeamInCompetition(int competitionId, int teamId)
        {
            ViewTeamInCompetition resultTeamInCompetition = await _teamRepo.GetTotalResultTeamInCompetition(competitionId, teamId);
            if (resultTeamInCompetition == null) throw new NullReferenceException("Not found any result of team in this competition");

            resultTeamInCompetition.MembersInTeam = await _teamInRoundRepo.GetMembersInTeam(teamId);
            return resultTeamInCompetition;
        }

        public async Task<List<ViewResultTeam>> GetFinalResultTeamsInCompetition(string token, int competitionId, int? top)
        {
            // check valid
            if (competitionId == 0 || top < 0) throw new ArgumentException("Invalid competition || top must be greater than 0");
            bool isExisted = await _competitionRepo.CheckExistedCompetition(competitionId);
            if (!isExisted) throw new ArgumentException("Not found this competition");

            // Action
            List<ViewResultTeam> teams = await _teamRepo.GetFinalResultAllTeamsInComp(competitionId, top);
            if (teams == null) throw new NullReferenceException("Not found any teams in this competition");

            return teams;
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
                int UserId = _decodeToken.Decode(token, "Id");

                if (model.CompetitionId == 0
                 || string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException("Competition Id Null || Name Null");

                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                //Check Competition
                if (competition == null) throw new ArgumentException("Competition is not found");

                //Check Competition can create Team StartTimeRegister < local time < StartTime
                //nên chỉnh sang status
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                if (CheckDate(competition.StartTimeRegister, competition.StartTime) == false) throw new ArgumentException("Không thể tạo team khi đã qua thời gian tạo !");

                //check competition is Event or not -> if event can't create team
                if (competition.NumberOfTeam == 0) throw new ArgumentException("Event can't not create team !");

                //Check Participant
                Participant participant = await _participantRepo.ParticipantInCompetition(UserId, model.CompetitionId);
                if (participant == null) throw new UnauthorizedAccessException("You aren't participant in Competition");

                //Check participant already in team
                ParticipantInTeam participantInAnotherTeam = await _participantInTeamRepo.CheckParticipantInAnotherTeam(model.CompetitionId, UserId);
                if (participantInAnotherTeam != null) throw new ArgumentException("You are already in Team, Please out team previous to create new Team");

                //-----------------Add Team
                Team team = new Team()
                {
                    CompetitionId = model.CompetitionId,
                    Name = model.Name,
                    Description = model.Description ?? "",
                    //number of student in team
                    NumberOfStudentInTeam = 1,// auto vừa tạo là 1
                                              //generate code
                    InvitedCode = await CheckExistCode(),
                    //status available
                    Status = TeamStatus.Available
                };
                int Team_Id = await _teamRepo.Insert(team);

                if (Team_Id <= 0) throw new ArgumentException("Add Team Failed");

                Team getTeam = await _teamRepo.Get(Team_Id);
                //-----------------Add ParticiPant in Team with Role Leader
                ParticipantInTeam pit = new ParticipantInTeam()
                {
                    ParticipantId = participant.Id,
                    //Team Id
                    TeamId = getTeam.Id,
                    //auto leader 
                    TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Leader"),
                    //auto status 
                    Status = ParticipantInTeamStatus.InTeam
                };
                int result = await _participantInTeamRepo.Insert(pit);

                if (result <= 0) throw new ArgumentException("Add Team Leader Failed");

                return TransformViewTeam(getTeam);

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
                int UserId = _decodeToken.Decode(token, "Id");

                if (string.IsNullOrEmpty(model.InvitedCode)) throw new ArgumentNullException("Invited Code Null");

                //----- Check invited code 
                Team team = await _teamRepo.GetTeamByInvitedCode(model.InvitedCode);
                if (team == null) throw new ArgumentException("Not found team with Invited Code");

                //----- Check student is participant in that competiiton
                Participant participant = await _participantRepo.ParticipantInCompetition(UserId, team.CompetitionId);
                if (participant == null) throw new UnauthorizedAccessException("You aren't participant in Competition");

                //----- Check Team Status Is Locked 
                if (team.Status == TeamStatus.IsLocked) throw new ArgumentException("Team Is Locked you can't join");

                //----- Check join in the same Team in Competition
                ParticipantInTeam participantInTeam = await _participantInTeamRepo.CheckParticipantInTeam(team.Id, UserId);
                if (participantInTeam != null) throw new ArgumentException("You are already in that Team");

                //kh có trong team này nhưng có thể ở team khác
                //----- Check join another team when Student not out the previous team
                ParticipantInTeam participantInAnotherTeam = await _participantInTeamRepo.CheckParticipantInAnotherTeam(team.CompetitionId, UserId);
                if (participantInAnotherTeam != null) throw new ArgumentException("You are already in Team, Please out team previous to join the next Team");

                //----- Check number of member in team 
                Competition competition = await _competitionRepo.Get(team.CompetitionId);
                if (await _participantInTeamRepo.CheckNumberParticipantInTeam(team.Id, (int)competition.MaxNumber) == false) throw new ArgumentException("Team is full");

                //6.Check Time Status
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                Competition c = await _competitionRepo.Get(team.CompetitionId);
                if (CheckDate(c.StartTimeRegister, c.StartTime) == false) throw new ArgumentException("Không thể thực hiện hành động khi đã quá hạn !");

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

                t.NumberOfStudentInTeam = t.NumberOfStudentInTeam + 1;

                await _teamRepo.Update();

                return TransformViewParticipantInTeam(pit, competition.Id);

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
                int UserId = _decodeToken.Decode(token, "Id");

                if (model.TeamId == 0) throw new ArgumentNullException("Team Id Null");
                //1.check team
                Team team = await _teamRepo.Get(model.TeamId);
                if (team == null) throw new ArgumentException("Not found this team");

                //2.role leader of team can update
                ParticipantInTeam participantInTeam = await _participantInTeamRepo.CheckParticipantInTeam(model.TeamId, UserId);
                if (participantInTeam == null) throw new ArgumentException("You aren't participant in that team");

                //3.check teamRole of user is Leader 
                if (participantInTeam.TeamRoleId != await _teamRoleRepo.GetRoleIdByName("Leader")) throw new UnauthorizedAccessException("You don't have permission to update team role");

                //6.Check Time Status
                Competition competition = await _competitionRepo.Get(team.CompetitionId);
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                if (CheckDate(competition.StartTimeRegister, competition.StartTime) == false) throw new ArgumentException("Không thể thực hiện hành động khi đã quá hạn !");

                team.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : team.Name;
                team.Description = (!string.IsNullOrEmpty(model.Description)) ? model.Description : team.Description;

                if (model.Status.HasValue)
                {
                    //IsLocked
                    if (model.Status.Value == TeamStatus.IsLocked)
                    {
                        //Competition competition = await _competitionRepo.Get(team.CompetitionId);
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
                //Count số team Locked
                int numberOfTeamIsLocked = await _teamRepo.CountNumberOfTeamIsLocked(competition.Id);
                //update Number of team
                competition.NumberOfTeam = numberOfTeamIsLocked;
                await _competitionRepo.Update();
                return true;
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
                int UserId = _decodeToken.Decode(token, "Id");
                if (model.ParticipantInTeamId == 0) throw new ArgumentNullException("Participant In Team Id Null");
                //----- Check participant
                ParticipantInTeam member_PIT = await _participantInTeamRepo.Get(model.ParticipantInTeamId);
                if (member_PIT == null) throw new ArgumentException("Not found this participant in team");

                Team team = await _teamRepo.Get(member_PIT.TeamId);
                Competition competition = await _competitionRepo.Get(team.CompetitionId);
                int competitionId = competition.Id;

                //check user gọi hàm này phải là leader của Team này 
                //1.check student is participant in that competiiton
                Participant participant = await _participantRepo.ParticipantInCompetition(UserId, team.CompetitionId);
                if (participant == null) throw new UnauthorizedAccessException("You aren't participant in Competition");

                //2.check user in same Team in Competition
                ParticipantInTeam userInTeam = await _participantInTeamRepo.CheckParticipantInTeam(team.Id, UserId);
                if (userInTeam == null) throw new ArgumentException("You aren't participant in that team");

                //3.check teamRole of user is Leader 
                if (userInTeam.TeamRoleId != await _teamRoleRepo.GetRoleIdByName("Leader"))
                    throw new UnauthorizedAccessException("You don't have permission to update team role");

                //6.Check Time Status
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                if (CheckDate(competition.StartTimeRegister, competition.StartTime) == false) throw new ArgumentException("Không thể thực hiện hành động khi đã quá hạn !");

                //---UPDATE PIT ROLE
                //USER TO MEMBER
                userInTeam.TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Member");
                //MEMBER TO LEADER
                member_PIT.TeamRoleId = await _teamRoleRepo.GetRoleIdByName("Leader");
                await _participantInTeamRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //not build yet
        public async Task<bool> CompetitionManagerLockTeam(LockTeamModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0 || model.ClubId == 0) throw new ArgumentNullException("Competition Id Null || Club Id Null");

                await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, true);

                //Competition Status UpComming mới được ấn
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition.Status == CompetitionStatus.Draft
                 || competition.Status == CompetitionStatus.PendingReview
                 || competition.Status == CompetitionStatus.Pending
                 || competition.Status == CompetitionStatus.Approve
                 || competition.Status == CompetitionStatus.Publish)
                    throw new ArgumentException("Can't do this action at this Competition State");

                //Count số team Locked
                int numberOfTeamIsLocked = await _teamRepo.CountNumberOfTeamIsLocked(model.CompetitionId);

                //update Number of team
                competition.NumberOfTeam = numberOfTeamIsLocked;
                await _competitionRepo.Update();

                return true;
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
                int UserId = _decodeToken.Decode(token, "Id");

                //if (TeamId == 0) throw new ArgumentNullException("Team Id Null");
                //1.check team
                Team team = await _teamRepo.Get(TeamId);
                if (team == null) throw new ArgumentException("Not found this team");

                //----- Check Team Status Is Locked 
                if (team.Status == TeamStatus.IsLocked) throw new ArgumentException("Team Is Locked you delete");

                //2.check user in same Team in Competition
                ParticipantInTeam participantInTeam = await _participantInTeamRepo.CheckParticipantInTeam(TeamId, UserId);
                if (participantInTeam == null) throw new ArgumentException("You aren't participant in that team");

                //3.check teamRole of user is Leader 
                if (participantInTeam.TeamRoleId != await _teamRoleRepo.GetRoleIdByName("Leader"))
                    throw new UnauthorizedAccessException("You don't have permission to update team role");

                //6.Check Time Status
                Competition competition = await _competitionRepo.Get(team.CompetitionId);
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                if (CheckDate(competition.StartTimeRegister, competition.StartTime) == false) throw new ArgumentException("Không thể thực hiện hành động khi đã quá hạn !");

                //Delete Participant In Team
                await _participantInTeamRepo.DeleteAllParticipantInTeam(TeamId);
                //Delete Team
                await _teamRepo.DeleteTeam(TeamId);
                //
                return true;

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
                int UserId = _decodeToken.Decode(token, "Id");

                //if (TeamId == 0) throw new ArgumentNullException("Team Id Null");

                //1.check team
                Team team = await _teamRepo.Get(TeamId);
                if (team == null) throw new ArgumentException("Not found this team");

                //2. if team is locked can't out
                if (team.Status == TeamStatus.IsLocked) throw new ArgumentException("Can't out team because team is locked");

                //3.check user in same Team in Competition
                ParticipantInTeam Participant_In_Team = await _participantInTeamRepo.CheckParticipantInTeam(TeamId, UserId);
                if (Participant_In_Team == null) throw new ArgumentException("You aren't participant in that team");

                //6.Check Time Status
                Competition competition = await _competitionRepo.Get(team.CompetitionId);
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                if (CheckDate(competition.StartTimeRegister, competition.StartTime) == false) throw new ArgumentException("Không thể thực hiện hành động khi đã quá hạn !");

                //Delete Participant In Team
                await _participantInTeamRepo.DeleteParticipantInTeam(Participant_In_Team.Id);

                Team t = await _teamRepo.Get(team.Id);
                t.NumberOfStudentInTeam = t.NumberOfStudentInTeam - 1;
                //------------
                if (t.NumberOfStudentInTeam == 0)
                {
                    await _teamRepo.DeleteTeam(team.Id);
                }
                else
                {
                    await _teamRepo.Update();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Member is deleted by leader 
        public async Task<bool> DeleteMemberByLeader(int teamId, int participantId, string token)
        {
            try
            {
                int UserId = _decodeToken.Decode(token, "Id");

                //1.check team
                Team team = await _teamRepo.Get(teamId);
                if (team == null) throw new ArgumentException("Not found this team");

                //if team is locked can't out
                if (team.Status == TeamStatus.IsLocked) throw new ArgumentException("Can't Delete Team because team is locked");

                //2.check user in same Team in Competition
                ParticipantInTeam leaderInTeam = await _participantInTeamRepo.CheckParticipantInTeam(teamId, UserId);
                if (leaderInTeam == null) throw new ArgumentException("You aren't participant in that team");

                //3.check teamRole of user is Leader 
                if (leaderInTeam.TeamRoleId != await _teamRoleRepo.GetRoleIdByName("Leader"))
                    throw new UnauthorizedAccessException("You don't have permission to update team role");

                //4. Check Participant Id
                Participant p = await _participantRepo.Get(participantId);
                if (p == null) throw new ArgumentException("Participant not found");

                //5. Check xem participant này có trong team kh 
                ParticipantInTeam participantInTeam = await _participantInTeamRepo.CheckParticipantInTeam(teamId, p.StudentId);
                if (participantInTeam == null) throw new ArgumentException("member aren't participant in that team");

                //6.Check Time Status
                Competition competition = await _competitionRepo.Get(team.CompetitionId);
                if (competition.Status == CompetitionStatus.Pending) throw new ArgumentException("Không thể tạo team khi Cuộc Thi Sự Kiện đang bảo trì");

                if (CheckDate(competition.StartTimeRegister, competition.StartTime) == false) throw new ArgumentException("Không thể thực hiện hành động khi đã quá hạn !");

                //Delete Participant In Team
                await _participantInTeamRepo.DeleteParticipantInTeam(participantInTeam.Id);

                Team t = await _teamRepo.Get(team.Id);
                t.NumberOfStudentInTeam = t.NumberOfStudentInTeam - 1;
                //------------
                if (t.NumberOfStudentInTeam == 0)
                {
                    await _teamRepo.DeleteTeam(team.Id);
                }
                else
                {
                    await _teamRepo.Update();
                }
                return true;

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
                Status = team.Status,
                NumberOfMemberInTeam = team.NumberOfStudentInTeam,
                //await _teamRepo.getNumberOfMemberInTeam(team.Id),
            };
        }


        //Generate Seed code length 15
        private string GenerateSeedCode()
        {
            string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[6];
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


        private async Task<bool> CheckMemberInCompetition(string Token, int CompetitionId, int ClubId, bool isOrganization)
        {
            //------------- CHECK Competition in system
            Competition competition = await _competitionRepo.Get(CompetitionId);
            if (competition == null) throw new ArgumentException("Competition or Event not found ");

            //------------- CHECK Club in system
            Club club = await _clubRepo.Get(ClubId);
            if (club == null) throw new ArgumentException("Club in not found");

            //------------- CHECK Is Member in Club
            int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(Token, "Id"), club.Id);
            Member member = await _memberRepo.Get(memberId);
            if (member == null) throw new UnauthorizedAccessException("You aren't member in Club");

            //------------- CHECK User is in CompetitionManger table                
            MemberInCompetition isAllow = await _memberInCompetitionRepo.GetMemberInCompetition(CompetitionId, memberId);
            if (isAllow == null) throw new UnauthorizedAccessException("You do not in Competition Manager ");

            if (isOrganization)
            {
                //1,2 accept
                if (isAllow.CompetitionRoleId >= 3) throw new UnauthorizedAccessException("Only role Manager can do this action");
                return true;
            }
            else
            {
                return true;
            }
        }


    }
}
