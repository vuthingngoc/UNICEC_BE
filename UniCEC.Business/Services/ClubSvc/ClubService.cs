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

        private ViewClub TransformViewClub(Club club)
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
            if (clubs.Items != null)
            {
                List<ViewClub> items = new List<ViewClub>();
                clubs.Items.ForEach(item =>
                {
                    ViewClub club = TransformViewClub(item);
                    items.Add(club);
                });
                return new PagingResult<ViewClub>(items, clubs.TotalCount, clubs.CurrentPage, clubs.PageSize);
            }

            throw new NullReferenceException("Not found any club");
        }

        public async Task<ViewClub> GetByClub(int id)
        {
            Club club = await _clubRepo.Get(id);
            if (club != null)
            {
                return TransformViewClub(club);
            }

            throw new NullReferenceException("Not found this club");
        }

        public async Task<PagingResult<ViewClub>> GetByCompetition(int competitionId, PagingRequest request)
        {
            PagingResult<Club> listClub = await _clubRepo.GetByCompetition(competitionId, request);
            if (listClub == null) throw new NullReferenceException("Not found any club with this competition id");

            List<ViewClub> clubs = new List<ViewClub>();
            listClub.Items.ForEach(element =>
            {
                ViewClub club = TransformViewClub(element);
                clubs.Add(club);
            });
            return new PagingResult<ViewClub>(clubs, listClub.TotalCount, listClub.CurrentPage, listClub.PageSize);
        }

        public async Task<PagingResult<ViewClub>> GetByName(string name, PagingRequest request)
        {
            PagingResult<Club> listClub = await _clubRepo.GetByName(name, request);
            if (listClub == null) throw new NullReferenceException("Not found any club with this name");

            List<ViewClub> clubs = new List<ViewClub>();
            listClub.Items.ForEach(element =>
            {
                ViewClub club = TransformViewClub(element);
                clubs.Add(club);
            });
            return new PagingResult<ViewClub>(clubs, listClub.TotalCount, listClub.CurrentPage, listClub.PageSize);
        }

        public async Task<ViewClub> Insert(ClubInsertModel club)
        {
            if (club == null) throw new ArgumentNullException("Null argument");

            int clubId = await _clubRepo.CheckExistedClubName(club.UniversityId, club.Name);
            if (clubId > 0) throw new ArgumentException("Duplicated club name");

            Club clubObject = new Club()
            {
                Description = club.Description,
                Founding = club.Founding,
                Name = club.Name,
                TotalMember = club.TotalMember,
                UniversityId = club.UniversityId,
                Status = club.Status,
            };
            int id = await _clubRepo.Insert(clubObject);
            if (id > 0)
            {
                clubObject.Id = id;
                return TransformViewClub(clubObject);
            }

            return null;
        }

        public async Task<bool> Update(ClubUpdateModel club)
        {
            if (club == null) throw new ArgumentNullException("Null argument");

            Club clubObject = await _clubRepo.Get(club.Id);
            if (clubObject == null) throw new NullReferenceException("Not found this club");
            int clubId = await _clubRepo.CheckExistedClubName(clubObject.UniversityId, club.Name);
            if (clubId > 0 && clubId != clubObject.Id) throw new ArgumentException("Duplicated club name");

            clubObject.Description = club.Description;
            clubObject.Founding = club.Founding;
            clubObject.Name = club.Name;
            clubObject.TotalMember = club.TotalMember;
            clubObject.Status = club.Status;

            return await _clubRepo.Update();
        }

        public async Task<bool> Delete(int id)
        {
            Club clubObject = await _clubRepo.Get(id);
            if (clubObject != null) throw new NullReferenceException("Not found this club");
            if (clubObject.Status == false) return true;
            clubObject.Status = false;
            return await _clubRepo.Update();
        }
    }
}
