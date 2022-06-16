using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Business.Services.CompetitionRoundSvc
{
    public class CompetitionRoundService : ICompetitionRoundService
    {
        private ICompetitionRoundRepo _competitionRoundRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;
        private IMajorRepo _majorRepo;
        private IClubRepo _clubRepo;

        private DecodeToken _decodeToken;

        public CompetitionRoundService(ICompetitionRoundRepo competitionRoundRepo, ICompetitionManagerRepo competitionManager, 
                                            IMajorRepo majorRepo, IClubRepo clubRepo)
        {
            _competitionRoundRepo = competitionRoundRepo;
            _competitionManagerRepo = competitionManager;
            _majorRepo = majorRepo;
            _clubRepo = clubRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task CheckValidAuthorizedAsync(string token, int competitionId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isValidUser = _competitionManagerRepo.CheckValidManagerByUser(competitionId, userId);
            if (!isValidUser) throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task Delete(string token, int id)
        {
            CheckValidAuthorizedAsync(token, id);

            CompetitionRound competitionRound = await _competitionRoundRepo.Get(id);
            if (competitionRound == null || (competitionRound != null && !competitionRound.CompetitionId.Equals(id))) 
                throw new NullReferenceException("Not found this competition round");

            //competitionRound.
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCompetitionRound>> GetByConditions(string token, CompetitionRoundRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionRound> GetById(string token, int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionRound> Insert(string token, CompetitionRoundInsertModel model)
        {
            throw new NotImplementedException();
        }

        public Task Update(string token, CompetitionRoundUpdateModel model)
        {
            throw new NotImplementedException();
        }
    }
}
