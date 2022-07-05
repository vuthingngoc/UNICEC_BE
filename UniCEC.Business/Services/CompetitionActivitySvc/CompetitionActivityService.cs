using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

namespace UniCEC.Business.Services.CompetitionActivitySvc
{
    public class CompetitionActivityService : ICompetitionActivityService
    {
        private ICompetitionActivityRepo _competitionActivityRepo;
        //
        private IActivitiesEntityRepo _activitiesEntityRepo;
        private IMemberTakesActivityRepo _memberTakesActivityRepo;
        private IClubRepo _clubRepo;
        private ICompetitionRepo _competitionRepo;
        private IMemberRepo _memberRepo;
        private IFileService _fileService;
        private IUserRepo _userRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private DecodeToken _decodeToken;

        public CompetitionActivityService(ICompetitionActivityRepo clubActivityRepo,
                                          IMemberTakesActivityRepo memberTakesActivityRepo,
                                          IClubRepo clubRepo,
                                          ICompetitionRepo competitionRepo,
                                          IMemberRepo memberRepo,
                                          IActivitiesEntityRepo activitiesEntityRepo,
                                          IUserRepo userRepo,
                                          IMemberInCompetitionRepo memberInCompetitionRepo,
                                          IFileService fileService)
        {
            _competitionActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubRepo = clubRepo;
            _competitionRepo = competitionRepo;
            _memberRepo = memberRepo;
            _activitiesEntityRepo = activitiesEntityRepo;
            _userRepo = userRepo;
            _fileService = fileService;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _decodeToken = new DecodeToken();
        }



        //Get Process + Top 3
        //public async Task<List<ViewProcessCompetitionActivity>> GetTop3TasksOfCompetition(int clubId, string token)
        //{
        //    try
        //    {
        //        if (clubId == 0) throw new ArgumentException("Club Id Null");
        //        List<ViewProcessCompetitionActivity> result = await _competitionActivityRepo.GetTop3CompetitionActivity(clubId);
        //        if (result == null) throw new NullReferenceException();
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw new NullReferenceException("Not Has Data !!!");
        //    }
        //}

        //public async Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions, string token)
        //{
        //    try
        //    {
        //        if (conditions.ClubId == 0) throw new ArgumentException("Club Id Null");

        //        //có Competition Id
        //        if (conditions.CompetitionId.HasValue)
        //        {
        //            bool check = await CheckConditions(token, conditions.CompetitionId.Value, conditions.ClubId);

        //            if (check)
        //            {

        //                //
        //                PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListActivitiesByConditions(conditions);

        //                //
        //                if (result == null) throw new NullReferenceException();

        //                List<ViewCompetitionActivity> list_vdca = result.Items.ToList();

        //                foreach (ViewCompetitionActivity viewDetailCompetitionActivity in list_vdca)
        //                {

        //                    //List Activities Entity
        //                    List<ViewActivitiesEntity> ListView_ActivitiesEntity = new List<ViewActivitiesEntity>();

        //                    List<ActivitiesEntity> ActivitiesEntities = await _activitiesEntityRepo.GetListActivitesEntityByCompetition(viewDetailCompetitionActivity.Id);

        //                    if (ActivitiesEntities != null)
        //                    {
        //                        foreach (ActivitiesEntity ActivitiesEntity in ActivitiesEntities)
        //                        {
        //                            //get IMG from Firebase                        
        //                            string imgUrl_ActivitiesEntity;
        //                            try
        //                            {
        //                                imgUrl_ActivitiesEntity = await _fileService.GetUrlFromFilenameAsync(ActivitiesEntity.ImageUrl);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                imgUrl_ActivitiesEntity = "";
        //                            }

        //                            ViewActivitiesEntity viewActivitiesEntity = new ViewActivitiesEntity()
        //                            {
        //                                Id = ActivitiesEntity.Id,
        //                                CompetitionActivityId = ActivitiesEntity.CompetitionActivityId,
        //                                ImageUrl = imgUrl_ActivitiesEntity,
        //                                Name = ActivitiesEntity.Name,
        //                            };
        //                            //
        //                            ListView_ActivitiesEntity.Add(viewActivitiesEntity);
        //                        }
        //                    }
        //                    viewDetailCompetitionActivity.ActivitiesEntities = ListView_ActivitiesEntity;
        //                }

        //                return result;


        //            }
        //            else
        //            {
        //                throw new NullReferenceException();
        //            }

