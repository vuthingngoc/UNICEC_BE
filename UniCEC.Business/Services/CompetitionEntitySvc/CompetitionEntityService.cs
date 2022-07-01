using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Business.Services.CompetitionEntitySvc
{
    public class CompetitionEntityService : ICompetitionEntityService
    {

        private ICompetitionEntityRepo _competitionEntityRepo;
        //Add 
        private ICompetitionRepo _competitionRepo;
        private IFileService _fileService;
        private IClubRepo _clubRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private IMemberRepo _memberRepo;
        private DecodeToken _decodeToken;


        public CompetitionEntityService(ICompetitionEntityRepo competitionEntityRepo,
                                        ICompetitionRepo competitionRepo,
                                        IFileService fileService,
                                        IClubRepo clubRepo,
                                        IMemberInCompetitionRepo memberInCompetitionRepo,
                                        IMemberRepo memberRepo)
        {
            _competitionEntityRepo = competitionEntityRepo;
            _competitionRepo = competitionRepo;
            _fileService = fileService;
            _clubRepo = clubRepo;
            _memberRepo = memberRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _decodeToken = new DecodeToken();
        }

        //State Draft - Approve for Sponsor, Influncer
        //Every State for Image

        public async Task<List<ViewCompetitionEntity>> AddImage(ImageInsertModel model, string token)
        {
            try
            {

                List<ViewCompetitionEntity> ViewCompetitionEntities = new List<ViewCompetitionEntity>();

                if (model.CompetitionId == 0 || model.ClubId == 0 || model.Images.Count < 0)
                    throw new ArgumentNullException("Competition Id NULL || ClubId NULL || Image is NULL");

                foreach (AddImageModel modelItem in model.Images)
                {
                    if (string.IsNullOrEmpty(modelItem.Base64StringImg)) throw new ArgumentNullException("Image is NULL");
                }

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, false);
                if (Check)
                {

                    Competition competition = await _competitionRepo.Get(model.CompetitionId);



                    //------------ Insert Competition-Entities-----------
                    foreach (AddImageModel modelItem in model.Images)
                    {

                        string Url = await _fileService.UploadFile(modelItem.Base64StringImg);
                        CompetitionEntity competitionEntity = new CompetitionEntity()
                        {
                            CompetitionId = model.CompetitionId,
                            Name = modelItem.Name,
                            ImageUrl = Url,
                            EntityTypeId = 1, //1 là hình ảnh
                        };

                        int id = await _competitionEntityRepo.Insert(competitionEntity);

                        if (id > 0)
                        {
                            CompetitionEntity entity = await _competitionEntityRepo.Get(id);

                            ViewCompetitionEntities.Add(await TransferViewCompetitionEntity(entity));
                        }

                    }
                    return (ViewCompetitionEntities.Count > 0) ? ViewCompetitionEntities : null;
                }
                //end if check
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ViewCompetitionEntity>> AddInfluencer(InfluencerInsertModel model, string token)
        {
            try
            {
                List<ViewCompetitionEntity> ViewCompetitionEntities = new List<ViewCompetitionEntity>();

                if (model.CompetitionId == 0 || model.ClubId == 0 || model.Influencers.Count < 0)
                    throw new ArgumentNullException("Competition Id NULL || ClubId NULL || Influencer is NULL");

                foreach (AddInfluencerModel modelItem in model.Influencers)
                {
                    if (string.IsNullOrEmpty(modelItem.Base64StringImg) || string.IsNullOrEmpty(modelItem.Name))
                        throw new ArgumentNullException("Image of Influencer is NULL || Influencer name is NULL");
                }

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, false);
                if (Check)
                {

                    //Chỉ Cho những Trạng Thái này update những trạng thái trước khi publish
                    Competition comp = await _competitionRepo.Get(model.CompetitionId);
                    if (comp.Status != CompetitionStatus.Draft || comp.Status != CompetitionStatus.Approve)
                        throw new ArgumentException("Competition State is not suitable to do this action");


                    //------------ Insert Competition-Entities-----------
                    foreach (AddInfluencerModel modelItem in model.Influencers)
                    {

                        string Url = await _fileService.UploadFile(modelItem.Base64StringImg);
                        CompetitionEntity competitionEntity = new CompetitionEntity()
                        {
                            CompetitionId = model.CompetitionId,
                            Name = modelItem.Name,
                            ImageUrl = Url,
                            EntityTypeId = 2, //2 là influencer
                        };

                        int id = await _competitionEntityRepo.Insert(competitionEntity);

                        if (id > 0)
                        {
                            CompetitionEntity entity = await _competitionEntityRepo.Get(id);

                            ViewCompetitionEntities.Add(await TransferViewCompetitionEntity(entity));
                        }

                    }
                    return (ViewCompetitionEntities.Count > 0) ? ViewCompetitionEntities : null;
                }
                //end if check
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<ViewCompetitionEntity>> AddSponsor(SponsorInsertModel model, string token)
        {
            try
            {
                List<ViewCompetitionEntity> ViewCompetitionEntities = new List<ViewCompetitionEntity>();

                if (model.CompetitionId == 0 || model.ClubId == 0 || model.Sponsors.Count < 0)
                    throw new ArgumentNullException("Competition Id NULL || ClubId NULL || Sponsor is NULL");

                foreach (AddSponsorModel modelItem in model.Sponsors)
                {
                    if (string.IsNullOrEmpty(modelItem.Base64StringImg)
                       || string.IsNullOrEmpty(modelItem.Name)
                       || string.IsNullOrEmpty(modelItem.Email)
                       || string.IsNullOrEmpty(modelItem.Description)
                       || string.IsNullOrEmpty(modelItem.Website))
                        throw new ArgumentNullException("Image of Sponsor is NULL || Name is NULL || Email is NULL || Description is NULL|| Website is NULL");
                }

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, false);
                if (Check == false) return null;

                //Chỉ Cho những Trạng Thái này update những trạng thái trước khi publish
                Competition comp = await _competitionRepo.Get(model.CompetitionId);
                if (comp.Status != CompetitionStatus.Draft || comp.Status != CompetitionStatus.Approve)
                    throw new ArgumentException("Competition State is not suitable to do this action");

                foreach (AddSponsorModel modelItem in model.Sponsors)
                {
                    string Url = await _fileService.UploadFile(modelItem.Base64StringImg);
                    CompetitionEntity competitionEntity = new CompetitionEntity()
                    {

                        CompetitionId = model.CompetitionId,
                        Name = modelItem.Name,
                        ImageUrl = Url,
                        EntityTypeId = 3, //3 là Sponsor
                        Description = modelItem.Description,
                        Website = modelItem.Website,
                        Email = modelItem.Email,
                    };

                    int id = await _competitionEntityRepo.Insert(competitionEntity);

                    if (id > 0)
                    {
                        CompetitionEntity entity = await _competitionEntityRepo.Get(id);

                        ViewCompetitionEntities.Add(await TransferViewCompetitionEntity(entity));
                    }

                }

                return (ViewCompetitionEntities.Count > 0) ? ViewCompetitionEntities : null;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCompetitionEntity(CompetitionEntityDeleteModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0 || model.ClubId == 0 || model.CompetitionEntityId == 0)
                    throw new ArgumentNullException("Competition Id NULL || ClubId NULL || Competition Entity Id is NULL");

                CompetitionEntity entity = await _competitionEntityRepo.Get(model.CompetitionId);
                if (entity == null) throw new ArgumentException("Competition Entity not found ");

                Competition competition = await _competitionRepo.Get(entity.CompetitionId);
                if ((entity.EntityTypeId == 2 || entity.EntityTypeId == 3) == true
                    && (competition.Status != CompetitionStatus.Draft || competition.Status != CompetitionStatus.Approve) == true)
                    throw new ArgumentException("Can't Remove Sponsor or Influencer");

                if (entity.CompetitionId != model.CompetitionId) throw new ArgumentException("Competition Entity not belong to this Competition ");

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, false);

                if (Check == false) return false;

                await _competitionEntityRepo.DeleteCompetitionEntity(model.CompetitionEntityId);

                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ViewCompetitionEntity> TransferViewCompetitionEntity(CompetitionEntity entity)
        {
            //get IMG from Firebase                        
            string imgUrl;
            try
            {
                imgUrl = await _fileService.GetUrlFromFilenameAsync(entity.ImageUrl);
            }
            catch (Exception)
            {
                imgUrl = "";
            }

            return new ViewCompetitionEntity()
            {
                Id = entity.Id,
                Name = entity.Name,
                CompetitionId = entity.CompetitionId,
                ImageUrl = imgUrl,
                EntityTypeId = entity.EntityTypeId,
                EntityTypeName = entity.EntityType.Name,
                Website = (entity.Website != null) ? entity.Website : null,
                Email = (entity.Email != null) ? entity.Email : null,
                Description = (entity.Description != null) ? entity.Description : null,
            };
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
                if (isAllow.CompetitionRoleId != 1 || isAllow.CompetitionRoleId != 2) throw new UnauthorizedAccessException("Only role Manager can do this action");
                return true;
            }
            else
            {
                return true;
            }
        }


    }
}

