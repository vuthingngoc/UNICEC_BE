﻿using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CompetitionRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Competition;

namespace UNICS.Business.Services.CompetitionSvc
{
    public class CompetitionSvc : ICompetitionSvc
    {
        private ICompetitionRepo _competitionRepo;

        public CompetitionSvc(ICompetitionRepo competitionRepo)
        {
            _competitionRepo = competitionRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCompetition>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetition> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(CompetitionInsertModel competition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCompetition competition)
        {
            throw new NotImplementedException();
        }
    }
}
