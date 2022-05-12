using System;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.SponsorInCompetitionSvc
{
    public class SponsorInCompetitionService : ISponsorInCompetitionService
    {
        private ISponsorInCompetitionRepo _sponsorInCompetitionRepo;
        //change Status of Competition
        private ICompetitionRepo _competitionRepo;

        public SponsorInCompetitionService(ISponsorInCompetitionRepo sponsorInCompetitionRepo, ICompetitionRepo competitionRepo)
        {
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
            _competitionRepo = competitionRepo;

        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewSponsorInCompetition>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSponsorInCompetition> GetBySponsorInCompetitionId(int id)
        {
            throw new NotImplementedException();
        }

        //Sponsor Create Competition - Event
        public async Task<ViewSponsorInCompetition> Insert(SponsorInCompetitionInsertModel model)
        {
            try
            {
                //------------------------------------check-sponsor-id-create-competition-or-event-duplicate
                bool checkCreateSponsorInCompetition = await _sponsorInCompetitionRepo.CheckDuplicateCreateCompetitionOrEvent(model.SponsorId, model.CompetitionId);
                if (checkCreateSponsorInCompetition)
                {
                    SponsorInCompetition sponsorInCompetition = new SponsorInCompetition();
                    sponsorInCompetition.SponsorId = model.SponsorId;
                    sponsorInCompetition.CompetitionId = model.CompetitionId;

                    int result = await _sponsorInCompetitionRepo.Insert(sponsorInCompetition);
                    if (result > 0)
                    {
                        //đổi status của competition 
                        Competition comp = await _competitionRepo.Get(model.CompetitionId);
                        comp.Status = CompetitionStatus.Launching;
                        await _competitionRepo.Update();
                        //
                        return TransferView(sponsorInCompetition);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> Update(ViewSponsorInCompetition sponsorInCompetition)
        {
            throw new NotImplementedException();
        }

        private ViewSponsorInCompetition TransferView(SponsorInCompetition sponsorInCompetition)
        {
            return new ViewSponsorInCompetition()
            {
                Id = sponsorInCompetition.Id,
                SponsorId = sponsorInCompetition.SponsorId,
                CompetitionId = sponsorInCompetition.CompetitionId,
            };
        }
    }
}
