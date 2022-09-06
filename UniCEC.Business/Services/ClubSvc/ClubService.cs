using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public class ClubService : IClubService
    {
        private IClubRepo _clubRepo;
        private ICompetitionActivityRepo _competitionActivityRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionInClubRepo _competitionInClubRepo;
        private ICompetitionRepo _competitionRepo;
        private IUserRepo _userRepo;

        private IFileService _fileService;

        private DecodeToken _decodeToken;

        public ClubService(IClubRepo clubRepo, ICompetitionActivityRepo competitionActivityRepo
                            , IMemberRepo memberRepo, ICompetitionInClubRepo competitionInClubRepo
                                , ICompetitionRepo competitionRepo, IFileService fileService, IUserRepo userRepo)
        {
            _clubRepo = clubRepo;
            _competitionActivityRepo = competitionActivityRepo;
            _memberRepo = memberRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _competitionRepo = competitionRepo;
            _fileService = fileService;
            _userRepo = userRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task<ViewClub> AddMoreInfoClub(ViewClub club)
        {
            club.TotalActivity = await _competitionActivityRepo.GetTotalActivityByClub(club.Id);
            club.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(club.Id);
            club.MemberIncreaseThisMonth = await _memberRepo.GetQuantityNewMembersByClub(club.Id);
            club.Image = await GetUrlImage(club.Image, club.Id);

            return club;
        }

        private async Task<PagingResult<ViewClub>> AddMoreInfoClub(PagingResult<ViewClub> clubs)
        {
            foreach (ViewClub element in clubs.Items)
            {
                element.TotalActivity = await _competitionActivityRepo.GetTotalActivityByClub(element.Id);
                element.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(element.Id);
                element.MemberIncreaseThisMonth = await _memberRepo.GetQuantityNewMembersByClub(element.Id);
                element.Image = await GetUrlImage(element.Image, element.Id);                
            }

            return clubs;
        }

        private async Task<List<ViewClub>> AddMoreInfoClub(List<ViewClub> clubs)
        {
            foreach (ViewClub club in clubs)
            {
                club.TotalActivity = await _competitionActivityRepo.GetTotalActivityByClub(club.Id);
                club.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(club.Id);
                club.MemberIncreaseThisMonth = await _memberRepo.GetQuantityNewMembersByClub(club.Id);
                club.Image = await GetUrlImage(club.Image, club.Id);
            }

            return clubs;
        }

        private async Task<string> GetUrlImage(string imageUrl, int clubId)
        {
            string fullPathImage = await _fileService.GetUrlFromFilenameAsync(imageUrl) ?? "";
            if (!string.IsNullOrEmpty(imageUrl) && !imageUrl.Equals(fullPathImage)) // for old data save filename in  db
            {
                Club club = await _clubRepo.Get(clubId);
                club.Image = fullPathImage;
                await _clubRepo.Update();
            }

            return fullPathImage;
        }

        public async Task<ViewClub> GetById(string token, int id)
        {
            bool? status = null;

            int roleId = _decodeToken.Decode(token, "RoleId");
            if (roleId.Equals(3)) status = true; // for student

            ViewClub club = await _clubRepo.GetById(id, status);
            if (club == null) throw new NullReferenceException("Not found this club");

            if (!roleId.Equals(4)) // not system admin
            {
                int uniId = _decodeToken.Decode(token, "UniversityId");
                if (!uniId.Equals(club.UniversityId)) throw new UnauthorizedAccessException("You do not have permission to access this club");
            }

            return await AddMoreInfoClub(club);
        }

        public async Task<PagingResult<ViewClub>> GetByCompetition(string token, int competitionId, PagingRequest request) // check again
        {
            int roleId = _decodeToken.Decode(token, "RoleId");

            if (!roleId.Equals(4))// not system admin
            {
                int universityId = _decodeToken.Decode(token, "UniversityId");
                CompetitionScopeStatus scope = await _competitionRepo.GetScopeCompetition(competitionId);
                if (scope.Equals(CompetitionScopeStatus.InterUniversity))
                {
                    bool isValid = await _competitionRepo.CheckExisteUniInCompetition(competitionId, universityId);
                    if (!isValid) throw new UnauthorizedAccessException("You do not have permission to access this resource");
                }
            }

            PagingResult<ViewClub> clubs = await _clubRepo.GetByCompetition(competitionId, request);
            if (clubs == null) throw new NullReferenceException("Not found any club with this competition id");

            return await AddMoreInfoClub(clubs);
        }

        public async Task<PagingResult<ViewClub>> GetByConditions(string token, ClubRequestModel request) // not check status for admin and anothers yet
        {
            int roleId = _decodeToken.Decode(token, "RoleId");

            if (!roleId.Equals(4)) // not system admin
            {
                int uniId = _decodeToken.Decode(token, "UniversityId");
                if (!request.UniversityId.Equals(uniId)) throw new UnauthorizedAccessException("You do not have permission to access this club");
            }

            if (roleId.Equals(3)) request.Status = true; // default status for student

            PagingResult<ViewClub> clubs = await _clubRepo.GetByConditions(request);
            if (clubs == null) throw new NullReferenceException("Not found any club with this name");

            return await AddMoreInfoClub(clubs);
        }

        public async Task<List<ViewClub>> GetByUser(string token, int id)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (roleId.Equals(4) || roleId.Equals(2)) throw new UnauthorizedAccessException("You can not access this resource");

            int userId = _decodeToken.Decode(token, "Id");
            int universityId = _decodeToken.Decode(token, "UniversityId");

            bool isSameUni = await _userRepo.CheckExistedUser(universityId, id);

            if (userId.Equals(id) || (roleId.Equals(1) && isSameUni))
            {
                List<ViewClub> clubs = await _clubRepo.GetByUser(id);
                if (clubs == null) throw new NullReferenceException("This user is not a member of any clubs");

                // add more info
                return await AddMoreInfoClub(clubs);
            }

            throw new UnauthorizedAccessException("You can not access this resource");
        }

        //TA
        public async Task<List<ViewClub>> GetClubByUni(string token)
        {
            int universityId = _decodeToken.Decode(token, "UniversityId");
            List<ViewClub> clubs = await _clubRepo.GetByUni(universityId);
            if (clubs == null) throw new NullReferenceException("This user is not a member of any clubs");

            // add more info
            return await AddMoreInfoClub(clubs);
        }

        public async Task<PagingResult<ViewClub>> GetByManager(string token, ClubRequestByManagerModel request)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, request.clubId);
            if (!clubRoleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            PagingResult<ViewClub> clubs = await _clubRepo.GetByManager(request);
            if (clubs == null) throw new NullReferenceException();

            return await AddMoreInfoClub(clubs);
        }

        public async Task<ViewClub> Insert(string token, ClubInsertModel model)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (roleId.Equals(4)) throw new UnauthorizedAccessException("You can not access this resource");// if system admin

            int universityId = _decodeToken.Decode(token, "UniversityId");

            if (!roleId.Equals(1) && !universityId.Equals(model.UniversityId))
                throw new UnauthorizedAccessException("You do not have permission to add new club");

            if (string.IsNullOrEmpty(model.Description) || model.UniversityId == 0 ||
                    string.IsNullOrEmpty(model.Name) || model.Founding == DateTime.MinValue)
                throw new ArgumentNullException("Description Null || UniversityId Null || Name Null || Founding Null");

            int checkClubId = await _clubRepo.CheckExistedClubName(model.UniversityId, model.Name);
            if (checkClubId > 0) throw new ArgumentException("Duplicated club name");

            int statusValidLeader = _memberRepo.CheckValidLeader(model.UserId, model.UniversityId);// 0 is valid case
            if (statusValidLeader.Equals(-1)) throw new ArgumentException("This user is not in the university");
            else if (statusValidLeader.Equals(1)) throw new ArgumentException("This user is leader in another club");

            Club club = new Club()
            {
                Description = model.Description,
                Founding = model.Founding,
                Name = model.Name,
                TotalMember = 1, // default number member 
                UniversityId = model.UniversityId,
                Status = true, // default status 
                Image = await _fileService.UploadFile(model.Image),
                ClubContact = model.ClubContact,
                ClubFanpage = model.ClubFanpage
            };
            int clubId = await _clubRepo.Insert(club);

            DateTime currentTime = new LocalTime().GetLocalTime().DateTime;
            Member member = new Member()
            {
                ClubId = clubId,
                ClubRoleId = 1, // leader
                StartTime = currentTime,
                UserId = model.UserId,
                Status = MemberStatus.Active // default status 
            };
            await _memberRepo.Insert(member);

            ViewClub viewClub = await _clubRepo.GetById(clubId, club.Status);

            return await AddMoreInfoClub(viewClub);
        }

        public async Task Update(string token, ClubUpdateModel model)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.Id);

            // if role is not leader or vice president
            if (!clubRoleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to update this club");

            Club club = await _clubRepo.Get(model.Id);
            if (club == null) throw new NullReferenceException("Not found this club");

            if (!string.IsNullOrEmpty(model.Name))
            {
                int clubId = await _clubRepo.CheckExistedClubName(club.UniversityId, model.Name);
                if (clubId > 0 && clubId != club.Id) throw new ArgumentException("Duplicated club name");
                club.Name = model.Name;
            }

            if (!string.IsNullOrEmpty(model.Description)) club.Description = model.Description;

            if (model.Founding != DateTime.MinValue) club.Founding = model.Founding;

            club.TotalMember = await _memberRepo.GetTotalMembersByClub(club.Id);

            if (model.Status != false) club.Status = model.Status;

            if(!string.IsNullOrEmpty(model.Image))
            {
                if (model.Image.Contains("https"))
                {
                    club.Image = model.Image;
                }
                else // base64
                {
                    club.Image = (string.IsNullOrEmpty(club.Image))
                                ? await _fileService.UploadFile(model.Image) // insert
                                : await _fileService.UploadFile(club.Image, model.Image);// update
                }   
            }

            if (!string.IsNullOrEmpty(model.ClubContact)) club.ClubContact = model.ClubContact;

            if (!string.IsNullOrEmpty(model.ClubFanpage)) club.ClubFanpage = model.ClubFanpage;

            await _clubRepo.Update();
        }

        public async Task Update(string token, int clubId, bool status) // for university admin
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (roleId.Equals(4)) throw new UnauthorizedAccessException("You can not access this resource");

            int uniId = _decodeToken.Decode(token, "UniversityId");
            int universityId = await _clubRepo.GetUniversityByClub(clubId);

            if (!roleId.Equals(1) && !uniId.Equals(universityId))
                throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Club club = await _clubRepo.Get(clubId);
            if (club == null) throw new NullReferenceException("Not found this club");
            club.Status = status;
            await _clubRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (roleId.Equals(4)) throw new UnauthorizedAccessException("You can not access this resource"); // if system admin 

            int universityId = _decodeToken.Decode(token, "UniversityId");

            Club club = await _clubRepo.Get(id);
            if (club == null) throw new NullReferenceException("Not found this club");

            if (!roleId.Equals(1) && !universityId.Equals(club.UniversityId))
                throw new UnauthorizedAccessException("You do not have permission to delete this club");

            club.Status = false; // default status for delete
            await _clubRepo.Update();

            await _memberRepo.UpdateStatusDeletedClub(id);
        }

        public async Task<ViewActivityOfClubModel> GetActivityOfClubById(string token, int clubId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, clubId);

            // if role is not leader or vice president
            if (!clubRoleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to get information");

            Club club = await _clubRepo.Get(clubId);
            if (club == null) throw new NullReferenceException("Not found this club");

            ViewActivityOfClubModel result = await _clubRepo.GetActivityOfClubById(clubId);
            return result;
        }
    }
}
