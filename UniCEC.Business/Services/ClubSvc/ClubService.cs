using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
                Image = club.Image
            };
        }

        public async Task<PagingResult<ViewClub>> GetAllPaging(PagingRequest request)
        {
            PagingResult<Club> clubs = await _clubRepo.GetAllPaging(request);
            if (clubs != null)
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

        public async Task<List<ViewClub>> GetByUser(int userId, string token)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
            int id = Int32.Parse(claim.Value);
            if (id != userId) throw new NullReferenceException("Not Found");

            List<Club> listClub = await _clubRepo.GetByUser(userId);
            if (listClub == null) throw new NullReferenceException("This user is not a member of any clubs");

            List<ViewClub> clubs = new List<ViewClub>();
            listClub.ForEach(element =>
            {
                ViewClub club = TransformViewClub(element);
                clubs.Add(club);
            });
            return clubs;           
        }

        public async Task<PagingResult<ViewClub>> GetByUniversity(string token, PagingRequest request)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int universityId = Int32.Parse(claim.Value);

            PagingResult<Club> clubs = await _clubRepo.GetByUniversity(universityId, request);
            if (clubs == null) throw new NullReferenceException("This university have no any clubs");

            List<ViewClub> items = new List<ViewClub>();
            clubs.Items.ForEach(element =>
            {
                ViewClub club = TransformViewClub(element);
                items.Add(club);
            });

            return new PagingResult<ViewClub>(items, clubs.TotalCount, request.CurrentPage, request.PageSize);
        }

        public async Task<ViewClub> Insert(ClubInsertModel club)
        {
            if (string.IsNullOrEmpty(club.Description) || club.UniversityId == 0 || club.TotalMember == 0 
                || string.IsNullOrEmpty(club.Name) || club.Founding == DateTime.Parse("1/1/0001 12:00:00 AM")) 
                    throw new ArgumentNullException("Description Null || UniversityId Null || TotalMember Null || Name Null || Founding Null");

            int clubId = await _clubRepo.CheckExistedClubName(club.UniversityId, club.Name);
            if (clubId > 0) throw new ArgumentException("Duplicated club name");

            // default status when inserting
            bool status = true;
            Club clubObject = new Club()
            {
                Description = club.Description,
                Founding = club.Founding,
                Name = club.Name,
                TotalMember = club.TotalMember,
                UniversityId = club.UniversityId,
                Status = status,
                Image = club.Image,
            };
            int id = await _clubRepo.Insert(clubObject);
            if (id > 0)
            {
                clubObject.Id = id;
                return TransformViewClub(clubObject);
            }

            return null;
        }

        public async Task Update(ClubUpdateModel club)
        {
            Club clubObject = await _clubRepo.Get(club.Id);
            if (clubObject == null) throw new NullReferenceException("Not found this club");
            int clubId = await _clubRepo.CheckExistedClubName(clubObject.UniversityId, club.Name);
            if (clubId > 0 && clubId != clubObject.Id) throw new ArgumentException("Duplicated club name");

            if(!string.IsNullOrEmpty(club.Description)) clubObject.Description = club.Description;
            if(club.Founding != DateTime.Parse("1/1/0001 12:00:00 AM")) clubObject.Founding = club.Founding;
            if(!string.IsNullOrEmpty(club.Name)) clubObject.Name = club.Name;
            if(club.TotalMember != 0) clubObject.TotalMember = club.TotalMember;
            clubObject.Status = club.Status;
            if(!string.IsNullOrEmpty(club.Image)) clubObject.Image = club.Image;

            await _clubRepo.Update();
        }

        public async Task Delete(int id)
        {
            Club clubObject = await _clubRepo.Get(id);
            if (clubObject == null) throw new NullReferenceException("Not found this club");            
            clubObject.Status = false;
            await _clubRepo.Update();
        }
    }
}
