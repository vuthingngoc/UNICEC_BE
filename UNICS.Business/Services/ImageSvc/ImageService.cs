using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.ImageRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Image;

namespace UNICS.Business.Services.ImageSvc
{
    public class ImageService : IImageService
    {
        private IImageRepo _imageRepo;

        public ImageService(IImageRepo imageRepo)
        {
            _imageRepo = imageRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewImage>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewImage>> GetByAlbumId(int albumId)
        {
            throw new NotImplementedException();
        }

        public Task<ViewImage> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(ImageInsertModel image)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ImageUpdateModel image)
        {
            throw new NotImplementedException();
        }
    }
}