        //        }
        //        //kh có Competition Id
        //        else
        //        {
        //            //thì thằng này phải là club leader 
        //            Member clubLeader = await _memberRepo.GetLeaderByClub(conditions.ClubId);
        //            if (clubLeader.UserId == _decodeToken.Decode(token, "Id"))
        //            {
        //                PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListProcessActivitiesByConditions(conditions);
        //                if (result == null) throw new NullReferenceException();
        //                return result;
        //            }
        //            else
        //            {
        //                throw new ArgumentException("You don't have permission to do this action");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        //Get ClubActivity-By-Id
        public async Task<ViewDetailCompetitionActivity> GetCompetitionActivityById(int id, int clubId, string token)
        {
            try
            {

                if (id == 0 || clubId == 0) throw new ArgumentException(" Id Null || Club Id Null");

                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(id);
                if (competitionActivity == null) throw new NullReferenceException();

                //Check Condititon
                // trong đây đã check được là nếu User cố tình lấy Id Task của Competition khác thì sẽ không được
                // khi check đến MemberInCompetition thì sẽ thấy được là User đó kh thuộc trong Competition
                await CheckMemberInCompetition(token, competitionActivity.CompetitionId, clubId, false);
                
                return await TransformViewDetailCompetitionActivity(competitionActivity);

            }
            catch (Exception)
            {
                throw;
            }
        }

        //Insert

        //Insert
        public async Task<ViewDetailCompetitionActivity> Insert(CompetitionActivityInsertModel model, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name)
                    || model.ClubId == 0
                    || model.CompetitionId == 0
                    || model.SeedsPoint < 0
                    || string.IsNullOrEmpty(model.Description)
                    || model.Ending == DateTime.Parse("1/1/0001 12:00:00 AM"))
                    throw new ArgumentNullException("Name Null || ClubId Null || Competition Id Null || SeedsPoint Null || Description Null || Beginning Null || Ending Null");

