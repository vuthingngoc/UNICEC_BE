using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.SponsorRepo
{
    public interface ISponsorRepo : IRepository<Sponsor>
    {
        public Task<bool> CheckSponsorIsCreated(int UserId);
    }
}
