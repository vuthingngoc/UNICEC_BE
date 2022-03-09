using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Business.Services.AlbumTypeSvc;
using UNICS.Data.Repository.ImplRepo.AlbumTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.AlbumType;

namespace UNICS.Business.Services.AreaSvc
{
    public class AlbumTypeSvc : IAlbumTypeSvc
    {
        private IAlbumTypeRepo _albumTypeRepo;

        public AlbumTypeSvc(IAlbumTypeRepo albumTypeRepo)
        {
            _albumTypeRepo = albumTypeRepo;
        }

        public ViewAlbumType getAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public ViewAlbumType getById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
