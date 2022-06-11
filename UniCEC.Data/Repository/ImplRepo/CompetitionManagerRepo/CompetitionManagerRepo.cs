using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo
{
    public class CompetitionManagerRepo : Repository<CompetitionManager>, ICompetitionManagerRepo
    {
        public CompetitionManagerRepo(UniCECContext context) : base(context)
        {
        }

        public bool CheckValidManagerByUser(int competitionId, int userId)
        {
            var query = from cm in context.CompetitionManagers
                        join m in context.Members on cm.MemberId equals m.Id
                        join c in context.Clubs on m.ClubId equals c.Id
                        join cic in context.CompetitionInClubs on c.Id equals cic.ClubId
                        where cic.CompetitionId.Equals(competitionId) && m.UserId.Equals(userId)
                        select new {cm, m, c, cic};
            
            return query.Any();
        }


        //public async Task<CompetitionManager> GetManagerInCompetitionManager(int CompetitionId, int ClubId, int MemberId)
        //{
        //    var query = from cic in context.CompetitionInClubs
        //                where cic.CompetitionId == CompetitionId && cic.ClubId == ClubId
        //                from cm in context.CompetitionManagers
        //                where cm.CompetitionInClubId == cic.Id && cm.MemberId == MemberId                     
        //                select cm;

        //    CompetitionManager competitionManager = await query.FirstOrDefaultAsync();
        //    if (competitionManager != null)
        //    {
        //        return competitionManager;
        //    }
        //    return null; 
        //}

        public async Task<CompetitionManager> GetMemberInCompetitionManager(int CompetitionId, int MemberId, int ClubId)
        {
            var query = from cic in context.CompetitionInClubs
                        where cic.CompetitionId == CompetitionId && cic.ClubId == ClubId
                        from cm in context.CompetitionManagers
                        where cm.CompetitionInClubId == cic.Id && cm.MemberId == MemberId
                        select cm;

            CompetitionManager competitionManager = await query.FirstOrDefaultAsync();
            if (competitionManager != null)
            {
                return competitionManager;
            }
            return null;
        }
    }
}
