using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo
{
    public interface ICompetitionActivityRepo : IRepository<CompetitionActivity>
    {
        
        public Task<bool> CheckExistCode(string code);
        ////
        public Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions);

        public Task<PagingResult<ViewCompetitionActivity>> GetListProcessActivitiesByConditions(CompetitionActivityRequestModel conditions);

        public Task<List<ViewProcessCompetitionActivity>> GetTopCompetitionActivity(int clubId, int topCompetition, int topCompetitionActivity);

        // Nhat
        public Task<int> GetTotalActivityByClub(int clubId);
    }
}
