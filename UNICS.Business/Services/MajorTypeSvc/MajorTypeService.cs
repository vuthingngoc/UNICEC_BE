using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.MajorTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.MajorType;

namespace UNICS.Business.Services.MajorTypeSvc
{
    public class MajorTypeService : IMajorTypeService
    {
        private IMajorTypeRepo _majorTypeRepo;

        public MajorTypeService(IMajorTypeRepo majorTypeRepo)
        {
            _majorTypeRepo = majorTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewMajorType>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMajorType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(MajorTypeInsertModel majorType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewMajorType majorType)
        {
            throw new NotImplementedException();
        }
    }
}
