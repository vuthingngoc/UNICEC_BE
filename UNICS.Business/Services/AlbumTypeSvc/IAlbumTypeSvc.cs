using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.AlbumType;

namespace UNICS.Business.Services.AlbumTypeSvc
{
    public interface IAlbumTypeSvc
    {
        public ViewAlbumType getAll(PagingRequest request);
        public ViewAlbumType getById(int id);
    }
}
