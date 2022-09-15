using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundTypeRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.CompetitionRoundType;

namespace UniCEC.Business.Services.CompetitionRoundTypeSvc
{
    public class CompetitionRoundTypeService : ICompetitionRoundTypeService
    {
        private ICompetitionRoundTypeRepo _competitionRoundTypeRepo;
        private DecodeToken _decodeToken;

        public CompetitionRoundTypeService(ICompetitionRoundTypeRepo competitionRoundTypeRepo)
        {
            _competitionRoundTypeRepo = competitionRoundTypeRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task Delete(int id, string token) // for system  admin
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            CompetitionRoundType competitionRoundType = await _competitionRoundTypeRepo.Get(id);
            if (competitionRoundType == null) throw new NullReferenceException("Not found this round type");

            competitionRoundType.Status = false; // delete status
            await _competitionRoundTypeRepo.Update();
        }

        private bool IsSystemAdmin(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                int roleId = _decodeToken.Decode(token, "RoleId");
                return roleId.Equals(4);
            }

            return false;
        }

        public async Task<ViewCompetitionRoundType> GetById(int id, string token)
        {
            ViewCompetitionRoundType competitionRoundType = await _competitionRoundTypeRepo.GetById(id);
            if (competitionRoundType == null) throw new NullReferenceException("Not found this round type");

            bool isSystemAdmin = IsSystemAdmin(token);
            if(!isSystemAdmin && competitionRoundType.Status.Equals(false))
                throw new NullReferenceException("Not found this round type");

            return competitionRoundType;
        }

        public async Task<List<ViewCompetitionRoundType>> GetByConditions(CompetitionRoundTypeRequestModel request, string token)
        {
            bool isSystemAdmin = IsSystemAdmin(token);
            if(!isSystemAdmin) request.Status = true; // default status

            List<ViewCompetitionRoundType> competitionRoundTypes = await _competitionRoundTypeRepo.GetByConditions(request);
            if (competitionRoundTypes == null) throw new NullReferenceException("Not found any round types");
            return competitionRoundTypes;
        }

        public async Task<ViewCompetitionRoundType> Insert(CompetitionRoundTypeInsertModel model, string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (string.IsNullOrEmpty(model.Name)) throw new ArgumentException("Name Null");

            bool isDuplicated = await _competitionRoundTypeRepo.CheckDuplicatedCompetitionRoundType(model.Name);
            if (isDuplicated) throw new ArgumentException("Duplicated round type");

            CompetitionRoundType competitionRoundType = new CompetitionRoundType()
            {
                Name = model.Name,
                Status = true // default status when insert
            };

            int id = await _competitionRoundTypeRepo.Insert(competitionRoundType);
            return new ViewCompetitionRoundType()
            {
                Id = id,
                Name = competitionRoundType.Name,
                Status = competitionRoundType.Status,
            };
        }

        public async Task Update(CompetitionRoundTypeUpdateModel model, string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (model.Id.Equals(0)) throw new ArgumentException("RoundTypeId Null");

            CompetitionRoundType competitionRoundType = await _competitionRoundTypeRepo.Get(model.Id);
            if (competitionRoundType == null) throw new NullReferenceException("Not found this round type");

            if (!string.IsNullOrEmpty(model.Name))
            {
                bool isDuplicated = await _competitionRoundTypeRepo.CheckDuplicatedCompetitionRoundType(model.Name);
                if (isDuplicated) throw new ArgumentException("Duplicated round type");
                competitionRoundType.Name = model.Name;
            }

            if (model.Status.HasValue) competitionRoundType.Status = model.Status.Value;

            await _competitionRoundTypeRepo.Update();
        }
    }
}
