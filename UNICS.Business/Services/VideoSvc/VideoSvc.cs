using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.VideoRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Video;

namespace UNICS.Business.Services.VideoSvc
{
    public class VideoSvc : IVideoSvc
    {
        private IVideoRepo _videoRepo;

        public VideoSvc(IVideoRepo videoRepo)
        {
            _videoRepo = videoRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewVideo>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewVideo> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(VideoInsertModel video)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(VideoUpdateModel video)
        {
            throw new NotImplementedException();
        }
    }
}
