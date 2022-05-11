using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public class ClubService : IClubService
    {
        private IClubRepo _clubRepo;
        private IClubActivityRepo _clubActivityRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionInClubRepo _competitionInClubRepo;

        public ClubService(IClubRepo clubRepo, IClubActivityRepo clubActivityRepo
                            , IMemberRepo memberRepo, ICompetitionInClubRepo competitionInClubRepo)
        {
            _clubRepo = clubRepo;
            _clubActivityRepo = clubActivityRepo;
            _memberRepo = memberRepo;
            _competitionInClubRepo = competitionInClubRepo;
        }

        public async Task<ViewClub> GetByClub(string token, int id)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var roleClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoleId"));
            var universityClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int roleId = Int32.Parse(roleClaim.Value);
            int universityId = Int32.Parse(universityClaim.Value);

            ViewClub club = await _clubRepo.GetById(id, roleId, universityId);
            if (club == null) throw new NullReferenceException("Not found this club");

            // add more info
            club.TotalActivity = await _clubActivityRepo.GetTotalActivityByClub(id);
            club.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(id) + club.TotalActivity;
            club.MemberIncreaseLastMonth = await _memberRepo.GetQuantityNewMembersByClub(id);

            return club;
        }

        public async Task<PagingResult<ViewClub>> GetByCompetition(string token, int competitionId, PagingRequest request)
        {
            PagingResult<ViewClub> clubs = await _clubRepo.GetByCompetition(competitionId, request);
            if (clubs == null) throw new NullReferenceException("Not found any club with this competition id");
            return clubs;
        }

        public async Task<PagingResult<ViewClub>> GetByName(string token, int universityId, string name, PagingRequest request)
        {
            PagingResult<ViewClub> clubs = await _clubRepo.GetByName(universityId, name, request);
            if (clubs == null) throw new NullReferenceException("Not found any club with this name");

            // add more info
            foreach(ViewClub element in clubs.Items)
            {
                element.TotalActivity = await _clubActivityRepo.GetTotalActivityByClub(element.Id);
                element.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(element.Id) + element.TotalActivity;
                element.MemberIncreaseLastMonth = await _memberRepo.GetQuantityNewMembersByClub(element.Id);
            }

            return clubs;
        }

        public async Task<List<ViewClub>> GetByUser(string token)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
            int userId = Int32.Parse(claim.Value);

            List<ViewClub> clubs = await _clubRepo.GetByUser(userId);
            if (clubs == null) throw new NullReferenceException("This user is not a member of any clubs");

            // add more info
            foreach (ViewClub element in clubs)
            {
                element.TotalActivity = await _clubActivityRepo.GetTotalActivityByClub(element.Id);
                element.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(element.Id) + element.TotalActivity;
                element.MemberIncreaseLastMonth = await _memberRepo.GetQuantityNewMembersByClub(element.Id);
            }

            return clubs;
        }

        public async Task<PagingResult<ViewClub>> GetByUniversity(string token, int id, PagingRequest request)
        {
            PagingResult<ViewClub> clubs = await _clubRepo.GetByUniversity(id, request);
            if (clubs == null) throw new NullReferenceException("This university have no any clubs");

            // add more info
            foreach (ViewClub element in clubs.Items)
            {
                element.TotalActivity = await _clubActivityRepo.GetTotalActivityByClub(element.Id);
                element.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(element.Id) + element.TotalActivity;
                element.MemberIncreaseLastMonth = await _memberRepo.GetQuantityNewMembersByClub(element.Id);
            }

            return clubs;
        }

        public async Task<ViewClub> Insert(string token, ClubInsertModel model)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoldId"));
            int roleId = Int32.Parse(claim.Value);
            if (roleId == 2) throw new UnauthorizedAccessException();

            if (string.IsNullOrEmpty(model.Description) || model.UniversityId == 0 || model.TotalMember == 0
                || string.IsNullOrEmpty(model.Name) || model.Founding == DateTime.Parse("1/1/0001 12:00:00 AM"))
                throw new ArgumentNullException("Description Null || UniversityId Null || TotalMember Null || Name Null || Founding Null");

            int clubId = await _clubRepo.CheckExistedClubName(model.UniversityId, model.Name);
            if (clubId > 0) throw new ArgumentException("Duplicated club name");

            // default status when inserting
            bool status = true;
            Club club = new Club()
            {
                Description = model.Description,
                Founding = model.Founding,
                Name = model.Name,
                TotalMember = model.TotalMember,
                UniversityId = model.UniversityId,
                Status = status,
                Image = model.Image,
            };

            int id = await _clubRepo.Insert(club);
            return await _clubRepo.GetById(id, roleId, model.UniversityId);
        }

        public async Task Update(ClubUpdateModel model)
        {
            Club club = await _clubRepo.Get(model.Id);
            if (club == null) throw new NullReferenceException("Not found this club");
            int clubId = await _clubRepo.CheckExistedClubName(club.UniversityId, model.Name);
            if (clubId > 0 && clubId != club.Id) throw new ArgumentException("Duplicated club name");

            if (!string.IsNullOrEmpty(model.Description)) club.Description = model.Description;
            if (model.Founding != DateTime.Parse("1/1/0001 12:00:00 AM")) club.Founding = model.Founding;
            if (!string.IsNullOrEmpty(model.Name)) club.Name = model.Name;
            if (model.TotalMember != 0) club.TotalMember = model.TotalMember;
            club.Status = model.Status;
            if (!string.IsNullOrEmpty(model.Image)) club.Image = model.Image;

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
