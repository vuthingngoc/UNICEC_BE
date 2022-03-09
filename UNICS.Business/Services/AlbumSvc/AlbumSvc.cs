using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.AlbumRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Album;

namespace UNICS.Business.Services.AlbumSvc
{
    public class AlbumSvc : IAlbumSvc
    {
        private IAlbumRepo _albumRepo;

        public AlbumSvc(IAlbumRepo albumRepo)
        {
            _albumRepo = albumRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewAlbum>> GetAlbumByCompetitionId(int competitionId)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewAlbum>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewAlbum> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
