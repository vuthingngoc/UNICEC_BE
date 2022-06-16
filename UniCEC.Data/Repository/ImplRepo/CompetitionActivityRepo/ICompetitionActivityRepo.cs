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
        //
        public Task<bool> CheckExistCode(string code);
        //
        public Task<PagingResult<ViewDetailCompetitionActivity>> GetListClubActivitiesByConditions(CompetitionActivityRequestModel conditions);
        //
        public Task<List<ViewDetailCompetitionActivity>> GetClubActivitiesByCreateTime(int universityId, int clubId);

        // Nhat
        public Task<int> GetTotalActivityByClub(int clubId);
    }
}
