using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;

namespace UniCEC.Business.Services.CompetitionHistorySvc
{
    public class CompetitionHistoryService : ICompetitionHistoryService
    {
        private ICompetitionHistoryRepo _competitionHistoryRepo;
        private ICompetitionRepo _competitionRepo;
        private IClubRepo _clubRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private IMemberRepo _memberRepo;
        private DecodeToken _decodeToken;
        public CompetitionHistoryService(ICompetitionHistoryRepo competitionHistoryRepo,
                                         ICompetitionRepo competitionRepo,
                                         IClubRepo clubRepo,
                                         IMemberInCompetitionRepo memberInCompetitionRepo,
                                         IMemberRepo memberRepo)
        {
            _competitionHistoryRepo = competitionHistoryRepo;
            _competitionRepo = competitionRepo;
            _clubRepo = clubRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _memberRepo = memberRepo;
            _decodeToken = new DecodeToken();
        }



        public async Task<List<ViewCompetitionHistory>> GetAllHistoryOfCompetition(int competitionId, int clubId, string token)
        {
            try
            {
                if (competitionId < 0 || clubId < 0) throw new ArgumentException("Competition Id NULL || Club Id NULL");

                bool Check = await CheckMemberInCompetition(token, competitionId, clubId, false);
                if (Check == false) throw new NullReferenceException();

                List<ViewCompetitionHistory> result = await _competitionHistoryRepo.GetAllHistoryOfCompetition(competitionId);

                return (result != null) ? result : throw new NullReferenceException();

            }
            catch (Exception)
            {
                throw;
            }
        }



        private async Task<bool> CheckMemberInCompetition(string Token, int CompetitionId, int ClubId, bool isOrganization)
        {
            //------------- CHECK Competition in system
            Competition competition = await _competitionRepo.Get(CompetitionId);
            if (competition == null) throw new ArgumentException("Competition or Event not found ");

            //------------- CHECK Club in system
            Club club = await _clubRepo.Get(ClubId);
            if (club == null) throw new ArgumentException("Club in not found");

            //------------- CHECK Is Member in Club
            int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(Token, "Id"), club.Id);
            Member member = await _memberRepo.Get(memberId);
            if (member == null) throw new UnauthorizedAccessException("You aren't member in Club");

            //------------- CHECK User is in CompetitionManger table                
            MemberInCompetition isAllow = await _memberInCompetitionRepo.GetMemberInCompetition(CompetitionId, memberId);
            if (isAllow == null) throw new UnauthorizedAccessException("You do not in Competition Manager ");

            if (isOrganization)
            {
                //------------- CHECK Role Is highest role
                //1,2 accept
                if (isAllow.CompetitionRoleId >= 3) throw new UnauthorizedAccessException("Only role Manager can do this action");
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
