using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;

namespace UniCEC.Business.Services.CompetitionEntitySvc
{
    public class CompetitionEntityService : ICompetitionEntityService
    {
        private ICompetitionEntityRepo _competitionEntityRepo;
       

        public CompetitionEntityService(ICompetitionEntityRepo competitionEntityRepo)
        { 
            _competitionEntityRepo = competitionEntityRepo;
           
        }
        
    }
}
