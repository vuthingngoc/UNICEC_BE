using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantRepo
{
    public interface IParticipantRepo : IRepository<Participant>
    {
       // public Task<bool> CheckStudentInCom_In_Dep(List<int> listComp_In_Dep_Id, User stuInfo);

        public Task<bool> CheckDuplicateUser(User stuInfo, int CompetitionId);

        public Task<bool> CheckNumOfParticipant(int CompetitionId, int numOfCompetition);

        public Task<Participant> ParticipantInCompetition(int UserId, int CompetitionId);

        public Task<List<Participant>> ListParticipantToAddPoint(int CompetitionId);

        public Task<int> NumOfParticipant(int CompetitionId);


    }
}
