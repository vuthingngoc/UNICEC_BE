using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MatchTypeRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.MatchType;

namespace UniCEC.Business.Services.MatchTypeSvc
{
    public class MatchTypeService : IMatchTypeService
    {
        private IMatchTypeRepo _matchTypeRepo;
        private DecodeToken _decodeToken;

        public MatchTypeService(IMatchTypeRepo matchTypeRepo)
        {
            _matchTypeRepo = matchTypeRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task Delete(int id, string token) // for system  admin
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            MatchType matchType = await _matchTypeRepo.Get(id);
            if (matchType == null) throw new NullReferenceException("Not found this match type");

            matchType.Status = false; // delete status
            await _matchTypeRepo.Update();
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

        public async Task<ViewMatchType> GetById(int id, string token)
        {
            ViewMatchType matchType = await _matchTypeRepo.GetById(id);
            if (matchType == null) throw new NullReferenceException("Not found this match type");

            bool isSystemAdmin = IsSystemAdmin(token);
            if(!isSystemAdmin && matchType.Status.Equals(false))
                throw new NullReferenceException("Not found this match type");

            return matchType;
        }

        public async Task<List<ViewMatchType>> GetByConditions(MatchTypeRequestModel request, string token)
        {
            bool isSystemAdmin = IsSystemAdmin(token);
            if(!isSystemAdmin) request.Status = true; // default status

            List<ViewMatchType> matchTypes = await _matchTypeRepo.GetByConditions(request);
            if (matchTypes == null) throw new NullReferenceException("Not found any match types");
            return matchTypes;
        }

        public async Task<ViewMatchType> Insert(MatchTypeInsertModel model, string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (string.IsNullOrEmpty(model.Name)) throw new ArgumentException("Name Null");

            bool isDuplicated = await _matchTypeRepo.CheckDuplicatedMatchType(model.Name);
            if (isDuplicated) throw new ArgumentException("Duplicated match type");

            MatchType matchType = new MatchType()
            {
                Name = model.Name,
                Status = true // default status when insert
            };

            int id = await _matchTypeRepo.Insert(matchType);
            return new ViewMatchType()
            {
                Id = id,
                Name = matchType.Name,
                Status = matchType.Status,
            };
        }

        public async Task Update(MatchTypeUpdateModel model, string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            MatchType matchType = await _matchTypeRepo.Get(model.Id);
            if (matchType == null) throw new NullReferenceException("Not found this match type");

            if (!string.IsNullOrEmpty(model.Name))
            {
                bool isDuplicated = await _matchTypeRepo.CheckDuplicatedMatchType(model.Name);
                if (isDuplicated) throw new ArgumentException("Duplicated match type");
                matchType.Name = model.Name;
            }

            if(model.Status.HasValue) matchType.Status = model.Status.Value;

            await _matchTypeRepo.Update();
        }
    }
}
