using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.InfluencerInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.InfluencerRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Business.Services.InfluencerSvc
{
    public class InfluencerService : IInfluencerService
    {
        private readonly IInfluencerRepo _influencerRepo;
        private readonly ICompetitionManagerRepo _competitionManagerRepo;
        private readonly IInfluencerInCompetitionRepo _influencerInCompetitionRepo;

        private readonly IFileService _fileService;
        private JwtSecurityTokenHandler _tokenHandler;

        public InfluencerService(IInfluencerRepo influencerRepo, IFileService fileService, ICompetitionManagerRepo competitionManagerRepo, 
                                    IInfluencerInCompetitionRepo influencerInCompetitionRepo)
        {
            _influencerRepo = influencerRepo;
            _competitionManagerRepo = competitionManagerRepo;
            _influencerInCompetitionRepo = influencerInCompetitionRepo;
            _fileService = fileService;
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        public async Task Delete(int id)
        {
            Influencer influencer = await _influencerRepo.Get(id);
            if (influencer == null) throw new NullReferenceException("Not found this influencer");
            await _influencerRepo.Delete(influencer);
        }

        public async Task<PagingResult<ViewInfluencer>> GetByCompetition(int competitionId, PagingRequest request)
        {
            PagingResult<ViewInfluencer> result = await _influencerRepo.GetByCompetition(competitionId, request);
            if (result == null) throw new NullReferenceException();
            return result;
        }

        public async Task<ViewInfluencer> Insert(InfluencerInsertModel model, string token)
        {
            int userId = DecodeToken(token, "Id");
            //check role
            bool isValidManager = _competitionManagerRepo.CheckValidManagerByUser(model.CompetitionId, userId);
            if (!isValidManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            string avatar = await _fileService.UploadFile(model.ImageUrl);
            Influencer influencer = new Influencer()
            {
                Name = model.Name,
                ImageUrl = avatar
            };

            int id = await _influencerRepo.Insert(influencer);
            if (id > 0)
            {
                InfluencerInCompetition influencerInCompetition = new InfluencerInCompetition()
                {
                    InfluencerId = id,
                    CompetitionId = model.CompetitionId
                };
                await _influencerInCompetitionRepo.Insert(influencerInCompetition);
            }

            return await _influencerRepo.GetById(id);
        }

        // update name
        public async Task Update(InfluencerUpdateModel model, string token)
        {
            // check role
            int userId = DecodeToken(token, "Id");
            bool isValidManager = _competitionManagerRepo.CheckValidManagerByUser(model.CompetitionId, userId);
            if (!isValidManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");
            // update
            Influencer influencer = await _influencerRepo.Get(model.Id);
            if (influencer == null) throw new NullReferenceException("Not found this influencer");
            influencer.Name = model.Name;
            await _influencerRepo.Update();
        }

        // update image
        //public async Task Update(int id, IFormFile imageFile, string token)
        //{
        //    // check role

        //    // update
        //    Influencer influencer = await _influencerRepo.Get(id);
        //    if (influencer == null) throw new NullReferenceException("Not found this influencer");



        //    await _fileService.UploadFile(influencer.ImageUrl, imageFile);


        //}
    }
}
