﻿using System;
using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionType;

namespace UNICS.Business.Services.CompetitionTypeSvc
{
    public class CompetitionTypeSvc : ICompetitionTypeSvc
    {
        private ICompetitionTypeRepo _competitionTypeRepo;

        public CompetitionTypeSvc(ICompetitionTypeRepo competitionTypeRepo)
        {
            _competitionTypeRepo = competitionTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<CompetitionType>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<CompetitionType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(CompetitionTypeInsertModel competitionType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCompetitionType competitionType)
        {
            throw new NotImplementedException();
        }
    }
}
