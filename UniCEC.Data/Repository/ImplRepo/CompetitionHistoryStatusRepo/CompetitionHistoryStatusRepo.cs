using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo
{
    public class CompetitionHistoryStatusRepo : Repository<CompetitionHistoryStatus>, ICompetitionHistoryStatusRepo
    {
        public CompetitionHistoryStatusRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<ViewCompetitionHistoryStatus>> GetAllHistoryOfCompetition(int competitionId)
        {
            List<CompetitionHistoryStatus> competitionHistoryStatuses = await (from chs in context.CompetitionHistoryStatuses
                                                                   where chs.CompetitionId == competitionId
                                                                   select chs).ToListAsync();

            List<ViewCompetitionHistoryStatus> result = new List<ViewCompetitionHistoryStatus>();

            foreach (CompetitionHistoryStatus competitionHistoryStatus in competitionHistoryStatuses)
            {
                string Name = null;
                if (competitionHistoryStatus.ChangerId != null)
                {
                    Member member = await (from m in context.Members
                                           where m.Id == competitionHistoryStatus.ChangerId
                                           select m).FirstOrDefaultAsync();
                    Name = member.User.Fullname;
                }

                ViewCompetitionHistoryStatus vch = new ViewCompetitionHistoryStatus()
                {
                    Id = competitionHistoryStatus.Id,
                    CompetitionId = competitionHistoryStatus.CompetitionId,
                    ChangerId = (competitionHistoryStatus.ChangerId != null) ? competitionHistoryStatus.ChangerId : null,
                    ChangerName = (Name != null) ? Name : null,
                    ChangeDate = competitionHistoryStatus.ChangeDate,
                    Description = (competitionHistoryStatus.Description != null) ? competitionHistoryStatus.Description : null,
                    Status = competitionHistoryStatus.Status     
                };
                result.Add(vch);
            }

            return (result.Count > 0) ? result : null;
        }
    }
}
