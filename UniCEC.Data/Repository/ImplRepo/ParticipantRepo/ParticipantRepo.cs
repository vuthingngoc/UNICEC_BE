using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.Participant;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.RequestModels;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantRepo
{
    public class ParticipantRepo : Repository<Participant>, IParticipantRepo
    {
        public ParticipantRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckDuplicateUser(User stuInfo, int CompetitionId)
        {
            // chưa được add
            bool result = false;

            //xem user đó có đã join trong competition đó hay chưa 
            //Participant -> User
            var query = from p in context.Participants
                        where p.CompetitionId == CompetitionId && p.StudentId == stuInfo.Id
                        select p;

            Participant participant = await query.FirstOrDefaultAsync();
            if (participant != null)
            {
                //có nghĩa là add nó r 
                return true;
            }

            return result;
        }

        //
        //public async Task<bool> CheckStudentInCom_In_Dep(List<int> listComp_In_Dep_Id, User stuInfo)
        //{
        //    bool result = false;

        //    foreach (var Com_In_Dep_Id in listComp_In_Dep_Id)
        //    {
        //        //tìm sinh viên thuộc 1 trường có mã nghành đó 
        //        //stu -> major -> department -> department in University
        //        //nó cứ đưa list Department Id vào nếu thỏa đúng thì nó sẽ trả ra user đó 
        //        var query = from stu in context.Users
        //                    where stu.Id == stuInfo.Id
        //                    from dep in context.Departments
        //                    where dep.Id == stuInfo.MajorId
        //                    from mj in context.Majors
        //                    where mj.Id == dep.Id && mj.Id == Com_In_Dep_Id
        //                    select stu;
        //        //
        //        User student = await query.FirstOrDefaultAsync();
        //        //
        //        if (student != null)
        //        {
        //            return true;
        //        }
        //    }

        //    return result;
        //}

        //
        public bool CheckNumOfParticipant(int CompetitionId, int NumOfCompetition)
        {

            var query = from p in context.Participants
                        where p.CompetitionId == CompetitionId
                        select p;

            int totalNumOfParticipant = query.Count();

            if (totalNumOfParticipant < NumOfCompetition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Participant> ParticipantInCompetition(int UserId, int CompetitionId)
        {
            var query = from p in context.Participants
                        where p.CompetitionId == CompetitionId && p.StudentId == UserId
                        select p;
            Participant participant = await query.FirstOrDefaultAsync();
            if (participant != null)
            {
                return participant;
            }
            else
            {
                return null;
            }
        }

        public async Task<int> NumOfParticipant(int CompetitionId)
        {
            var query = from p in context.Participants
                        where p.CompetitionId == CompetitionId
                        select p;

            int totalNumOfParticipant = await query.CountAsync();

            return totalNumOfParticipant;
        }

        public async Task<List<Participant>> ListParticipantToAddPoint(int CompetitionId)
        {
            List<Participant> participants = await (from p in context.Participants                                                   
                                                    where p.CompetitionId == CompetitionId && p.Attendance == true
                                                    select p).ToListAsync();

            return (participants.Count > 0) ? participants : null;
        }


        public async Task<List<Participant>> ListParticipant(int CompetitionId)
        {
            List<Participant> participants = await (from p in context.Participants                                                  
                                                    where p.CompetitionId == CompetitionId 
                                                    select p).ToListAsync();

            return (participants.Count > 0) ? participants : null;
        }

        public async Task<ViewParticipant> GetByCompetitionId(int competitionId, int userId)
        {
            Participant participant = await (from p in context.Participants
                                             join c in context.Competitions on p.CompetitionId equals c.Id
                                             where competitionId == c.Id && p.StudentId == userId
                                             select p).FirstOrDefaultAsync();
            User user = null;
            if (participant != null)
            {
                user = await (from u in context.Users
                              where u.Id == userId
                              select u).FirstOrDefaultAsync();
            }
            return participant != null ? new ViewParticipant
            {
                Id = participant.Id,
                CompetitionId = participant.CompetitionId,
                StudentId = participant.StudentId,
                Avatar = (user != null) ? user.Avatar : null,
                RegisterTime = participant.RegisterTime,
                Attendance = participant.Attendance,
                StudentCode = user.StudentCode,
                StudentName = user.Fullname
            } : null;

        }

        public async Task<PagingResult<ViewParticipant>> GetByConditions(ParticipantRequestModel request)
        {
            IQueryable<Participant> query;

            if (!request.HasTeam.HasValue)
            {
                query = from p in context.Participants
                        where p.CompetitionId == request.CompetitionId
                        select p;

                //attendance
                if (request.HasAttendance.HasValue) query = query.Where(p => p.Attendance == request.HasAttendance.Value);

                int totalCount = query.Count();

                List<ViewParticipant> listvp = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                  .Select(x => new ViewParticipant
                                                  {
                                                      Id = x.Id,
                                                      StudentId = x.StudentId,
                                                      CompetitionId = x.CompetitionId,
                                                      RegisterTime = x.RegisterTime,
                                                      Attendance = x.Attendance,
                                                      Avatar = x.Student.Avatar,
                                                      StudentCode = x.Student.StudentCode,
                                                      StudentName = x.Student.Fullname
                                                  }).ToListAsync();
                return listvp.Count > 0 ? new PagingResult<ViewParticipant>(listvp, totalCount, request.CurrentPage, request.PageSize) : null;

            }
            else
            {
                if (request.HasTeam.Value == true)
                {
                    query = from p in context.Participants
                            join pit in context.ParticipantInTeams on p.Id equals pit.ParticipantId
                            where p.CompetitionId == request.CompetitionId
                                  && pit.Status == ParticipantInTeamStatus.InTeam
                            select p;

                    //attendance
                    if (request.HasAttendance.HasValue) query = query.Where(p => p.Attendance == request.HasAttendance.Value);

                    int totalCount = query.Count();
                    List<ViewParticipant> listvp = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                  .Select(x => new ViewParticipant
                                                  {
                                                      Id = x.Id,
                                                      StudentId = x.StudentId,
                                                      CompetitionId = x.CompetitionId,
                                                      RegisterTime = x.RegisterTime,
                                                      Attendance = x.Attendance,
                                                      Avatar = x.Student.Avatar,
                                                      StudentCode = x.Student.StudentCode,
                                                      StudentName = x.Student.Fullname
                                                  }).ToListAsync();
                    return listvp.Count > 0 ? new PagingResult<ViewParticipant>(listvp, totalCount, request.CurrentPage, request.PageSize) : null;

                }
                else
                {
                    List<Participant> AllParticipant = await (from p in context.Participants
                                                              where p.CompetitionId == request.CompetitionId
                                                              select p).ToListAsync();

                    List<Participant> ParticipantHasTeam = await (from p in context.Participants
                                                                  join pit in context.ParticipantInTeams on p.Id equals pit.ParticipantId
                                                                  where p.CompetitionId == request.CompetitionId
                                                                        && pit.Status == ParticipantInTeamStatus.InTeam
                                                                  select p).ToListAsync();



                    AllParticipant = AllParticipant.Except(ParticipantHasTeam).ToList();

                   //attendance
                   if(request.HasAttendance.HasValue) AllParticipant = AllParticipant.Where(ap => ap.Attendance == request.HasAttendance.Value).ToList();

                    //==> sau tất cả thì AllParticipant sẽ chứa list Participant chưa có team
                    int totalCount = AllParticipant.Count;

                    AllParticipant = AllParticipant.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

                    //
                    List<ViewParticipant> listvp = new List<ViewParticipant>();
                    //
                    foreach (Participant ap in AllParticipant)
                    {
                       ViewParticipant vp = new ViewParticipant
                       {
                           Id = ap.Id,
                           StudentId = ap.StudentId,
                           CompetitionId = ap.CompetitionId,
                           RegisterTime = ap.RegisterTime,
                           Attendance  = ap.Attendance,
                           Avatar = ap.Student.Avatar,
                           StudentCode = ap.Student.StudentCode,
                           StudentName = ap.Student.Fullname
                       };
                        listvp.Add(vp);
                    }

                    return listvp.Count > 0 ? new PagingResult<ViewParticipant>(listvp, totalCount, request.CurrentPage, request.PageSize) : null;

                }
            }
            
        }

       
    }
}
