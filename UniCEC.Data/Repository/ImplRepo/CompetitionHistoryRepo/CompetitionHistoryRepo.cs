using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo
{
    public class CompetitionHistoryRepo : Repository<CompetitionHistory>, ICompetitionHistoryRepo
    {
        public CompetitionHistoryRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<ViewCompetitionHistory>> GetAllHistoryOfCompetition(int competitionId)
        {
            List<CompetitionHistory> competitionHistories = await (from chs in context.CompetitionHistories
                                                                         where chs.CompetitionId == competitionId
                                                                   select chs).ToListAsync();

            List<ViewCompetitionHistory> result = new List<ViewCompetitionHistory>();

            foreach (CompetitionHistory competitionHistory in competitionHistories)
            {
                string Name = null;
                if (competitionHistory.ChangerId != null)
                {
                    Member member = await (from m in context.Members
                                           where m.Id == competitionHistory.ChangerId
                                           select m).FirstOrDefaultAsync();
                    Name = member.User.Fullname;
                }

                ViewCompetitionHistory vch = new ViewCompetitionHistory()
                {
                    Id = competitionHistory.Id,
                    CompetitionId = competitionHistory.CompetitionId,
                    ChangerId = (competitionHistory.ChangerId != null) ? competitionHistory.ChangerId : null,
                    ChangerName = (Name != null) ? Name : null,
                    ChangeDate = competitionHistory.ChangeDate,
                    Description = (competitionHistory.Description != null) ? competitionHistory.Description : null,
                    Status = competitionHistory.Status     
                };
                result.Add(vch);
            }

            return (result.Count > 0) ? result : null;
        }
    }
}
