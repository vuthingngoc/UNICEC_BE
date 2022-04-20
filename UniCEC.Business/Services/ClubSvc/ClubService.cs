using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public class ClubService : IClubService
    {
        private IClubRepo _clubRepo;

        public ClubService(IClubRepo clubRepo)
        {
            _clubRepo = clubRepo;
        }

        private ViewClub transformViewClub(Club club)
        {
            return new ViewClub()
            {
                Id = club.Id,
                Description = club.Description,
                Founding = club.Founding,
                Name = club.Name,
                TotalMember = club.TotalMember,
                UniversityId = club.UniversityId,
                Status = club.Status,
            };
        }

        public async Task<PagingResult<ViewClub>> GetAllPaging(PagingRequest request)
        {
            PagingResult<Club> clubs = await _clubRepo.GetAllPaging(request);
            if(clubs.Items != null)
            {
                List<ViewClub> items = new List<ViewClub>();
                clubs.Items.ForEach(item =>
                {
                    ViewClub club = transformViewClub(item);
                    items.Add(club);
                });
                return new PagingResult<ViewClub>(items, clubs.TotalCount, clubs.CurrentPage, clubs.PageSize);
            }

            throw new NullReferenceException("Not found any club"); 
        }

        public async Task<ViewClub> GetByClub(int id)
        {
            Club club = await _clubRepo.Get(id);
            if(club != null)
            {
                return transformViewClub(club);                
            }

            throw new NullReferenceException("Not found this club");
        }

        public Task<List<ViewClub>> GetByCompetition(int competitionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewClub>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClub> Insert(ClubInsertModel club)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ClubUpdateModel club)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
