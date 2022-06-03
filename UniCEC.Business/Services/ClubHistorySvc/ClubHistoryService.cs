using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.TermSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.Member;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.ClubHistorySvc
{
    public class ClubHistoryService : IClubHistoryService // Test ...
    {
        private IClubHistoryRepo _clubHistoryRepo;
        private IMemberRepo _memberRepo;
        private ITermService _termService;

        public ClubHistoryService(IClubHistoryRepo clubHistoryRepo, IMemberRepo memberRepo, ITermService termService)
        {
            _clubHistoryRepo = clubHistoryRepo;
            _memberRepo = memberRepo;
            _termService = termService;
        }

        public async Task<PagingResult<ViewClubHistory>> GetByContitions(string token, int clubId, MemberRequestModel request)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userIdClaim = tokenHandler.Claims.FirstOrDefault(claim => claim.Type.ToString().Equals("Id"));
            int userId = Int32.Parse(userIdClaim.Value);

            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            //PagingResult<ViewClubHistory> ClubHistories = await _clubHistoryRepo.GetByConditions(clubId, request);
            //if (ClubHistories == null) throw new NullReferenceException("Not found any previous clubs");
            //return ClubHistories;
            return null;
        }

        public async Task InsertForNewTerm(string token, int clubId, TermInsertModel termModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userIdClaim = tokenHandler.Claims.FirstOrDefault(claim => claim.Type.ToString().Equals("Id"));
            int userId = Int32.Parse(userIdClaim.Value);

            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, clubId);
            // if user is not leader or vice president
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            ViewTerm term = await _termService.Insert(termModel);
            if(term != null)
            {
                // update endtime of old term
                await _termService.CloseOldTermByClub(clubId);

                List<ClubHistory> clubHistories = new List<ClubHistory>();//await _clubHistoryRepo.GetCurrentHistoryByClub(clubId);
                if(clubHistories != null)
                {
                    //await _clubHistoryRepo.UpdateEndTerm(clubId);
                    // insert new records
                    foreach (ClubHistory record in clubHistories)
                    {
                        record.TermId = term.Id;
                        await _clubHistoryRepo.Insert(record);
                    }
                }
            }
        }

        // ???
        public async Task<PagingResult<ViewBasicInfoMember>> GetMembersByClub(int clubId, int termId, PagingRequest request)
        {
            //PagingResult<ViewBasicInfoMember> clubMembers = await _clubHistoryRepo.GetMembersByClub(clubId, termId, request);
            //if (clubMembers == null) throw new NullReferenceException("This club has no any members in this term");
            //return clubMembers;
            return null;
        }

        //Task<PagingResult<ViewClubMember>> IClubHistoryService.GetMembersByClub(int clubId, int termId, PagingRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        // Tien Anh
        //public async Task<ViewBasicInfoMember> GetMemberInCLub(GetMemberInClubModel model)
        //{
        //    ViewBasicInfoMember result = await _clubHistoryRepo.GetMemberInCLub(model);
        //    return result;
        //}
    }
}
