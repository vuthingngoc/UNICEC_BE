using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
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
        private ICompetitionRepo _competitionRepo;
        private ITermRepo _termRepo;
        private IClubHistoryRepo _clubHistoryRepo;

        public ClubService(IClubRepo clubRepo, IClubActivityRepo clubActivityRepo, ITermRepo termRepo
                            , IMemberRepo memberRepo, ICompetitionInClubRepo competitionInClubRepo, IClubHistoryRepo clubHistoryRepo
                                , ICompetitionRepo competitionRepo)
        {
            _clubRepo = clubRepo;
            _clubActivityRepo = clubActivityRepo;
            _memberRepo = memberRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _competitionRepo = competitionRepo;
            _termRepo = termRepo;
            _clubHistoryRepo = clubHistoryRepo;
        }

        public async Task<ViewClub> GetByClub(string token, int id, int universityId)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var roleClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoleId"));
            var universityClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int roleId = Int32.Parse(roleClaim.Value);
            int uniId = Int32.Parse(universityClaim.Value);

            // is student role
            if (roleId.Equals(3) && !uniId.Equals(universityId)) throw new UnauthorizedAccessException("You do not have permission to access this club");  

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
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var roleClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoleId"));
            var universityClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int roleId = Int32.Parse(roleClaim.Value);
            int universityId = Int32.Parse(universityClaim.Value);

            if(roleId == 3)// is student role
            {
                bool isPublic = await _competitionRepo.CheckIsPublic(competitionId);
                if (!isPublic)
                {
                    List<int> universityIds = await _competitionRepo.GetUniversityByCompetition(competitionId);
                    if (universityIds == null) throw new NullReferenceException("Not found this competition");

                    bool isSameUni = false;
                    foreach(int id in universityIds)
                    {
                        if (id.Equals(universityId))
                        {
                            isSameUni = true;
                            break;
                        }
                    }

                    if (isSameUni == false) throw new UnauthorizedAccessException("You do not have permission to access this competition");
                }
            }

            PagingResult<ViewClub> clubs = await _clubRepo.GetByCompetition(competitionId, request);
            if (clubs == null) throw new NullReferenceException("Not found any club with this competition id");
            return clubs;
        }

        // not finish yet
        public async Task<PagingResult<ViewClub>> GetByName(string token, int universityId, string name, PagingRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var universityIdClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            var roleClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoleId"));
            int uniId = Int32.Parse(universityIdClaim.Value);
            int roleId = Int32.Parse(roleClaim.Value);

            // student and sponsor
            if (roleId != 1 && !uniId.Equals(universityId)) throw new UnauthorizedAccessException("You do not have permission to access this club");

            PagingResult<ViewClub> clubs = await _clubRepo.GetByName(universityId, roleId, name, request);
            if (clubs == null) throw new NullReferenceException("Not found any club with this name");

            // add more info
            foreach (ViewClub element in clubs.Items)
            {
                element.TotalActivity = await _clubActivityRepo.GetTotalActivityByClub(element.Id);
                element.TotalEvent = await _competitionInClubRepo.GetTotalEventOrganizedByClub(element.Id) + element.TotalActivity;
                element.MemberIncreaseLastMonth = await _memberRepo.GetQuantityNewMembersByClub(element.Id);
            }

            return clubs;
        }

        public async Task<List<ViewClub>> GetByUser(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
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
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var universityClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            var roleClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoleId"));
            int universityId = Int32.Parse((universityClaim.Value).ToString());
            int roleId = Int32.Parse((roleClaim.Value).ToString());

            // student role
            if (roleId == 3 && !universityId.Equals(id)) throw new UnauthorizedAccessException("You do not have permission to access this club");

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
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var roleClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("RoleId"));
            var userIdClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
            if (roleClaim == null || userIdClaim == null) throw new ArgumentException();
            int roleId = Int32.Parse(roleClaim.Value);
            int userId = Int32.Parse(userIdClaim.Value);
            if (roleId == 2) throw new UnauthorizedAccessException("You do not have permission to add new club");

            if (string.IsNullOrEmpty(model.Description) || model.UniversityId == 0 || model.TotalMember == 0
                || string.IsNullOrEmpty(model.Name) || model.Founding == DateTime.Parse("1/1/0001 12:00:00 AM"))
                throw new ArgumentNullException("Description Null || UniversityId Null || TotalMember Null || Name Null || Founding Null");

            int clubId = await _clubRepo.CheckExistedClubName(model.UniversityId, model.Name);
            if (clubId > 0) throw new ArgumentException("Duplicated club name");

            Club club = new Club()
            {
                Description = model.Description,
                Founding = model.Founding,
                Name = model.Name,
                TotalMember = model.TotalMember,
                UniversityId = model.UniversityId,
                Status = true, // default status when insert
                Image = model.Image,
            };
            int id = await _clubRepo.Insert(club);

            DateTime currentTime = new LocalTime().GetLocalTime().DateTime;
            Member member = new Member()
            {
                StudentId = userId,
                JoinDate = currentTime
            };
            int memberId = await _memberRepo.Insert(member);

            Term term = new Term()
            {
                Name = "First Term", // default name
                CreateTime = currentTime,
                EndTime = currentTime.AddYears(1),
                Status = true // default status
            };
            int termId = await _termRepo.Insert(term);

            ClubHistory clubHistory = new ClubHistory()
            {
                ClubId = id,
                ClubRoleId = 1, // leader
                MemberId = memberId,
                TermId = termId,
                StartTime = currentTime,
                Status = ClubHistoryStatus.Active
            };
            int clubHistoryId = await _clubHistoryRepo.Insert(clubHistory);

            return await _clubRepo.GetById(clubId, roleId, model.UniversityId);
        }

        public async Task Update(string token, ClubUpdateModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
            int userId = Int32.Parse(claim.Value);
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.Id);
            // if role is not leader or vice president
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to update this club");

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

        public async Task Delete(string token, int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userIdClaim = tokenHandler.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
            int userId = Int32.Parse(userIdClaim.Value);

            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, id);
            if (!clubRoleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to delete this club");

            Club clubObject = await _clubRepo.Get(id);
            if (clubObject == null) throw new NullReferenceException("Not found this club");
            clubObject.Status = false;
            await _clubRepo.Update();
        }
    }
}
