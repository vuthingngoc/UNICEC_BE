using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public class SponsorInCompetitionRepo : Repository<SponsorInCompetition>, ISponsorInCompetitionRepo
    {
        public SponsorInCompetitionRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<SponsorInCompetition> CheckSponsorInCompetition(int sponsorId, int competitionId, int userId)
        {
            SponsorInCompetition query = await (from sic in context.SponsorInCompetitions
                                                where sic.SponsorId == sponsorId && sic.CompetitionId == competitionId && sic.UserId == userId
                                                select sic).FirstOrDefaultAsync();
            if (query != null)
            {

                return query;
            }
            else
            {
                return null;
            }
        }

        public async Task<SponsorInCompetition> CheckSponsorInCompetition(int sponsorId, int competitionId)
        {
            SponsorInCompetition query = await(from sic in context.SponsorInCompetitions
                                               where sic.SponsorId == sponsorId && sic.CompetitionId == competitionId
                                               select sic).FirstOrDefaultAsync();
            if (query != null)
            {

                return query;
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteSponsorInCompetition(int sponsorInCompetitionId)
        {
            SponsorInCompetition result = await (from sic in context.SponsorInCompetitions
                                                 where sic.Id == sponsorInCompetitionId
                                                 select sic).FirstOrDefaultAsync();
            context.SponsorInCompetitions.Remove(result);
            await Update();
        }

        //
        public async Task<List<ViewSponsorInComp>> GetListSponsor_In_Competition(int CompetitionId)
        {
            List<SponsorInCompetition> sponsor_In_Competition_List = await (from sic in context.SponsorInCompetitions
                                                                            where CompetitionId == sic.CompetitionId
                                                                            select sic).ToListAsync();

            List<ViewSponsorInComp> listViewSponsor = new List<ViewSponsorInComp>();

            if (sponsor_In_Competition_List.Count > 0)
            {
                foreach (var sponsor_In_Competition in sponsor_In_Competition_List)
                {
                    Sponsor sponsor = await (from s in context.Sponsors
                                             where s.Id == sponsor_In_Competition.SponsorId
                                             select s).FirstOrDefaultAsync();

                    ViewSponsorInComp vsc = new ViewSponsorInComp()
                    {
                        Id = sponsor.Id,
                        Name = sponsor.Name,
                        Logo = sponsor.Logo,
                    };

                    listViewSponsor.Add(vsc);
                }

                if (listViewSponsor.Count > 0)
                {
                    return listViewSponsor;
                }
            }
            return null;
        }
    }
}
