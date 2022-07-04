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
                                                                   orderby chs.ChangeDate descending
                                                                   select chs).ToListAsync();

            List<ViewCompetitionHistory> result = new List<ViewCompetitionHistory>();

            foreach (CompetitionHistory competitionHistory in competitionHistories)
            {
                string Name = null;
                if (competitionHistory.ChangerId != null)
                {
                    if (competitionHistory.ChangerId != 0)
                    {
                        Member member = await (from m in context.Members
                                               where m.Id == competitionHistory.ChangerId
                                               select m).FirstOrDefaultAsync();
                        if (member != null)
                        {
                            Name = member.User.Fullname;
                        }
                        else
                        {
                            User adminUni = await (from us in context.Users
                                                   where us.Id == competitionHistory.ChangerId
                                                   select us).FirstOrDefaultAsync();
                            Name = adminUni.Fullname;
                        }
                    }
                }

                ViewCompetitionHistory vch = new ViewCompetitionHistory()
                {
                    Id = competitionHistory.Id,
                    CompetitionId = competitionHistory.CompetitionId,
                    ChangerId = (competitionHistory.ChangerId != 0) ? competitionHistory.ChangerId : 0,
                    ChangerName = (Name != null) ? Name : "System",
                    ChangeDate = competitionHistory.ChangeDate,
                    Description = (competitionHistory.Description != null) ? competitionHistory.Description : null,
                    Status = competitionHistory.Status
                };
                result.Add(vch);
            }

            return (result.Count > 0) ? result : null;
        }

        public async Task<CompetitionHistory> GetNearestStateAfterPending(int competitionId)
        {
            CompetitionHistory competitionHistory = await (from ch in context.CompetitionHistories
                                                                 where ch.CompetitionId == competitionId
                                                                 orderby ch.ChangeDate descending
                                                                 select ch).Skip(1).Take(1).FirstOrDefaultAsync();


            return competitionHistory;


        }
    }
}
