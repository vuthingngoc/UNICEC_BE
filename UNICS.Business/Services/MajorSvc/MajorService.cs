using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.MajorRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Major;

namespace UNICS.Business.Services.MajorSvc
{
    public class MajorService : IMajorService
    {
        private IMajorRepo _majorRepo;

        public MajorService(IMajorRepo majorRepo)
        {
            _majorRepo = majorRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewMajor>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMajor> GetByMajorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMajor> Insert(MajorInsertModel major)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewMajor major)
        {
            throw new NotImplementedException();
        }
    }
}
