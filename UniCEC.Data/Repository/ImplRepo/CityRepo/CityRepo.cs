using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CityRepo
{
    public class CityRepo : Repository<City>, ICityRepo
    {
        public CityRepo(UniCECContext context) : base(context)
        {

        }
    }
}
