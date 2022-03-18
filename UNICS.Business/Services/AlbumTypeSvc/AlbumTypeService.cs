using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.AlbumTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.AlbumType;

namespace UNICS.Business.Services.AlbumTypeSvc
{
    public class AlbumTypeService : IAlbumTypeService
    {
        private ICompetitionEntityRepo _albumTypeRepo;

        public AlbumTypeService(ICompetitionEntityRepo albumTypeRepo)
        {
            _albumTypeRepo = albumTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewAlbumType>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewAlbumType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(AlbumTypeInsertModel albumType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewAlbumType albumType)
        {
            throw new NotImplementedException();
        }
    }
}
