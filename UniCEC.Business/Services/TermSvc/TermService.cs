using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.TermSvc
{
    public class TermService : ITermService
    {
        private ITermRepo _termRepo;
        public TermService(ITermRepo termRepo)
        {
            _termRepo = termRepo;
        }

        public async Task<PagingResult<ViewTerm>> GetByClub(int clubId, PagingRequest request)
        {
            PagingResult<ViewTerm> terms = await _termRepo.GetByClub(clubId, request);
            if (terms == null) throw new NullReferenceException("Not found any term");
            return terms;
        }

        public async Task<PagingResult<ViewTerm>> GetByConditions(int clubId, TermRequestModel request)
        {
            PagingResult<ViewTerm> terms = await _termRepo.GetByConditions(clubId, request);
            if (terms == null) throw new NullReferenceException("Not found any term");
            return terms;
        }

        public async Task<ViewTerm> GetById(int clubId, int id)
        {
            ViewTerm term = await _termRepo.GetById(clubId, id);
            if (term == null) throw new NullReferenceException("Not found this term");
            return term;
        }

        public async Task<ViewTerm> Insert(TermInsertModel term)
        {
            if (string.IsNullOrEmpty(term.Name) || term.CreateTime == DateTime.MinValue
                || term.EndTime == DateTime.MinValue) throw new ArgumentNullException("Name Null || CreateTime Null || EndTime Null"); 

            Term termObject = new Term()
            {
                Name = term.Name,
                CreateTime = term.CreateTime,
                EndTime = term.EndTime,
                Status = true // default status when insert
            };
            int id = await _termRepo.Insert(termObject);
            return new ViewTerm()
            {
                Id = id,
                Name = term.Name,
                CreateTime = term.CreateTime,
                EndTime = term.EndTime
            };
        }

        public async Task Update(TermUpdateModel term)
        {
            if (string.IsNullOrEmpty(term.Name) || term.CreateTime == DateTime.MinValue
                || term.EndTime == DateTime.MinValue) throw new ArgumentNullException("Name Null || CreateTime Null || EndTime Null");

            Term termObject = await _termRepo.Get(term.Id);
            if (termObject == null) throw new NullReferenceException("Not found this term");

            if(!string.IsNullOrEmpty(term.Name)) termObject.Name = term.Name;
            if(term.CreateTime != DateTime.MinValue) termObject.CreateTime = term.CreateTime;
            if (term.EndTime != DateTime.MinValue) termObject.EndTime = term.EndTime;
            if (term.Status.HasValue) termObject.Status = term.Status.Value;
            await _termRepo.Update();
        }

        public async Task Delete(int id)
        {
            Term termObject = await _termRepo.Get(id);
            if (termObject == null) throw new NullReferenceException("Not found this term");

            termObject.Status = false;
            await _termRepo.Update();
        }
    }
}
