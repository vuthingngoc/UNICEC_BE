using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;

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
       //private ITermRepo _termRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;
        private IFileService _fileService;
        private IUserRepo _userRepo;

        private DecodeToken _decodeToken;

        public CompetitionActivityService(ICompetitionActivityRepo clubActivityRepo,
                                          IMemberTakesActivityRepo memberTakesActivityRepo,
                                          IClubRepo clubRepo,
                                          ICompetitionRepo competitionRepo,
                                          //ITermRepo termRepo,
                                          IMemberRepo memberRepo,
                                          ICompetitionManagerRepo competitionManagerRepo,
                                          IActivitiesEntityRepo activitiesEntityRepo,
                                          IUserRepo userRepo,
                                          IFileService fileService)
        {
            _competitionActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubRepo = clubRepo;
            _competitionRepo = competitionRepo;
            //_termRepo = termRepo;
            _memberRepo = memberRepo;
            _competitionManagerRepo = competitionManagerRepo;
            _activitiesEntityRepo = activitiesEntityRepo;
            _userRepo = userRepo;
            _fileService = fileService;
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


        ////Get-ClubActivity-By-Id
        //public async Task<ViewDetailCompetitionActivity> GetCompetitionActivityById(int id, int clubId, string token)
        //{
        //    try
        //    {

        //        if (clubId == 0) throw new ArgumentException("Club Id Null");

        //        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(id);
        //        //
        //        if (competitionActivity == null) throw new NullReferenceException();

        //        Competition c = await _competitionRepo.Get(competitionActivity.CompetitionId);


        //        bool check = await CheckConditions(token, c.Id, clubId); // trong đây đã check được là nếu User cố tình lấy Id Task của Competition khác thì sẽ không được
        //                                                                // khi check đến competitionManager thì sẽ thấy được là User đó kh thuộc trong Competitio
        //        if (check)
        //        {
        //            return await TransformViewDetailCompetitionActivity(competitionActivity);
        //        }
        //        else
        //        {
        //            throw new NullReferenceException();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        ////Insert
        //public async Task<ViewDetailCompetitionActivity> Insert(CompetitionActivityInsertModel model, string token)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(model.Name)
        //            || model.ClubId == 0
        //            || model.SeedsPoint < 0
        //            || string.IsNullOrEmpty(model.Description)
        //            || model.Ending == DateTime.Parse("1/1/0001 12:00:00 AM"))
        //            throw new ArgumentNullException("Name Null || ClubId Null || SeedsPoint Null || Description Null || Beginning Null || Ending Null");

        //        bool check = await CheckConditions(token, model.CompetitionId, model.ClubId);
        //        if (check)
        //        {
        //            bool checkDate = CheckDate(model.Ending);
        //            if (checkDate)
        //            {
        //                //Add Activities Entity
        //                bool insertActivitiesEntity;
        //                if (!string.IsNullOrEmpty(model.ActivitiesEntity.Base64StringEntity))
        //                {
        //                    insertActivitiesEntity = true;
        //                }
        //                else
        //                {
        //                    insertActivitiesEntity = false;
        //                }

        //                //
        //                CompetitionActivity competitionActivity = new CompetitionActivity();
        //                competitionActivity.CompetitionId = model.CompetitionId;
        //                competitionActivity.NumOfMember = 0;                                                    //When Member Take Activity will +1
        //                competitionActivity.Description = model.Description;
        //                competitionActivity.Name = model.Name;
        //                competitionActivity.SeedsPoint = model.SeedsPoint;
        //                competitionActivity.CreateTime = new LocalTime().GetLocalTime().DateTime;               //LocalTime
        //                competitionActivity.Ending = model.Ending;
        //                competitionActivity.SeedsCode = await checkExistCode();                                 //Check Code
        //                competitionActivity.Process = CompetitionActivityProcessStatus.NotComplete;             //Will update when Member Submit task
        //                competitionActivity.Status = CompetitionActivityStatus.Happenning;                      //Check Status
        //                competitionActivity.Priority = model.Priority;
        //                competitionActivity.UserId = _decodeToken.Decode(token,"Id");


        //                int result = await _competitionActivityRepo.Insert(competitionActivity);
        //                if (result > 0)
        //                {
        //                    //------------ Insert Activities Entity
        //                    if (insertActivitiesEntity)
        //                    {
        //                        string Url = await _fileService.UploadFile(model.ActivitiesEntity.Base64StringEntity);
        //                        ActivitiesEntity activitesEntity = new ActivitiesEntity()
        //                        {
        //                            CompetitionActivityId = result,
        //                            Name = model.ActivitiesEntity.NameEntity,
        //                            ImageUrl = Url
        //                        };
        //                        await _activitiesEntityRepo.Insert(activitesEntity);
        //                    }


        //                    CompetitionActivity ca = await _competitionActivityRepo.Get(result);
        //                    ViewDetailCompetitionActivity viewClubActivity = await TransformViewDetailCompetitionActivity(ca);
        //                    return viewClubActivity;
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Date not suitable");
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        ////Update
        //public async Task<bool> Update(CompetitionActivityUpdateModel model, string token)
        //{
        //    try
        //    {

        //        if (model.ClubId == 0) throw new ArgumentException("Club Id Null");

        //        //get competition Activity
        //        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.Id);
        //        if (competitionActivity == null) throw new ArgumentException("Competition Activity not found in system");

        //        //Canceling
        //        if (competitionActivity.Status == CompetitionActivityStatus.Canceling) throw new ArgumentException("Competition Activity not found to update");

        //        Competition c = await _competitionRepo.Get(competitionActivity.CompetitionId);

        //        bool check = await CheckConditions(token, c.Id, model.ClubId);

        //        if (check)
        //        {
        //            //------------ Check date update
        //            bool checkDateUpdate = false;
        //            bool Ending = false;
        //            //Ending Update = Ending 
        //            if (DateTime.Compare(model.Ending.Value, competitionActivity.Ending) != 0) // data mới
        //            {
        //                Ending = true;
        //            }

        //            if (Ending)
        //            {
        //                checkDateUpdate = CheckDate(model.Ending.Value);
        //            }
        //            else
        //            {
        //                checkDateUpdate = true;
        //            }
        //            if (checkDateUpdate)
        //            {

        //                competitionActivity.Name = (model.Name.Length > 0) ? model.Name : competitionActivity.Name;
        //                competitionActivity.Description = (model.Description.Length > 0) ? model.Description : competitionActivity.Description;
        //                competitionActivity.SeedsPoint = (model.SeedsPoint != 0) ? model.SeedsPoint : competitionActivity.SeedsPoint;
        //                competitionActivity.Ending = (DateTime)((model.Ending.HasValue) ? model.Ending : competitionActivity.Ending);
        //                competitionActivity.Priority = (PriorityStatus)((model.Priority.HasValue) ? model.Priority : competitionActivity.Priority);

        //                if (Ending)
        //                {
        //                    //Update DEADLINE day of member takes activity
        //                    await _memberTakesActivityRepo.UpdateDeadlineDate(competitionActivity.Id, competitionActivity.Ending);
        //                }

        //                await _competitionActivityRepo.Update();
        //                return true;

        //            }
        //            else
        //            {
        //                throw new ArgumentException("Date not suitable");
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////Delete-Club-Activity-By-Id
        //public async Task<bool> Delete(CompetitionActivityDeleteModel model, string token)
        //{
        //    try
        //    {

        //        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.CompetitionActivityId);
        //        //
        //        if (competitionActivity == null) throw new ArgumentException("Club Activity not found to update");

        //        Competition c = await _competitionRepo.Get(competitionActivity.CompetitionId);

        //        bool check = await CheckConditions(token, c.Id, model.ClubId);
        //        if (check)
        //        {
        //            competitionActivity.Status = CompetitionActivityStatus.Canceling;
        //            await _competitionActivityRepo.Update();

        //            //Update MemberTakeActivityStatus là Canceling

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        ////transform View Model
        //public async Task<ViewDetailCompetitionActivity> TransformViewDetailCompetitionActivity(CompetitionActivity competitionActivity)
        //{

        //    //List Activities Entity
        //    List<ViewActivitiesEntity> ListView_ActivitiesEntity = new List<ViewActivitiesEntity>();

        //    List<ActivitiesEntity> ActivitiesEntities = competitionActivity.ActivitiesEntities.ToList();

        //    if (ActivitiesEntities != null)
        //    {
        //        foreach (ActivitiesEntity ActivitiesEntity in ActivitiesEntities)
        //        {
        //            //get IMG from Firebase                        
        //            string imgUrl_ActivitiesEntity;
        //            try
        //            {
        //                imgUrl_ActivitiesEntity = await _fileService.GetUrlFromFilenameAsync(ActivitiesEntity.ImageUrl);
        //            }
        //            catch (Exception ex)
        //            {
        //                imgUrl_ActivitiesEntity = "";
        //            }

        //            ViewActivitiesEntity viewActivitiesEntity = new ViewActivitiesEntity()
        //            {
        //                Id = ActivitiesEntity.Id,
        //                CompetitionActivityId = ActivitiesEntity.CompetitionActivityId,
        //                ImageUrl = imgUrl_ActivitiesEntity,
        //                Name = ActivitiesEntity.Name,
        //            };
        //            //
        //            ListView_ActivitiesEntity.Add(viewActivitiesEntity);
        //        }
        //    }

        //    //Who Create This Task
        //    User creator = await _userRepo.Get(competitionActivity.UserId);


        //    //List Member Takes Activity


        //    return new ViewDetailCompetitionActivity()
        //    {
        //        Id = competitionActivity.Id,
        //        CompetitionId = competitionActivity.CompetitionId,
        //        Name = competitionActivity.Name,
        //        Description = competitionActivity.Description,
        //        SeedsCode = competitionActivity.SeedsCode,
        //        SeedsPoint = competitionActivity.SeedsPoint,
        //        NumOfMember = competitionActivity.NumOfMember,
        //        Ending = competitionActivity.Ending,
        //        CreateTime = competitionActivity.CreateTime,
        //        Priority = competitionActivity.Priority,
        //        ProcessStatus = competitionActivity.Process,
        //        Status = competitionActivity.Status,
        //        CreatorId = creator.Id,
        //        CreatorName = creator.Fullname,
        //        CreatorEmail = creator.Email,
        //        ActivitiesEntities = ListView_ActivitiesEntity
        //    };
        //}




        ////generate Seed code length 8
        //private string generateSeedCode()
        //{
        //    string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //    char[] chars = new char[8];
        //    string code = "";
        //    var random = new Random();

        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        code += string.Concat(codePool[random.Next(codePool.Length)]);
        //    }
        //    return code;
        //}

        ////check exist code
        //private async Task<string> checkExistCode()
        //{
        //    //auto generate seedCode
        //    bool check = true;
        //    string seedCode = "";
        //    while (check)
        //    {
        //        string generateCode = generateSeedCode();
        //        check = await _competitionActivityRepo.CheckExistCode(generateCode);
        //        seedCode = generateCode;
        //    }
        //    return seedCode;
        //}


        ////Check date 
        //// COC < Ending
        //private bool CheckDate(DateTime Ending)
        //{

        //    bool result = false;
        //    DateTime lt = new LocalTime().GetLocalTime().DateTime;
        //    int rs1 = DateTime.Compare(Ending, lt);

        //    if (rs1 > 0)
        //    {
        //        result = true;
        //    }

        //    return result;
        //}

        ////return UserId who Create this task
        //private async Task<bool> CheckConditions(string Token, int CompetitionId, int ClubId)
        //{
        //    //
        //    int UserId = _decodeToken.Decode(Token, "Id");

        //    //------------- CHECK Competition is have in system or not
        //    Competition competition = await _competitionRepo.Get(CompetitionId);
        //    if (competition != null)
        //    {
        //        //------------- CHECK Club in system
        //        Club club = await _clubRepo.Get(ClubId);
        //        if (club != null)
        //        {
        //            ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(ClubId);
        //            if (CurrentTermOfCLub != null)
        //            {
        //                GetMemberInClubModel conditions = new GetMemberInClubModel()
        //                {
        //                    UserId = UserId,
        //                    ClubId = ClubId,
        //                    TermId = CurrentTermOfCLub.Id
        //                };
        //                ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
        //                //------------- CHECK Mem in that club
        //                if (infoClubMem != null)
        //                {
        //                    //------------- CHECK is in CompetitionManger table                
        //                    CompetitionManager isAllow = await _competitionManagerRepo.GetMemberInCompetitionManager(CompetitionId, infoClubMem.UserId, ClubId);
        //                    if (isAllow != null)
        //                    {
        //                        return true;
        //                    }
        //                    else
        //                    {
        //                        throw new UnauthorizedAccessException("You do not in Competition Manager");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new UnauthorizedAccessException("You are not member in Club");
        //                }
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Term of ClubId is End");
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Club is not found");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Competition or Event not found ");
        //    }
        //}

        

    }
}
