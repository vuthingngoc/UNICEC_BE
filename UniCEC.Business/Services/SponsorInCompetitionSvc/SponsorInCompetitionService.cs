using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.SponsorInCompetitionSvc
{
    public class SponsorInCompetitionService : ISponsorInCompetitionService
    {
        private ISponsorInCompetitionRepo _sponsorInCompetitionRepo;
        //
        private ISponsorRepo _sponsorRepo;
        public SponsorInCompetitionService(ISponsorInCompetitionRepo sponsorInCompetitionRepo, ISponsorRepo sponsorRepo)
        {
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
            _sponsorRepo = sponsorRepo;
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
        public async Task<ViewSponsorInCompetition> Insert(SponsorInCompetitionInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var spIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));
                int SponsorId = Int32.Parse(spIdClaim.Value);

                if (model.SponsorIdCollaborate == 0
                   || model.CompetitionId == 0)
                    throw new ArgumentNullException("Sponsor Id Collaborate Null || Competition Id Null");
                //------------------------------------ 2 sponsor are the same 
                if (model.SponsorIdCollaborate != SponsorId)
                {
                    //------------------------------------check-sponsor-id-create-competition-or-event-duplicate
                    bool checkCreateSponsorInCompetition = await _sponsorInCompetitionRepo.CheckDuplicateCreateCompetitionOrEvent(SponsorId, model.CompetitionId);
                    //true  -> có nghĩa là nó chưa được tạo -> kh thể add được 
                    //false -> có nghĩa là nó chưa được tạo -> add được (do đây là add thêm Sponsor collaborate)
                    if (checkCreateSponsorInCompetition == false)
                    {
                        //------------------------------------Check Sponsor Collaborate in System 
                        Sponsor sponsor = await _sponsorRepo.Get(model.SponsorIdCollaborate);
                        if (sponsor != null)
                        {
                            SponsorInCompetition sponsorInCompetition = new SponsorInCompetition();
                            sponsorInCompetition.SponsorId = model.SponsorIdCollaborate;
                            sponsorInCompetition.CompetitionId = model.CompetitionId;

                            int result = await _sponsorInCompetitionRepo.Insert(sponsorInCompetition);
                            if (result > 0)
                            {
                                SponsorInCompetition sic = await _sponsorInCompetitionRepo.Get(result);
                                return TransferView(sic);
                            }//end result
                            else
                            {
                                throw new ArgumentException("Add Competition Or Event Failed");
                            }
                        }//end check sponsor Collaborate
                        else
                        {
                            throw new ArgumentException("Sponsor collaborate not in system");
                        }
                    }//end check exsit Competition Or Event
                    else
                    {
                        throw new ArgumentException("Competition or Event not found");
                    }
                }// end 2 sponsor is the same 
                else
                {
                    throw new ArgumentException("Sponsor already join");
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
