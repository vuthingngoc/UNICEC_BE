using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Business.Services.TeamSvc
{
    public class TeamService : ITeamService
    {
        private ITeamRepo _teamRepo;

        public TeamService(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewTeam>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeam> GetByTeamId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(TeamUpdateModel team)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeam> InsertTeam(TeamInsertModel model, string token)
        {
            throw new NotImplementedException();

            //đã vào thấy screen này thì có nghĩa là đã trở thành Paricipant Check phase

            //---------------------------------------------------------------------INSERT
            //flow insert -- TEAM
            //CHECK Null
            //CHECK có phải là participant hay là không ?
            //CHECK số lượng team trong 1 cuộc thi < Number Of Team
            //Add vào [Participant in Team] -> thằng tạo là leader của Team [TeamRole]
            //TeamStatus  
            //0.Available -> Can Join
            //1.IsLocked  -> Full Member
            //2.Error     -> Erroe when insert


            //flow insert -- PARTICIPANT IN TEAM (CompetitionId, InvitedCode)
            //CHECK Null
            //CHECK Competition Id in system ?
            //CHECK InvitedCODE đó là của Team thuộc Competition nào?
            //CHECK Team Status
            //CHECK Member Of Team is available
            //add Participant by InvitedCode


            //ROLE OF Team Member
            //1.Leader
            //2.Member

            //---------------------------------------------------------------------UPDATE 


        }

        //Generate Seed code length 15
        private string GenerateSeedCode()
        {
            string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[15];
            string code = "";
            var random = new Random();

            for (int i = 0; i < chars.Length; i++)
            {
                code += string.Concat(codePool[random.Next(codePool.Length)]);
            }
            return code;
        }
        //----------------------------------------------------------------------------------------Check
        //check exist code
        private async Task<string> CheckExistCode()
        {
            //auto generate seedCode
            bool check = true;
            string seedCode = "";
            while (check)
            {
                string generateCode = GenerateSeedCode();
                check = await _teamRepo.CheckExistCode(generateCode);
                seedCode = generateCode;
            }
            return seedCode;
        }

    }
}
