using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.InfluencerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Business.Services.InfluencerSvc
{
    public class InfluencerService : IInfluencerService
    {
        private readonly IInfluencerRepo _influencerRepo;
        private readonly IMemberRepo _memberRepo;
        private readonly IClubRepo _clubRepo;
        private readonly IFileService _fileService;

        

        private JwtSecurityTokenHandler _tokenHandler;

        public InfluencerService(IInfluencerRepo influencerRepo, IMemberRepo memberRepo, IClubRepo clubRepo, IFileService fileService)
        {
            _influencerRepo = influencerRepo;
            _memberRepo = memberRepo;
            _clubRepo = clubRepo;
            _fileService = fileService;          
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        private async Task<bool> CheckRoleUser(int userId, int clubId)
        {
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, clubId);
            return (clubRoleId.Equals(1)) ? true : false; // if clubRole is leader
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
            try
            {
                //int UserId = DecodeToken(token, "Id");
                ////check role
                //List<int> clubIds = await _clubRepo.GetByCompetition(model.CompetitionId);
                //foreach (int clubId in clubIds)
                //{
                //    bool isValid = await CheckRoleUser(UserId, clubId);
                //    if (!isValid) throw new UnauthorizedAccessException("You do not have permission to access this resource");
                //}

                //Influencer influencer = new Influencer()
                //{
                //    Name = model.Name,

                //};
                //int id = await _influencerRepo.Insert(influencer, model.CompetitionId);
                //return await _influencerRepo.GetById(id);
                throw new NotImplementedException();

                
            }
            catch (Exception)
            {
                throw;
            }

        }

        // update name
        public async Task Update(InfluencerUpdateModel model, string token)
        {
                 
            try
            {
                // check role

                // update
                Influencer influencer = await _influencerRepo.Get(model.Id);
                if (influencer == null) throw new NullReferenceException("Not found this influencer");
                influencer.Name = model.Name;
                await _influencerRepo.Update();
            }
            catch (Exception)
            {
                throw;
            }

        }

        // update image
        public async Task Update(int id, IFormFile imageFile, string token)
        {
            // check role

            // update
            Influencer influencer = await _influencerRepo.Get(id);
            if (influencer == null) throw new NullReferenceException("Not found this influencer");



            await _fileService.UploadFile(influencer.ImageUrl, imageFile);


        }
    }
}
