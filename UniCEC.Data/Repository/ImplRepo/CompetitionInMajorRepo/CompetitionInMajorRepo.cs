using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInMajorRepo
{
    public class CompetitionInMajorRepo : Repository<CompetitionInMajor>, ICompetitionInMajorRepo
    {
        public CompetitionInMajorRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<CompetitionInMajor> GetMajorInCompetition(int majorId, int competitionId)
        {
            var query = await (from cid in context.CompetitionInMajors
                               where cid.MajorId == majorId && cid.CompetitionId == competitionId
                               select cid).FirstOrDefaultAsync();
            if (query != null)
            {
                return query;
            }
            return null;
        }

        public async Task<List<int>> GetListMajorIdInCompetition(int competitionId)
        {
            List<int> majorIdList = await (from cid in context.CompetitionInMajors
                                           where competitionId == cid.CompetitionId
                                           select cid.MajorId).ToListAsync();
            if (majorIdList.Count > 0)
            {
                return majorIdList;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ViewMajorInComp>> GetListMajorInCompetition(int CompetitionId)
        {
            List<CompetitionInMajor> majorInCompetitionList = await (from cid in context.CompetitionInMajors
                                                                     where CompetitionId == cid.CompetitionId
                                                                     select cid).ToListAsync();
            List<ViewMajorInComp> list_vdic = new List<ViewMajorInComp>();
            if (majorInCompetitionList.Count > 0)
            {
                foreach (var majorInCompetition in majorInCompetitionList)
                {
                    Major major = await (from d in context.Majors
                                         where d.Id == majorInCompetition.MajorId
                                         select d).FirstOrDefaultAsync();

                    ViewMajorInComp vdic = new ViewMajorInComp()
                    {
                        Id = major.Id,
                        Name = major.Name,
                    };
                    list_vdic.Add(vdic);
                }

                if (list_vdic.Count > 0)
                {
                    return list_vdic;
                }
            }
            return null;
        }
    }
}
