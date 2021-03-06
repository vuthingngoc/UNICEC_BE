using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Enum;

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
        public async Task<bool> CheckNumOfParticipant(int CompetitionId, int NumOfCompetition)
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
                                                    join pit in context.ParticipantInTeams on p.Id equals pit.ParticipantId
                                                    where p.CompetitionId == CompetitionId && p.Attendance == true && pit.Status == ParticipantInTeamStatus.InTeam
                                                    select p).ToListAsync();

            return (participants.Count > 0) ? participants : null;
        }
    }
}
