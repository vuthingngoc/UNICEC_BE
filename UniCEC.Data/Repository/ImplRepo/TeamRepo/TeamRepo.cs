using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Team;
using UniCEC.Data.RequestModels;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Data.Repository.ImplRepo.TeamRepo
{
    public class TeamRepo : Repository<Team>, ITeamRepo
    {
        public TeamRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckExistCode(string code)
        {
            bool check = false;
            Team team = await context.Teams.FirstOrDefaultAsync(x => x.InvitedCode.Equals(code));
            if (team != null)
            {
                check = true;
                return check;
            }
            return check;
        }

        public async Task<bool> CheckNumberOfTeam(int CompetitionId)
        {
            var query = from t in context.Teams
                        where t.CompetitionId == CompetitionId
                        select t;

            var queryCompetition = from c in context.Competitions
                                   where c.Id == CompetitionId
                                   select c;

            Competition comp = queryCompetition.FirstOrDefault();
            int numberOfTeam = (int)comp.NumberOfTeam;
            int count = await query.CountAsync();
            if (count < numberOfTeam)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task DeleteTeam(int TeamId)
        {
            var query = from t in context.Teams
                        where t.Id == TeamId
                        select t;

            Team team = await query.FirstOrDefaultAsync();
            context.Teams.Remove(team);
            await Update();
        }


        public async Task<Team> GetTeamByInvitedCode(string invitedCode)
        {
            var query = from t in context.Teams
                        where t.InvitedCode == invitedCode
                        select t;

            Team team = await query.FirstOrDefaultAsync();

            if (team != null)
            {
                return team;
            }
            else
            {
                return null;
            }

        }

        public async Task<PagingResult<ViewTeam>> GetAllTeamInCompetition(TeamRequestModel request)
        {
            //
            List<ViewTeam> list_viewTeam = new List<ViewTeam>();

            var query = from t in context.Teams
                        where t.CompetitionId == request.CompetitionId
                        select t;

            int totalCount = await query.CountAsync();

            List<Team> list_team = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (Team team in list_team)
            {
                ViewTeam vt = new ViewTeam()
                {
                    TeamId = team.Id,
                    CompetitionId = team.CompetitionId,
                    Description = team.Description,
                    Name = team.Name,
                    InvitedCode = team.InvitedCode,
                    Status = team.Status,
                    NumberOfMemberInTeam = team.NumberOfStudentInTeam
                    //await getNumberOfMemberInTeam(team.Id)
                };

                list_viewTeam.Add(vt);
            }

            return (list_viewTeam.Count > 0) ? new PagingResult<ViewTeam>(list_viewTeam, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<ViewDetailTeam> GetDetailTeamInCompetition(int teamId, int competitionId)
        {
            Team team = await (from t in context.Teams
                               where t.Id == teamId && t.CompetitionId == competitionId
                               select t).FirstOrDefaultAsync();

            List<ParticipantInTeam> participantInTeams = team.ParticipantInTeams.ToList();

            List<Participant> participants = new List<Participant>();

            List<ViewDetailParticipant> viewParticipants = new List<ViewDetailParticipant>();
            //
            foreach (ParticipantInTeam pit in participantInTeams)
            {
                Participant participant = await (from p in context.Participants
                                                 where p.Id == pit.ParticipantId && p.CompetitionId == competitionId
                                                 select p).FirstOrDefaultAsync();
                if (participant != null)
                {
                    participants.Add(participant);
                }
            }
            //
            if (participants.Count > 0)
            {
                foreach (Participant participant in participants)
                {
                    //get avatar
                    User user = await (from u in context.Users
                                       from p in context.Participants
                                       where p.StudentId == u.Id
                                       select u).FirstOrDefaultAsync();

                    ParticipantInTeam participantInTeam = await (from pit in context.ParticipantInTeams
                                                                 where pit.ParticipantId == participant.Id && pit.TeamId == teamId
                                                                 select pit).FirstOrDefaultAsync();

                    ViewDetailParticipant vp = new ViewDetailParticipant()
                    {
                        ParticipantId = participant.Id,
                        CompetitionId = participant.CompetitionId,
                        RegisterTime = participant.RegisterTime,
                        StudentId = participant.StudentId,
                        Avatar = user.Avatar,
                        StudentCode = user.StudentCode,
                        UniversityName = user.University.Name,
                        ParticipantInTeamId = participantInTeam.Id,
                        TeamRoleId = participantInTeam.TeamRoleId,
                        TeamRoleName = participantInTeam.TeamRole.Name,
                        Status = participantInTeam.Status             
                    };

                    viewParticipants.Add(vp);
                }
            }

            return (team != null) ? new ViewDetailTeam()
            {
                TeamId = team.Id,
                CompetitionId = competitionId,
                Description = team.Description,
                InvitedCode = team.InvitedCode,
                Name = team.Name,
                ListParticipant = viewParticipants,
                Status = team.Status,
                NumberOfMemberInTeam = team.NumberOfStudentInTeam
                //await getNumberOfMemberInTeam(team.Id)
            } : null;

        }

        public async Task<bool> UpdateStatusAvailableForAllTeam(int CompetitionId)
        {
            List<Team> list_team = await (from t in context.Teams
                                          where t.CompetitionId == CompetitionId
                                          select t).ToListAsync();
            if (list_team.Count > 0)
            {
                foreach (Team team in list_team)
                {
                    team.Status = Enum.TeamStatus.Available;
                }
                await Update();
                return true;
            }
            else
            {
                return true; // vẫn là true nếu không có team để update thì thôi
            }
        }

        public async Task<bool> CheckExistedTeam(int teamId)
        {
            return await context.Teams.FirstOrDefaultAsync(team => team.Id.Equals(teamId)) != null;
        }

        public async Task<int> CountNumberOfTeamIsLocked(int competitionId)
        {
            var query = from t in context.Teams
                        where t.Status == Enum.TeamStatus.IsLocked && t.CompetitionId == competitionId
                        select t;
            return (await query.CountAsync() > 0) ? await query.CountAsync() : -1;
        }

        public async Task<int> getNumberOfMemberInTeam(int teamId)
        {
            List<ParticipantInTeam> participants = await (from pit in context.ParticipantInTeams
                                                          where pit.TeamId == teamId
                                                          select pit).ToListAsync();
            return participants.Count;
        }
    }
}
