using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;

namespace UniCEC.Business.Services.ActivitiesEntitySvc
{
    public class ActivitiesEntityService : IActivitiesEntityService
    {
        private IActivitiesEntityRepo _activitiesEntityRepo;
        //Add
        private ICompetitionActivityRepo _competitionActivityRepo;
        private ICompetitionRepo _competitionRepo;
        private IFileService _fileService;
        private IClubRepo _clubRepo;
        private IMemberRepo _memberRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private DecodeToken _decodeToken;


        public ActivitiesEntityService(IActivitiesEntityRepo activitiesEntityRepo,
                                       ICompetitionActivityRepo competitionActivityRepo,
                                       ICompetitionRepo competitionRepo,
                                       IFileService fileService,
                                       IClubRepo clubRepo,
                                       IMemberInCompetitionRepo memberInCompetitionRepo,
                                       IMemberRepo memberRepo)
        {
            _activitiesEntityRepo = activitiesEntityRepo;
            _competitionActivityRepo = competitionActivityRepo;
            _competitionRepo = competitionRepo;
            _fileService = fileService;
            _clubRepo = clubRepo;
            _memberRepo = memberRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<List<ViewActivitiesEntity>> AddActivitiesEntity(ActivitiesEntityInsertModel model, string token)
        {
            try
            {
                if (model.CompetitionActivityId == 0
                  || model.ClubId == 0)
                    throw new ArgumentNullException("CompetitionAcitvityId Null || ClubId Null");

                if (model.ListActivitiesEntities.Count.Equals(0)) return null;

                foreach (AddActivitiesEntity modelItem in model.ListActivitiesEntities)
                {
                    if (string.IsNullOrEmpty(modelItem.Base64StringImg)) throw new ArgumentNullException("Image is NULL");
                }                

                //Check Competition Activity Existed
                CompetitionActivity ca = await _competitionActivityRepo.Get(model.CompetitionActivityId);
                if (ca == null) throw new ArgumentException("Competition Activity is not found");

                //Check Status
                if (ca.Status == CompetitionActivityStatus.Cancelling) throw new ArgumentException("Competition Activity is Canceling");

                //Check Condititions
                Competition competition = await _competitionRepo.Get(ca.CompetitionId);
                await CheckMemberInCompetition(token, competition.Id, model.ClubId, false);

                List<ViewActivitiesEntity> ListVae = new List<ViewActivitiesEntity>();

                foreach (AddActivitiesEntity modelItem in model.ListActivitiesEntities)
                {
                    //------------ Insert Activities-Entities-----------
                    ActivitiesEntity activitiesEntity = new ActivitiesEntity();
                    activitiesEntity.CompetitionActivityId = model.CompetitionActivityId;
                    activitiesEntity.Name = modelItem.Name;
                    activitiesEntity.ImageUrl = (modelItem.Base64StringImg.Contains("https"))
                        ? modelItem.Base64StringImg
                        : await _fileService.UploadFile(modelItem.Base64StringImg);                   

                    //ActivitiesEntity ActivitiesEntity = new ActivitiesEntity()
                    //{
                    //    CompetitionActivityId = model.CompetitionActivityId,
                    //    Name = modelItem.Name,
                    //    ImageUrl = Url
                    //};

                    int id = await _activitiesEntityRepo.Insert(activitiesEntity);
                    activitiesEntity.Id = id;              

                    ViewActivitiesEntity vae = new ViewActivitiesEntity()
                    {
                        Id = activitiesEntity.Id,
                        Name = activitiesEntity.Name,
                        CompetitionActivityId = activitiesEntity.CompetitionActivityId,
                        ImageUrl = activitiesEntity.ImageUrl,
                    };

                    ListVae.Add(vae);
                }
                return (ListVae.Count > 0) ? ListVae : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteActivitiesEntity(int CompetitionActivityId, int ClubId, string token)
        {
            try
            {
                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(CompetitionActivityId);
                //Check CompetitionActivity
                if (competitionActivity == null) throw new ArgumentException("Not Found Competition Activity");
                //Check Status
                if (competitionActivity.Status == CompetitionActivityStatus.Cancelling) throw new ArgumentException("Competition Activity is Canceling");

                //Check Condition
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                await CheckMemberInCompetition(token, competition.Id, ClubId, false);

                await _activitiesEntityRepo.DeleteActivitiesEntity(CompetitionActivityId);

                return true;
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
