using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            int numberOfTeam = comp.NumberOfTeam;
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
    }
}