                //Check Condititon
                await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, false);

                //Check Status Competition
                Competition compe = await _competitionRepo.Get(model.CompetitionId);
                if (compe.Status == CompetitionStatus.Finish) throw new ArgumentException("Competition is End");

                //Check Ending date
                bool checkDate = CheckDate(model.Ending);
                if (checkDate == false) throw new ArgumentException("Date not suitable");

                //Add List Activities Entity
                bool insertActivitiesEntity;
                if (model.ListActivitiesEntities.Count > 0)
                {
                    foreach (AddActivitiesEntity modelItem in model.ListActivitiesEntities)
                    {
                        if (string.IsNullOrEmpty(modelItem.Base64StringImg)) throw new ArgumentNullException("Image is NULL");
                    }

                    insertActivitiesEntity = true;
                }
                else
                {
                    insertActivitiesEntity = false;
                }

                //
                CompetitionActivity competitionActivity = new CompetitionActivity();
                competitionActivity.CompetitionId = model.CompetitionId;
                competitionActivity.NumOfMember = 0;                                              //When Member Take Activity will +1
                competitionActivity.Description = model.Description;
                competitionActivity.Name = model.Name;
                competitionActivity.SeedsPoint = model.SeedsPoint;
                competitionActivity.CreateTime = new LocalTime().GetLocalTime().DateTime;         //LocalTime
                competitionActivity.Ending = model.Ending;
                competitionActivity.SeedsCode = await checkExistCode();                           //Check Code                       
                competitionActivity.Status = CompetitionActivityStatus.Open;                      //Check Status
                competitionActivity.Priority = model.Priority;
                competitionActivity.CreatorId = _decodeToken.Decode(token, "Id");

                int result = await _competitionActivityRepo.Insert(competitionActivity);
                if (result > 0)
                {
                    //------------ Insert Activities Entity
                    if (insertActivitiesEntity)
                    {
                        foreach (AddActivitiesEntity modelItem in model.ListActivitiesEntities)
                        {
                            //------------ Insert Activities-Entities-----------
                            string Url = await _fileService.UploadFile(modelItem.Base64StringImg);
                            ActivitiesEntity ActivitiesEntity = new ActivitiesEntity()
                            {
                                CompetitionActivityId = result,
                                Name = modelItem.Name,
                                ImageUrl = Url
                            };

                            int id = await _activitiesEntityRepo.Insert(ActivitiesEntity);
                        }
                    }
                    CompetitionActivity ca = await _competitionActivityRepo.Get(result);
                    ViewDetailCompetitionActivity viewClubActivity = await TransformViewDetailCompetitionActivity(ca);
                    return viewClubActivity;
                }
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

        //Update
        public async Task<bool> Update(CompetitionActivityUpdateModel model, string token)
        {
            try
            {
                if (model.ClubId == 0) throw new ArgumentException("Club Id Null");

                //Check competition Activity Existed
                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.Id);
                if (competitionActivity == null) throw new ArgumentException("Competition Activity not found in system");

                await CheckMemberInCompetition(token, competitionActivity.CompetitionId, model.ClubId, false);

                //Check Competition Status
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                if (competition.Status == CompetitionStatus.Finish) throw new ArgumentException("Competition is End");

                //Check Task Status
                if (competitionActivity.Status == CompetitionActivityStatus.Cancelling) throw new ArgumentException("Competition Activity is Cancel");

                //------------ Check date update
                bool checkDateUpdate = false;
                bool Ending = false;
                //Ending Update = Ending 
                if (model.Ending.HasValue && DateTime.Compare(model.Ending.Value, competitionActivity.Ending) != 0) // data mới
                {
                    Ending = true;
                }
                if (Ending)
                {
                    checkDateUpdate = CheckDate(model.Ending.Value);
                }
                else
                {
                    checkDateUpdate = true;
                }
                if (checkDateUpdate == false) { throw new ArgumentException("Date not suitable"); }

                competitionActivity.Name = (model.Name.Length > 0) ? model.Name : competitionActivity.Name;
                competitionActivity.Description = (model.Description.Length > 0) ? model.Description : competitionActivity.Description;
                competitionActivity.SeedsPoint = (model.SeedsPoint.HasValue) ? model.SeedsPoint.Value : competitionActivity.SeedsPoint;
                competitionActivity.Ending = (model.Ending.HasValue) ? model.Ending.Value : competitionActivity.Ending;
                competitionActivity.Priority = (model.Priority.HasValue) ? model.Priority.Value : competitionActivity.Priority;
                competitionActivity.Status = (model.Status.HasValue) ? model.Status.Value : competitionActivity.Status;
                await _competitionActivityRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Delete
        public async Task<bool> Delete(CompetitionActivityDeleteModel model, string token)
        {
            try
            {
                //
                if (model.CompetitionActivityId == 0 || model.ClubId == 0) throw new ArgumentException("Competition Activity Id Null || Club Id Null");

                //
                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.CompetitionActivityId);
                if (competitionActivity == null) throw new ArgumentException("Club Activity not found");

                //
                await CheckMemberInCompetition(token, competitionActivity.CompetitionId, model.ClubId, false);

                //
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                if (competition.Status == CompetitionStatus.Finish) throw new ArgumentException("Competition is End");

                competitionActivity.Status = CompetitionActivityStatus.Cancelling;
                await _competitionActivityRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //transform View Model
        public async Task<ViewDetailCompetitionActivity> TransformViewDetailCompetitionActivity(CompetitionActivity competitionActivity)
        {

            //List Activities Entity
            List<ViewActivitiesEntity> ListView_ActivitiesEntity = new List<ViewActivitiesEntity>();

            List<ActivitiesEntity> ActivitiesEntities = (List<ActivitiesEntity>)competitionActivity.ActivitiesEntities;

            if (ActivitiesEntities != null)
            {
                foreach (ActivitiesEntity ActivitiesEntity in ActivitiesEntities)
                {
                    //get IMG from Firebase                        
                    string imgUrl_ActivitiesEntity;
                    try
                    {
                        imgUrl_ActivitiesEntity = await _fileService.GetUrlFromFilenameAsync(ActivitiesEntity.ImageUrl);
                    }
                    catch (Exception)
                    {
                        imgUrl_ActivitiesEntity = "";
                    }

                    ViewActivitiesEntity viewActivitiesEntity = new ViewActivitiesEntity()
                    {
                        Id = ActivitiesEntity.Id,
                        CompetitionActivityId = ActivitiesEntity.CompetitionActivityId,
                        ImageUrl = imgUrl_ActivitiesEntity,
                        Name = ActivitiesEntity.Name,
                    };
                    //
                    ListView_ActivitiesEntity.Add(viewActivitiesEntity);
                }
            }

            //Who Create This Task
            User creator = await _userRepo.Get(competitionActivity.CreatorId);


            //List Member Takes Activity


            return new ViewDetailCompetitionActivity()
            {
                Id = competitionActivity.Id,
                CompetitionId = competitionActivity.CompetitionId,
                Name = competitionActivity.Name,
                Description = competitionActivity.Description,
                SeedsCode = competitionActivity.SeedsCode,
                SeedsPoint = competitionActivity.SeedsPoint,
                NumOfMember = competitionActivity.NumOfMember,
                Ending = competitionActivity.Ending,
                CreateTime = competitionActivity.CreateTime,
                Priority = competitionActivity.Priority,
                Status = competitionActivity.Status,
                CreatorId = creator.Id,
                CreatorName = creator.Fullname,
                CreatorEmail = creator.Email,
                ActivitiesEntities = ListView_ActivitiesEntity
            };
        }


        //generate Seed code length 8
        private string generateSeedCode()
        {
            string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[8];
            string code = "";
            var random = new Random();

            for (int i = 0; i < chars.Length; i++)
            {
                code += string.Concat(codePool[random.Next(codePool.Length)]);
            }
            return code;
        }

        //check exist code
        private async Task<string> checkExistCode()
        {
            //auto generate seedCode
            bool check = true;
            string seedCode = "";
            while (check)
            {
                string generateCode = generateSeedCode();
                check = await _competitionActivityRepo.CheckExistCode(generateCode);
                seedCode = generateCode;
            }
            return seedCode;
        }


        //Check date 
        // COC < Ending
        private bool CheckDate(DateTime Ending)
        {
            bool result = false;
            DateTime lt = new LocalTime().GetLocalTime().DateTime;
            int rs1 = DateTime.Compare(Ending, lt);

            if (rs1 > 0)
            {
                result = true;
            }
            return result;
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
