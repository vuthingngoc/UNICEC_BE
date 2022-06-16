using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;
using UniCEC.Data.ViewModels.Entities.Member;
using UniCEC.Data.ViewModels.Entities.Term;

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
        private ITermRepo _termRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;
        private IFileService _fileService;
        private JwtSecurityTokenHandler _tokenHandler;

        public CompetitionActivityService(ICompetitionActivityRepo clubActivityRepo,
                                          IMemberTakesActivityRepo memberTakesActivityRepo,
                                          IClubRepo clubRepo,
                                          ICompetitionRepo competitionRepo,
                                          ITermRepo termRepo,
                                          IMemberRepo memberRepo,
                                          ICompetitionManagerRepo competitionManagerRepo,
                                          IActivitiesEntityRepo activitiesEntityRepo,   
                                          IFileService fileService)
        {
            _competitionActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubRepo = clubRepo;
            _competitionRepo = competitionRepo;
            _termRepo = termRepo;
            _memberRepo = memberRepo;
            _competitionManagerRepo = competitionManagerRepo;
            _activitiesEntityRepo = activitiesEntityRepo;   
            _fileService = fileService;
        }



        //Get-List-Club-Activities-By-Conditions
        //lấy tất cả các task của 1 trường - 1 câu lạc bộ - seed point - Number of member
        public async Task<PagingResult<ViewCompetitionActivity>> GetListClubActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {
            //
            PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListClubActivitiesByConditions(conditions);
            //
            return result;
        }

        //Get Process + Top 4
        public async Task<List<ViewProcessCompetitionActivity>> GetTop4_Process(int clubId, string token)
        {
            //
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var UniIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int UniId = Int32.Parse(UniIdClaim.Value);

            List<ViewProcessCompetitionActivity> viewProcessClubActivities = new List<ViewProcessCompetitionActivity>();

            //check clubId in the system
            Club club = await _clubRepo.Get(clubId);
            if (club != null)
            {
                //check club belong to university id
                if (club.UniversityId == UniId)
                {
                    //top 4 activity by ClubId
                    List<ViewCompetitionActivity> ListViewClubActivity = await _competitionActivityRepo.GetClubActivitiesByCreateTime(UniId, clubId);

                    foreach (ViewCompetitionActivity viewClubActivity in ListViewClubActivity)
                    {
                        //Get Process
                        //get total num of member join
                        int NumberOfMemberJoin = await _memberTakesActivityRepo.GetNumOfMemInTask(viewClubActivity.Id);
                        //get number of member doing task
                        int NumMemberDoingTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Doing);
                        //get number of member submit on time task
                        int NumMemberDoneTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Finished);
                        //get number of member submit on late task
                        int NumMemberDoneLateTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.FinishedLate);
                        //get number of member late task
                        int NumMemberLateTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.LateTime);
                        //get number of member out task
                        //int NumMemberOutTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Approved);

                        ViewProcessCompetitionActivity vpca = new ViewProcessCompetitionActivity()
                        {
                            Ending = viewClubActivity.Ending,
                            CreateTime = viewClubActivity.CreateTime,
                            Description = viewClubActivity.Description,
                            Id = viewClubActivity.Id,
                            Name = viewClubActivity.Name,
                            NumOfMember = viewClubActivity.NumOfMember,
                            SeedsCode = viewClubActivity.SeedsCode,
                            SeedsPoint = viewClubActivity.SeedsPoint,
                            Status = viewClubActivity.Status,
                            NumOfMemberJoin = NumberOfMemberJoin,
                            NumMemberDoingTask = NumMemberDoingTask,
                            NumMemberDoneTask = NumMemberDoneTask,
                            NumMemberDoneLateTask = NumMemberDoneLateTask,
                            NumMemberLateTask = NumMemberLateTask,
                            //NumMemberOutTask = NumMemberOutTask
                        };
                        viewProcessClubActivities.Add(vpca);
                    }
                    return (viewProcessClubActivities.Count > 0) ? viewProcessClubActivities : throw new NullReferenceException();
                }//end check club in Uni
                else
                {
                    throw new ArgumentException("Club not in University");
                }
            }//end check club
            else
            {
                throw new ArgumentException("Club not found ");
            }
        }

        //Get-ClubActivity-By-Id
        public async Task<ViewCompetitionActivity> GetByClubActivityId(int id)
        {
            CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(id);
            //
            if (competitionActivity != null)
            {
                return await TransformViewCompetitionActivity(competitionActivity);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //Insert
        public async Task<ViewCompetitionActivity> Insert(CompetitionActivityInsertModel model, string token)
        {
            try
            {            
                if (string.IsNullOrEmpty(model.Name)
                    || model.ClubId == 0
                    || model.SeedsPoint < 0
                    || string.IsNullOrEmpty(model.Description)
                    || model.Ending == DateTime.Parse("1/1/0001 12:00:00 AM"))
                    throw new ArgumentNullException("Name Null || ClubId Null || SeedsPoint Null || Description Null || Beginning Null || Ending Null");

                bool check = await CheckConditions(token, model.CompetitionId, model.ClubId);
                if (check)
                {

                    //Add Activities Entity
                    bool insertActivitiesEntity;
                    if (!string.IsNullOrEmpty(model.ActivitiesEntity.Base64StringEntity))
                    {
                        insertActivitiesEntity = true;
                    }
                    else
                    {
                        insertActivitiesEntity = false;
                    }

                    //
                    CompetitionActivity competitionActivity = new CompetitionActivity();
                    competitionActivity.CompetitionId = model.CompetitionId;    
                    //When Member Take Activity will +1
                    competitionActivity.NumOfMember = 0;
                    competitionActivity.Description = model.Description;
                    competitionActivity.Name = model.Name;
                    competitionActivity.SeedsPoint = model.SeedsPoint;
                    //LocalTime
                    competitionActivity.CreateTime = new LocalTime().GetLocalTime().DateTime;
                    competitionActivity.Ending = model.Ending;
                    //Check Status
                    competitionActivity.Status = CompetitionActivityStatus.Happenning;
                    //Check Code
                    competitionActivity.SeedsCode = await checkExistCode();

                    int result = await _competitionActivityRepo.Insert(competitionActivity);
                    if (result > 0)
                    {
                        //------------ Insert Activities Entity
                        if (insertActivitiesEntity)
                        {
                            string Url = await _fileService.UploadFile(model.ActivitiesEntity.Base64StringEntity);
                            ActivitiesEntity activitesEntity = new ActivitiesEntity()
                            {
                                CompetitionActivityId = result,
                                Name = model.ActivitiesEntity.NameEntity,
                                ImageUrl = Url
                            };
                            await _activitiesEntityRepo.Insert(activitesEntity);
                        }


                        CompetitionActivity ca = await _competitionActivityRepo.Get(result);
                        ViewCompetitionActivity viewClubActivity = await TransformViewCompetitionActivity(ca);
                        return viewClubActivity;
                    }
                    else
                    {
                        return null;
                    }
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

                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);


                bool roleLeader = false;
                bool checkDateUpdate = false;

                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewBasicInfoMember infoClubMem = new ViewBasicInfoMember();// await _clubHistoryRepo.GetMemberInCLub(conditions);
                //------------ Check Mem in that club
                if (infoClubMem != null)
                {
                    //------------ Check Role Member Is Leader 
                    if (infoClubMem.ClubRoleName.Equals("Leader"))
                    {
                        roleLeader = true;
                    }
                    if (roleLeader)
                    {
                        //get club Activity
                        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.Id);
                        if (competitionActivity != null)
                        {
                            //------------ Check date update
                            //th1
                            if (model.Beginning.HasValue && !model.Ending.HasValue)
                            {
                                checkDateUpdate = CheckDate((DateTime)model.Beginning, competitionActivity.Ending);
                            }
                            //th2
                            if (model.Beginning.HasValue && model.Ending.HasValue)
                            {
                                checkDateUpdate = CheckDate((DateTime)model.Beginning, (DateTime)model.Ending);
                            }
                            if (checkDateUpdate)
                            {
                                //update name-des-seedpoint-beginning-ending
                                competitionActivity.Name = (model.Name.Length > 0) ? model.Name : competitionActivity.Name;
                                competitionActivity.Description = (model.Description.Length > 0) ? model.Description : competitionActivity.Description;
                                competitionActivity.SeedsPoint = (model.SeedsPoint != 0) ? model.SeedsPoint : competitionActivity.SeedsPoint;
                                competitionActivity.Ending = (DateTime)((model.Ending.HasValue) ? model.Ending : competitionActivity.Ending);

                                //Update DEADLINE day of member takes activity
                                await _memberTakesActivityRepo.UpdateDeadlineDate(competitionActivity.Id, competitionActivity.Ending);
                                await _competitionActivityRepo.Update();
                                return true;

                            }
                            else
                            {
                                throw new ArgumentException("Date not suitable");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Club Activity not found to update");
                        }

                    }
                    else
                    {
                        throw new UnauthorizedAccessException("You do not a role Leader to update this Club Activity");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("You aren't member in Club");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        //transform View Model
        public async Task<ViewCompetitionActivity> TransformViewCompetitionActivity(CompetitionActivity competitionActivity)
        {

            //List Activities Entity
            List<ViewActivitiesEntity> ListView_ActivitiesEntity = new List<ViewActivitiesEntity>();

            List<ActivitiesEntity> ActivitiesEntities = competitionActivity.ActivitiesEntities.ToList();

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
                    catch (Exception ex)
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

            return new ViewCompetitionActivity()
            {
                Ending = competitionActivity.Ending,
                CreateTime = competitionActivity.CreateTime,
                Description = competitionActivity.Description,
                Id = competitionActivity.Id,
                Name = competitionActivity.Name,
                NumOfMember = competitionActivity.NumOfMember,
                SeedsCode = competitionActivity.SeedsCode,
                SeedsPoint = competitionActivity.SeedsPoint,
                Status = competitionActivity.Status,
                CompetitionId = competitionActivity.CompetitionId,
                Priority = competitionActivity.Priority,
                ActivitiesEntities = ListView_ActivitiesEntity           
            };
        }

        //Delete-Club-Activity-By-Id
        public async Task<bool> Delete(CompetitionActivityDeleteModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);


                bool roleLeader = false;

                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };

                ViewBasicInfoMember infoClubMem = new ViewBasicInfoMember();//await _clubHistoryRepo.GetMemberInCLub(conditions);
                //------------ Check Mem in that club
                if (infoClubMem != null)
                {
                    //------------ Check Role Member Is Leader 
                    if (infoClubMem.ClubRoleName.Equals("Leader"))
                    {
                        roleLeader = true;
                    }

                    if (roleLeader)
                    {
                        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.ClubActivityId);
                        if (competitionActivity != null)
                        {
                            //
                            //clubActivity.Status = ClubActivityStatus.Canceling;
                            await _competitionActivityRepo.Update();
                            return true;
                        }
                        else
                        {
                            throw new ArgumentException("Club Activity not found to update");
                        }
                    }//end check role Leader
                    else
                    {
                        throw new UnauthorizedAccessException("You do not a role Leader to delete this Club Activity");
                    }
                }//end member
                else
                {
                    throw new UnauthorizedAccessException("You aren't member in Club");
                }
            }
            catch (Exception)
            {
                throw;
            }
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
        private bool CheckDate(DateTime CreateTimeOfCompetition, DateTime Ending)
        {

            bool result = false;

            int rs1 = DateTime.Compare(CreateTimeOfCompetition, Ending);

            if (rs1 < 0)
            {
                result = true;
            }

            return result;
        }


        private async Task<bool> CheckConditions(string Token, int CompetitionId, int ClubId)
        {
            //
            int UserId = DecodeToken(Token, "Id");

            //------------- CHECK Competition is have in system or not
            Competition competition = await _competitionRepo.Get(CompetitionId);
            if (competition != null)
            {
                //------------- CHECK Club in system
                Club club = await _clubRepo.Get(ClubId);
                if (club != null)
                {
                    ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(ClubId);
                    if (CurrentTermOfCLub != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = ClubId,
                            TermId = CurrentTermOfCLub.Id
                        };
                        ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
                        //------------- CHECK Mem in that club
                        if (infoClubMem != null)
                        {
                            //------------- CHECK is in CompetitionManger table                
                            CompetitionManager isAllow = await _competitionManagerRepo.GetMemberInCompetitionManager(CompetitionId, infoClubMem.Id, ClubId);
                            if (isAllow != null)
                            {                              
                                    return true;                            
                            }
                            else
                            {
                                throw new UnauthorizedAccessException("You do not in Competition Manager");
                            }
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("You are not member in Club");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Term of ClubId is End");
                    }
                }
                else
                {
                    throw new ArgumentException("Club is not found");
                }
            }
            else
            {
                throw new ArgumentException("Competition or Event not found ");
            }
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

    }
}
