using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Enum;
using System.Collections.Generic;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public class ParticipantInTeamRepo : Repository<ParticipantInTeam>, IParticipantInTeamRepo
    {
        public ParticipantInTeamRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckNumberParticipantInTeam(int TeamId, int NumberOfStudentInTeam)
        {
            var query = from pit in context.ParticipantInTeams
                        where pit.TeamId == TeamId
                        select pit;

            int count = await query.CountAsync();
            if (count < NumberOfStudentInTeam)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ParticipantInTeam> CheckParticipantInAnotherTeam(int CompetitionId, int UserId)
        {
            var teams_Id = from t in context.Teams
                           where t.CompetitionId == CompetitionId
                           select t.Id;
            List<int> listTeamId = await teams_Id.ToListAsync();
            foreach (int teamId in listTeamId)
            {
                ParticipantInTeam p = await CheckParticipantInTeam(teamId, UserId);
                if(p != null) return p;
            }
            return null;
        }

        public async Task<ParticipantInTeam> CheckParticipantInTeam(int TeamId, int UserId)
        {
            //var query = from p in context.Participants
            //            where UserId == p.StudentId
            //            from pit in context.ParticipantInTeams
            //            where pit.ParticipantId == p.Id && pit.TeamId == TeamId && pit.Status == ParticipantInTeamStatus.InTeam
            //            select pit;
            var query = from p in context.Participants
                        join pit in context.ParticipantInTeams on p.Id equals pit.ParticipantId
                        where p.StudentId == UserId && pit.TeamId == TeamId && pit.Status == ParticipantInTeamStatus.InTeam
                        select pit;

            ParticipantInTeam participant_in_team = await query.FirstOrDefaultAsync();
            if (participant_in_team != null)
            {
                return participant_in_team;
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteAllParticipantInTeam(int teamId)
        {
            var queryPIT = from pit in context.ParticipantInTeams
                           where pit.TeamId == teamId
                           select pit;

            List<ParticipantInTeam> participantInTeams = await queryPIT.ToListAsync();

            foreach (ParticipantInTeam participant in participantInTeams)
            {
                context.ParticipantInTeams.Remove(participant);
                await Update();
            }
        }

        public async Task DeleteParticipantInTeam(int participantInTeamId)
        {
            var queryPIT = from pit in context.ParticipantInTeams
                           where pit.Id == participantInTeamId
                           select pit;

            ParticipantInTeam participantInTeam = await queryPIT.FirstOrDefaultAsync();


            context.ParticipantInTeams.Remove(participantInTeam);
            await Update();


        }

        public async Task<int> GetNumberOfMemberInTeam(int teamId, int competitionId)
        {
            var query = from t in context.Teams
                        where t.CompetitionId == competitionId
                        from pit in context.ParticipantInTeams
                        where pit.TeamId == teamId && pit.TeamId == t.Id
                        select pit;

            int count = await query.CountAsync();

            return count;
        }
    }
}
