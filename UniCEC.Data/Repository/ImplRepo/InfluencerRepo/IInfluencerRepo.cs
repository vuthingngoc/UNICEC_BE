using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Data.Repository.ImplRepo.InfluencerRepo
{
    public interface IInfluencerRepo : IRepository<Influencer>
    {
        public Task<PagingResult<ViewInfluencer>> GetByCompetition(int competition);
    }
}
