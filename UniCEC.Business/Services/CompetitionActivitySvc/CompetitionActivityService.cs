using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Services.NotificationSvc;
using UniCEC.Business.Services.SeedsWalletSvc;
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
using UniCEC.Data.Repository.ImplRepo.NotificationRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

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
        private ISeedsWalletService _seedsWalletService;
        private INotificationService _notificationService;
        private INotificationRepo _notificationRepo;
        private DecodeToken _decodeToken;

        public CompetitionActivityService(ICompetitionActivityRepo clubActivityRepo,
                                          IMemberTakesActivityRepo memberTakesActivityRepo,
                                          IClubRepo clubRepo,
                                          ICompetitionRepo competitionRepo,
                                          IMemberRepo memberRepo,
                                          IActivitiesEntityRepo activitiesEntityRepo,
                                          IUserRepo userRepo,
                                          IMemberInCompetitionRepo memberInCompetitionRepo,
                                          ISeedsWalletService seedsWalletService,
                                          IFileService fileService,
                                          INotificationService notificationService,
                                          INotificationRepo notificationRepo)
        {
            _competitionActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubRepo = clubRepo;
            _competitionRepo = competitionRepo;
            _memberRepo = memberRepo;
            _activitiesEntityRepo = activitiesEntityRepo;
            _userRepo = userRepo;
            _fileService = fileService;
            _seedsWalletService = seedsWalletService;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _notificationService = notificationService;
            _notificationRepo = notificationRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<List<ViewProcessCompetitionActivity>> GetTopTasksOfCompetition(int clubId, int topCompetition, int topCompetitionActivity, string token)
        {
            try
            {
                if (clubId == 0 || topCompetition == 0 || topCompetitionActivity == 0) throw new ArgumentException("Club Id Null || Top Competition Null || Top Competition Activity Null");
                List<ViewProcessCompetitionActivity> result = await _competitionActivityRepo.GetTopCompetitionActivity(clubId, topCompetition, topCompetitionActivity);
                if (result == null) throw new NullReferenceException();
                return result;
            }
            catch (Exception)
            {
                throw new NullReferenceException("Not Has Data !!!");
            }
        }

        //Get By Condition
        public async Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions, string token)
        {
            try
            {
                //if (conditions.ClubId == 0) throw new ArgumentException("Club Id Null");

                ////có Competition Id
                //if (conditions.CompetitionId.HasValue)
                //{
                await CheckMemberInCompetition(token, conditions.CompetitionId.Value, conditions.ClubId, true);
                //
                PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListActivitiesByConditions(conditions);
                //
                if (result == null) throw new NullReferenceException();
                return result;

                //List<ViewCompetitionActivity> list_vdca = result.Items.ToList();


                ////Phần lấy hình ảnh
                //foreach (ViewCompetitionActivity viewDetailCompetitionActivity in list_vdca)
                //{
                //    //List Activities Entity
                //    List<ViewActivitiesEntity> ListView_ActivitiesEntity = new List<ViewActivitiesEntity>();
                //    List<ActivitiesEntity> ActivitiesEntities = await _activitiesEntityRepo.GetListActivitesEntityByCompetition(viewDetailCompetitionActivity.Id);
                //    if (ActivitiesEntities != null)
                //    {
                //        foreach (ActivitiesEntity ActivitiesEntity in ActivitiesEntities)
                //        {
                //            //get IMG from Firebase                        
                //            string imgUrl_ActivitiesEntity;
                //            try
                //            {
                //                if (ActivitiesEntity.ImageUrl.Contains("https"))
                //                {
                //                    imgUrl_ActivitiesEntity = ActivitiesEntity.ImageUrl;
                //                }
                //                else
                //                {
                //                    imgUrl_ActivitiesEntity = await _fileService.GetUrlFromFilenameAsync(ActivitiesEntity.ImageUrl);
                //                }
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
                //    else
                //    {
                //        viewDetailCompetitionActivity.ActivitiesEntities = null;
                //    }
                //}               
                //}
                ////kh có Competition Id
                //else
                //{
                //    //thì thằng này phải là club leader 
                //    Member clubLeader = await _memberRepo.GetLeaderByClub(conditions.ClubId);
                //    if (clubLeader.UserId == _decodeToken.Decode(token, "Id"))
                //    {
                //        PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListActivitiesByConditions2(conditions);
                //        if (result == null) throw new NullReferenceException();
                //        return result;
                //    }
                //    else
                //    {
                //        throw new ArgumentException("You don't have permission to do this action");
                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<PagingResult<ViewCompetitionActivity>> GetListCompetitionActivitiesIsAssigned(PagingRequest request, int competitionId, PriorityStatus? priorityStatus, List<CompetitionActivityStatus> statuses, string name, string token)
        {
            try
            {
                int userId = _decodeToken.Decode(token, "Id");

                PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListCompetitionActivitiesIsAssigned(request, competitionId, priorityStatus, statuses, name, userId);
                //
                if (result == null) throw new NullReferenceException();
                return result;
                //List<ViewCompetitionActivity> list_vdca = result.Items.ToList();

                //foreach (ViewCompetitionActivity viewDetailCompetitionActivity in list_vdca)
                //{
                //    //List Activities Entity
                //    List<ViewActivitiesEntity> ListView_ActivitiesEntity = new List<ViewActivitiesEntity>();

                //    List<ActivitiesEntity> ActivitiesEntities = await _activitiesEntityRepo.GetListActivitesEntityByCompetition(viewDetailCompetitionActivity.Id);

                //    if (ActivitiesEntities != null)
                //    {
                //        foreach (ActivitiesEntity ActivitiesEntity in ActivitiesEntities)
                //        {
                //            //get IMG from Firebase                        
                //            string imgUrl_ActivitiesEntity;
                //            try
                //            {
                //                if (ActivitiesEntity.ImageUrl.Contains("https"))
                //                {
                //                    imgUrl_ActivitiesEntity = ActivitiesEntity.ImageUrl;
                //                }

                //                else
                //                {
                //                    imgUrl_ActivitiesEntity = await _fileService.GetUrlFromFilenameAsync(ActivitiesEntity.ImageUrl);
                //                }
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
                //    else
                //    {
                //        viewDetailCompetitionActivity.ActivitiesEntities = null;
                //    }
                //}                               
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get ClubActivity-By-Id
        public async Task<ViewDetailCompetitionActivity> GetCompetitionActivityById(int id, int clubId, string token)
        {
            try
            {

                if (id == 0 || clubId == 0) throw new ArgumentException(" Id Null || Club Id Null");

                //------------- CHECK Club in system
                bool isExisted = await _clubRepo.CheckExistedClub(clubId);
                if (!isExisted) throw new ArgumentException("Club in not found");

                //------------- CHECK Is Member in Club
                int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), clubId);
                if (memberId.Equals(0)) throw new UnauthorizedAccessException("You aren't member in Club");

                //Check Condititon
                // trong đây đã check được là nếu User cố tình lấy Id Task của Competition khác thì sẽ không được
                // khi check đến MemberInCompetition thì sẽ thấy được là User đó kh thuộc trong Competition
                //await CheckMemberInCompetition(token, competitionActivity.CompetitionId, clubId, false);

                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(id);
                if (competitionActivity == null) throw new NullReferenceException();

                return await TransformViewDetailCompetitionActivity(competitionActivity);

            }
            catch (Exception)
            {
                throw;
            }
        }

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
                bool isCancleCompetition = await _competitionRepo.CheckExistedCompetitionByStatus(model.CompetitionId, CompetitionStatus.Cancel);
                if (isCancleCompetition) throw new ArgumentException("Cuộc thi đã bị hủy");

                //Check Ending date
                bool isValidDate = CheckDate(model.Ending);
                if (isValidDate == false) throw new ArgumentException("Date not suitable");

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
                //competitionActivity.SeedsCode = await checkExistCode();                           //Check Code                       
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

                //Check Task Status
                if (competitionActivity.Status == CompetitionActivityStatus.Cancelling && competitionActivity.Status == CompetitionActivityStatus.Completed) throw new ArgumentException("Competition Activity is Cancel Or Compeleted");

                //Check Competition Status
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                if (competition.Status == CompetitionStatus.Cancel) throw new ArgumentException("Cuộc thi đã bị hủy");

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

                competitionActivity.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : competitionActivity.Name;
                competitionActivity.Description = (!string.IsNullOrEmpty(model.Description)) ? model.Description : competitionActivity.Description;
                competitionActivity.SeedsPoint = (model.SeedsPoint.HasValue) ? model.SeedsPoint.Value : competitionActivity.SeedsPoint;
                competitionActivity.Ending = (model.Ending.HasValue) ? model.Ending.Value : competitionActivity.Ending;
                competitionActivity.Priority = (model.Priority.HasValue) ? model.Priority.Value : competitionActivity.Priority;
                //if (model.Status.HasValue)
                //{
                //    if (model.Status.Value == CompetitionActivityStatus.Completed || model.Status.Value == CompetitionActivityStatus.Cancelling || model.Status.Value == CompetitionActivityStatus.Open)
                //    {
                //        competitionActivity.Status = (model.Status.HasValue) ? model.Status.Value : competitionActivity.Status;
                //        //if Compeleted thì sẽ add point cho tất cả người tham gia trong task
                //        if ((model.Status.Value == CompetitionActivityStatus.Completed))
                //        {
                //            List<MemberTakesActivity> memberTakesActivities = await _memberTakesActivityRepo.ListMemberTakesActivity(competitionActivity.Id);
                //            foreach (MemberTakesActivity memberTakesActivity in memberTakesActivities)
                //            {
                //                await _seedsWalletService.UpdateAmount(memberTakesActivity.Member.UserId, competitionActivity.SeedsPoint);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        throw new ArgumentException("Status Cancelling and Completed and Open can Update");
                //    }
                //}
                // update hình ảnh
                if (model.ListActivitiesEntities != null)
                {
                    //check
                    foreach (AddActivitiesEntity modelItem in model.ListActivitiesEntities)
                    {
                        if (string.IsNullOrEmpty(modelItem.Base64StringImg)) throw new ArgumentNullException("Image is NULL");
                    }

                    //xóa hết tất cả entity trong competition Activity
                    await _activitiesEntityRepo.DeleteActivitiesEntity(model.Id);

                    //------------ Insert Competition-Entities-----------
                    foreach (AddActivitiesEntity modelItem in model.ListActivitiesEntities)
                    {
                        ActivitiesEntity ae = new ActivitiesEntity();
                        //1.Check TH đưa link cũ
                        if (modelItem.Base64StringImg.Contains("https"))
                        {

                            ae.CompetitionActivityId = model.Id;
                            ae.Name = modelItem.Name;
                            ae.ImageUrl = modelItem.Base64StringImg;

                        }
                        else
                        {
                            //2.Check Th tạo mới
                            string Url = await _fileService.UploadFile(modelItem.Base64StringImg);
                            ae.CompetitionActivityId = model.Id;
                            ae.Name = modelItem.Name;
                            ae.ImageUrl = Url;
                        }
                        await _activitiesEntityRepo.Insert(ae);
                    }
                }
                await _competitionActivityRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateStatus(CompetitionActivityUpdateStatusModel model, string token)
        {
            try
            {
                if (model.ClubId == 0) throw new ArgumentException("Club Id Null");

                //Check competition Activity Existed
                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.Id);
                if (competitionActivity == null) throw new ArgumentException("Competition Activity not found in system");

                await CheckMemberInCompetition(token, competitionActivity.CompetitionId, model.ClubId, false);

                //Check Task Status
                if (competitionActivity.Status == CompetitionActivityStatus.Cancelling
                    && competitionActivity.Status == CompetitionActivityStatus.Completed)
                    throw new ArgumentException("Competition Activity has already be Cancel Or Compeleted");

                //Check Competition Status
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                if (competition.Status == CompetitionStatus.Cancel) throw new ArgumentException("Cuộc thi đã bị hủy");

                if (model.Status.HasValue)
                {
                    if (model.Status.Value == CompetitionActivityStatus.Completed || model.Status.Value == CompetitionActivityStatus.Cancelling || model.Status.Value == CompetitionActivityStatus.Open)
                    {
                        //competitionActivity.Status = (model.Status.HasValue) ? model.Status.Value : competitionActivity.Status;
                        //if Compeleted thì sẽ add point cho tất cả người tham gia trong task
                        if ((model.Status.Value == CompetitionActivityStatus.Completed || model.Status.Value == CompetitionActivityStatus.Open)
                                && (competitionActivity.Status == CompetitionActivityStatus.Finished))
                        {
                            List<MemberTakesActivity> memberTakesActivities = await _memberTakesActivityRepo.ListMemberTakesActivity(competitionActivity.Id);
                            if (memberTakesActivities.Count <= 0 && model.Status.Value == CompetitionActivityStatus.Completed)
                                throw new ArgumentException("Hoạt động không có thành viên tham gia, không thể Duyệt");
                            if (model.Status.Value == CompetitionActivityStatus.Completed)
                            {
                                foreach (MemberTakesActivity memberTakesActivity in memberTakesActivities)
                                {
                                    await _seedsWalletService.UpdateAmount(memberTakesActivity.Member.UserId, competitionActivity.SeedsPoint);
                                }
                            }

                            // send notification
                            string fullname = _decodeToken.DecodeText(token, "Fullname");
                            List<int> memberIds = memberTakesActivities.Select(member => member.MemberId).ToList();
                            List<string> deviceTokens = await _userRepo.GetDeviceTokenByMembers(memberIds);
                            if (deviceTokens != null)
                            {
                                string message = (model.Status.Equals(CompetitionActivityStatus.Completed))
                                                    ? $"{fullname} mới vừa chấp thuận công việc {competitionActivity.Name} của bạn"
                                                    : $"{fullname} mới vừa từ chối công việc {competitionActivity.Name} của bạn";
                                Notification notification = new Notification()
                                {
                                    Title = "Thông báo",
                                    Body = message,
                                    RedirectUrl = "/viewCompetitionMemberTask",
                                    CreateTime = new LocalTime().GetLocalTime().DateTime
                                };

                                await _notificationService.SendNotification(notification, deviceTokens);
                            }

                        }
                        else
                        {
                            throw new ArgumentException("Hoạt động phải đang ở Status Finished");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Status Cancelling and Completed and Open can Update");
                    }
                    competitionActivity.Status = model.Status.Value;
                    await _competitionActivityRepo.Update();
                    return true;
                }
                else
                {
                    throw new ArgumentException("Status is Null");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Delete
        public async Task<bool> Delete(int CompetitionActivityId, int ClubId, string token)
        {
            try
            {
                //
                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(CompetitionActivityId);
                if (competitionActivity == null) throw new ArgumentException("Club Activity not found");

                if (competitionActivity.Status == CompetitionActivityStatus.Completed) throw new ArgumentException("Hoạt động đã Hoàn Thành không thể hủy");

                //
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                if (competition.Status == CompetitionStatus.Cancel) throw new ArgumentException("Cuộc Thi Sự Kiện đã bị hủy");

                //
                await CheckMemberInCompetition(token, competitionActivity.CompetitionId, ClubId, false);


                competitionActivity.Status = CompetitionActivityStatus.Cancelling;
                await _competitionActivityRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



        //-------------------------------------------MEMBER TAKE ACTIVITY 
        public async Task<ViewDetailMemberTakesActivity> AssignTaskForMember(MemberTakesActivityInsertModel model, string token)
        {
            try
            {
                if (model.MemberId == 0
                    || model.CompetitionActivityId == 0
                    || model.ClubId == 0)
                    throw new ArgumentNullException("MemberId Null || CompetitionActivityId Null || ClubId Null");

                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.CompetitionActivityId);

                //Check Competition Activity
                if (competitionActivity == null) throw new ArgumentException("Competition Activity not found");

                //Check là người add có quyền trong ban tổ chức hay không?
                await CheckMemberInCompetition(token, competitionActivity.CompetitionId, model.ClubId, false);

                //Competition Activity Status Canceling or Completed
                if (competitionActivity.Status == CompetitionActivityStatus.Cancelling || competitionActivity.Status == CompetitionActivityStatus.Completed)
                    throw new ArgumentException("Competition Activity is Cancelling or Completed");

                //Competition Status
                Competition competition = await _competitionRepo.Get(competitionActivity.CompetitionId);
                if (competition.Status == CompetitionStatus.Cancel) throw new ArgumentException("Cuộc thi đã bị hủy");

                //Check Member Id có status true
                Member member = await _memberRepo.Get(model.MemberId);
                if (member == null || member.Status == MemberStatus.Inactive) throw new ArgumentException("Member not found");

                //Check member is belong to club
                //chỉ có người trong club mình add người trong club mình
                if (member.ClubId != model.ClubId) throw new ArgumentException("Member are not belong to this club");

                //Check booker can not assign task for yourself 
                int memId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                if (memId == model.MemberId) throw new ArgumentException("Booker can not assign task for yourself");

                //Check add the same task for this member 
                bool mtaCheck = await _memberTakesActivityRepo.CheckMemberTakesTask(competitionActivity.Id, model.MemberId);
                if (mtaCheck) throw new ArgumentException("This member is already take this task");

                //-----------INSERT
                MemberTakesActivity mtaInsert = new MemberTakesActivity();
                mtaInsert.CompetitionActivityId = model.CompetitionActivityId;
                mtaInsert.MemberId = model.MemberId;
                mtaInsert.BookerId = _decodeToken.Decode(token, "Id");

                int result = await _memberTakesActivityRepo.Insert(mtaInsert);

                if (result == 0) throw new ArgumentException("Insert Failed !");

                //+ 1 number Of Member
                competitionActivity.NumOfMember = competitionActivity.NumOfMember + 1;
                await _competitionActivityRepo.Update();

                // send notification
                string fullname = _decodeToken.DecodeText(token, "Fullname");
                string deviceToken = await _userRepo.GetDeviceTokenByUser(member.UserId);
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    Notification notification = new Notification()
                    {
                        Title = "Thông báo",
                        Body = $"{fullname} vừa phân công cho bạn 1 công việc mới!",
                        RedirectUrl = "/viewCompetitionMemberTask",
                        UserId = member.UserId,
                    };

                    await _notificationService.SendNotification(notification, deviceToken);
                }

                return await TransferViewDetailMTA(mtaInsert);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> MemberUpdateStatusTask(MemberUpdateStatusTaskModel model, string token)
        {
            try
            {
                if (model.CompetitionActivityId == 0 || model.ClubId == 0)
                    throw new ArgumentNullException("CompetitionActivityId Null || ClubId Null");

                CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.CompetitionActivityId);

                //Check Competition Activity
                if (competitionActivity == null) throw new ArgumentException("Competition Activity not found");

                //Competition Activity Status Canceling or Completed
                if (competitionActivity.Status == CompetitionActivityStatus.Cancelling || competitionActivity.Status == CompetitionActivityStatus.Completed)
                    throw new ArgumentException("Competition Activity is Cancelling or Completed");

                //Competition Status
                bool isCancelCompetition = await _competitionRepo.CheckExistedCompetitionByStatus(competitionActivity.CompetitionId, CompetitionStatus.Cancel);
                if (isCancelCompetition) throw new ArgumentException("The competition is cancelled");// Cuộc thi đã bị hủy

                //Check User is Member of club
                int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                Member member = await _memberRepo.Get(memberId);
                if (member == null || member.Status == MemberStatus.Inactive) throw new ArgumentException("Member not found");

                //Check Member belong to this task
                bool mtaCheck = await _memberTakesActivityRepo.CheckMemberTakesTask(competitionActivity.Id, memberId);
                if (mtaCheck == false) throw new ArgumentException("This member is not in this task");

                //
                if (model.Status == CompetitionActivityStatus.Cancelling || model.Status == CompetitionActivityStatus.Completed)
                    throw new ArgumentException("You don't have permission to update this Status");

                competitionActivity.Status = model.Status;
                await _competitionActivityRepo.Update();

                // send notification
                string fullname = _decodeToken.DecodeText(token, "Fullname");
                List<MemberInCompetition> managers = await _memberInCompetitionRepo.GetAllManagerCompOrEve(competitionActivity.CompetitionId);
                List<MemberTakesActivity> members = await _memberTakesActivityRepo.ListMemberTakesActivity(model.CompetitionActivityId);
                List<int> listMemberIds = new List<int>();

                if (managers != null)
                {
                    List<int> managerIds = managers.Select(manager => manager.MemberId).ToList();
                    if (managerIds.Count > 0) listMemberIds.AddRange(managerIds);
                }

                if (members != null)
                {
                    // no need to push noti to the member who update status of task
                    List<int> memberIds = members.Where(member => member.MemberId != memberId)
                                                    .Select(member => member.MemberId).ToList();
                    if (memberIds.Count > 0) listMemberIds.AddRange(memberIds);
                }

                List<string> deviceTokens = await _userRepo.GetDeviceTokenByMembers(listMemberIds);
                deviceTokens = deviceTokens.Where(token => !string.IsNullOrEmpty(token)).ToList();
                if(deviceTokens.Count > 0)
                {
                    Notification notification = new Notification()
                    {
                        Title = "Thông báo",
                        Body = $"{fullname} vừa cập nhật một trạng thái công việc!",
                        RedirectUrl = "/viewCompetitionMemberTask",
                        CreateTime = new LocalTime().GetLocalTime().DateTime
                    };
                    await _notificationService.SendNotification(notification, deviceTokens);
                }

                // save notification
                List<Notification> notifications = new List<Notification>();
                listMemberIds.Add(memberId); // member do this task
                List<int> userIds = await _memberRepo.GetUserIdsByMembers(listMemberIds);
                foreach (int userId in userIds)
                {
                    Notification noti = new Notification()
                    {
                        Title = "Thông báo",
                        Body = $"{fullname} vừa cập nhật một trạng thái công việc!",
                        RedirectUrl = "/viewCompetitionMemberTask",
                        CreateTime = new LocalTime().GetLocalTime().DateTime,
                        UserId = userId
                    };
                    notifications.Add(noti);
                }

                await _notificationRepo.InsertNotifications(notifications);
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveMemberTakeTask(int MemberTakesActivityId, int ClubId, string token)
        {
            try
            {
                //Check task exsited
                MemberTakesActivity mta = await _memberTakesActivityRepo.Get(MemberTakesActivityId);
                if (mta == null) throw new ArgumentException("This member task not found !");

                //Competition Activity Status Canceling or Completed
                CompetitionActivity ca = await _competitionActivityRepo.Get(mta.CompetitionActivityId);
                if (ca.Status == CompetitionActivityStatus.Cancelling || ca.Status == CompetitionActivityStatus.Completed)
                    throw new ArgumentException("Competition Activity is Cancelling or Completed");

                //Check Competition Status                
                bool isCancelCompetition = await _competitionRepo.CheckExistedCompetitionByStatus(ca.CompetitionId, CompetitionStatus.Cancel);
                if (isCancelCompetition) throw new ArgumentException("The competition is cancelled");// Cuộc thi đã bị hủy

                //Check Condition
                await CheckMemberInCompetition(token, ca.CompetitionId, ClubId, false);

                // -1 Number Of Participant
                ca.NumOfMember = ca.NumOfMember - 1;
                await _competitionActivityRepo.Update();

                return await _memberTakesActivityRepo.RemoveMemberTakeTask(MemberTakesActivityId);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GetUrlImageActivityEntity(string imageUrl, int activityEntityId)
        {
            string fullPathImage = await _fileService.GetUrlFromFilenameAsync(imageUrl) ?? "";
            if (!string.IsNullOrEmpty(imageUrl) && !imageUrl.Equals(fullPathImage)) // for old data save filename in  db
            {
                ActivitiesEntity activityEntity = await _activitiesEntityRepo.Get(activityEntityId);
                activityEntity.ImageUrl = fullPathImage;
                await _activitiesEntityRepo.Update();
            }

            return fullPathImage;
        }

        //transform View Model
        public async Task<ViewDetailCompetitionActivity> TransformViewDetailCompetitionActivity(CompetitionActivity competitionActivity)
        {

            //List Activities Entity
            List<ViewActivitiesEntity> ListViewAE = new List<ViewActivitiesEntity>();

            List<ActivitiesEntity> ActivitiesEntities = competitionActivity.ActivitiesEntities.ToList();

            if (ActivitiesEntities != null)
            {
                foreach (ActivitiesEntity ActivitiesEntity in ActivitiesEntities)
                {
                    ViewActivitiesEntity viewActivitiesEntity = new ViewActivitiesEntity()
                    {
                        Id = ActivitiesEntity.Id,
                        CompetitionActivityId = ActivitiesEntity.CompetitionActivityId,
                        ImageUrl = await GetUrlImageActivityEntity(ActivitiesEntity.ImageUrl, ActivitiesEntity.Id),
                        Name = ActivitiesEntity.Name,
                    };
                    //
                    ListViewAE.Add(viewActivitiesEntity);
                }
            }

            //Who Create This Task
            User creator = await _userRepo.Get(competitionActivity.CreatorId);


            //List Member Takes Activity
            List<MemberTakesActivity> membersTakesActivity = competitionActivity.MemberTakesActivities.ToList();
            List<ViewMemberTakesActivity> listViewMTA = new List<ViewMemberTakesActivity>();
            if (membersTakesActivity != null)
            {

                foreach (MemberTakesActivity memberTakesActivity in membersTakesActivity)
                {
                    User booker = await _userRepo.Get(memberTakesActivity.BookerId);

                    Member mem = await _memberRepo.Get(memberTakesActivity.MemberId);

                    ViewMemberTakesActivity MTA = new ViewMemberTakesActivity()
                    {
                        Id = memberTakesActivity.Id,
                        CompetitionActivityId = memberTakesActivity.CompetitionActivityId,
                        BookerId = memberTakesActivity.BookerId,
                        BookerName = booker.Fullname,
                        MemberId = memberTakesActivity.MemberId,
                        MemberName = mem.User.Fullname,
                        MemberImg = mem.User.Avatar
                    };
                    listViewMTA.Add(MTA);
                }
            }


            return new ViewDetailCompetitionActivity()
            {
                Id = competitionActivity.Id,
                CompetitionId = competitionActivity.CompetitionId,
                Name = competitionActivity.Name,
                Description = competitionActivity.Description,
                //SeedsCode = competitionActivity.SeedsCode,
                SeedsPoint = competitionActivity.SeedsPoint,
                NumOfMember = competitionActivity.NumOfMember,
                Ending = competitionActivity.Ending,
                CreateTime = competitionActivity.CreateTime,
                Priority = competitionActivity.Priority,
                Status = competitionActivity.Status,
                CreatorId = creator.Id,
                CreatorName = creator.Fullname,
                CreatorEmail = creator.Email,
                ActivitiesEntities = ListViewAE,
                MemberTakesActivities = listViewMTA
            };
        }

        private async Task<ViewDetailMemberTakesActivity> TransferViewDetailMTA(MemberTakesActivity mta)
        {
            // Booker           
            User booker = await _userRepo.Get(mta.BookerId);

            // Member
            Member m2 = await _memberRepo.Get(mta.MemberId);
            User member = await _userRepo.Get(m2.UserId);

            return new ViewDetailMemberTakesActivity()
            {
                Id = mta.Id,
                MemberId = mta.MemberId,
                MemberName = member.Fullname,
                CompetitionActivityId = mta.CompetitionActivityId,
                BookerId = mta.BookerId,
                BookerName = booker.Fullname
            };
        }

        //generate Seed code length 8
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

        //check exist code
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


        //Check date 
        // COC < Ending
        private bool CheckDate(DateTime endTime)
        {
            DateTime currentTime = new LocalTime().GetLocalTime().DateTime;
            int result = DateTime.Compare(endTime, currentTime);

            return (result > 0) ? true : false;
        }

        private async Task<bool> CheckMemberInCompetition(string Token, int CompetitionId, int ClubId, bool isOrganization)
        {
            //------------- CHECK Competition in system
            bool isExisted = await _competitionRepo.CheckExistedCompetition(CompetitionId);
            if (!isExisted) throw new ArgumentException("Competition or Event not found ");

            //------------- CHECK Club in system
            isExisted = await _clubRepo.CheckExistedClub(ClubId);
            if (!isExisted) throw new ArgumentException("Club in not found");

            //------------- CHECK Is Member in Club
            int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(Token, "Id"), ClubId);
            if (memberId.Equals(0)) throw new UnauthorizedAccessException("You aren't member in Club");

            //------------- CHECK User is in CompetitionManger table                
            MemberInCompetition competitionManager = await _memberInCompetitionRepo.GetMemberInCompetition(CompetitionId, memberId);
            if (competitionManager == null) throw new UnauthorizedAccessException("You do not in Competition Managers");

            if (isOrganization && competitionManager.CompetitionRoleId >= 3) // accept competitionRoleId 1,2
                throw new UnauthorizedAccessException("Only role Manager can do this action");

            return true;
        }
    }
}
