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
            if (competitionRoundType == null) throw new NullReferenceException("Not found this match type");

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
            ViewCompetitionRoundType matchType = await _competitionRoundTypeRepo.GetById(id);
            if (matchType == null) throw new NullReferenceException("Not found this match type");

            bool isSystemAdmin = IsSystemAdmin(token);
            if(!isSystemAdmin && matchType.Status.Equals(false))
                throw new NullReferenceException("Not found this match type");

            return matchType;
        }

        public async Task<List<ViewCompetitionRoundType>> GetByConditions(CompetitionRoundTypeRequestModel request, string token)
        {
            bool isSystemAdmin = IsSystemAdmin(token);
            if(!isSystemAdmin) request.Status = true; // default status

            List<ViewCompetitionRoundType> matchTypes = await _competitionRoundTypeRepo.GetByConditions(request);
            if (matchTypes == null) throw new NullReferenceException("Not found any match types");
            return matchTypes;
        }

        public async Task<ViewCompetitionRoundType> Insert(CompetitionRoundTypeInsertModel model, string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (string.IsNullOrEmpty(model.Name)) throw new ArgumentException("Name Null");

            bool isDuplicated = await _competitionRoundTypeRepo.CheckDuplicatedMatchType(model.Name);
            if (isDuplicated) throw new ArgumentException("Duplicated match type");

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

            if (model.Id.Equals(0)) throw new ArgumentException("MatchTypeId Null");

            CompetitionRoundType competitionRoundType = await _competitionRoundTypeRepo.Get(model.Id);
            if (competitionRoundType == null) throw new NullReferenceException("Not found this match type");

            if (!string.IsNullOrEmpty(model.Name))
            {
                bool isDuplicated = await _competitionRoundTypeRepo.CheckDuplicatedMatchType(model.Name);
                if (isDuplicated) throw new ArgumentException("Duplicated match type");
                competitionRoundType.Name = model.Name;
            }

            if (model.Status.HasValue) competitionRoundType.Status = model.Status.Value;

            await _competitionRoundTypeRepo.Update();
        }
    }
}
